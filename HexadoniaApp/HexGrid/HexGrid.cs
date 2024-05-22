using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// using Managers;

namespace HexadoniaApp.HexGrid;

public class HexGrid : Node3D
{
     private Material _material;
     private Texture2D _cellOutlineTexture;

//     private NodePath _positionObjectNode;
     private Node3D _positionObject;

//     [Signal]
     public delegate void TouchEventHandler(Vector3 position);

//     [Signal]
     public delegate void MoveCellUpEventHandler(Vector3 position);

//     [Signal]
     public delegate void MoveCellDownEventHandler(Vector3 position);

     public const int ChunkCountX = 200;    // over 2km = (0.75 * 0.866025404 * 2) cell size * 200 chunks * 10 cells/chunk = 2.598km
     public const int ChunkCountZ = 200;    // over 2km = (0.75 * 2 - .75 / 2) cell size * 200 chunks * 10 cells/chunk = 2.250km
     private const int CellCountX = ChunkCountX * HexMetrics.ChunkSizeX;
     private const int CellCountZ = ChunkCountZ * HexMetrics.ChunkSizeZ;
     private const int ChunksStreamedX = 4;
     private const int ChunksStreamedZ = 5;

     private Dictionary<(int x, int z), HexGridChunk> _loadedChunks = new();

     private HexCell[] _cells;

     private (int x, int z)? _lastChunk = null;

     private ChunkBuilder _chunkBuilder;

     private HexCell _searchFromCell;
     private HexCell _searchToCell;

     public HexGrid()
     {
         _chunkBuilder = new ChunkBuilder();
         AddChild(_chunkBuilder);

//         _positionObject = GetNode<Node3D>(_positionObjectNode);
//
//         var startTime = Time.GetTicksUsec();
         if (!LoadGrid())
         {
             CreateCells();
         }
//         var endTime = Time.GetTicksUsec();
//         GD.Print("Startup time: ", endTime - startTime, "µs");

         // _lastChunk = GetCurrentOccupiedChunk();

//         Connect("Touch",new Callable(this,"OnTouch"));
//         Connect("MoveCellUp",new Callable(this,"OnMoveCellUp"));
//         Connect("MoveCellDown",new Callable(this,"OnMoveCellDown"));

//         var gameManager = GetNode<GameManager>("/root/World3D/GameManager");
//         gameManager.AfterGridLoaded(this);
     }

     public void Shutdown()
     {
         _chunkBuilder.Shutdown();
     }

     public HexCell GetCellAtPosition(Vector3 position)
     {
         position = ToLocal(position);
         var coordinates = HexCoordinates.FromPosition(position);
         var index = coordinates.X + coordinates.Z * CellCountX + coordinates.Z / 2;
         return _cells[index];
     }

     public HexCell GetCellByCoordinates(HexCoordinates coordinates)
     {
         return _cells[coordinates.X + coordinates.Z * CellCountX + coordinates.Z / 2];
     }

     public HexCell[] Cells => _cells;
     
     public HexCell GetCell(int x, int z)
     {
         return _cells[z * CellCountX + x];
     }
     
     public Vector3 GetCellPosition(int x, int z)
     {
         return _cells[z * CellCountX + x].Position;
     }

     private (int x, int z) GetCurrentOccupiedChunk()
     {
         var position = ToLocal(Vector3.Zero);//_positionObject.GlobalTransform.Origin);
         var coordinates = HexCoordinates.FromPosition(position);
         return ((coordinates.X + coordinates.Z / 2) / HexMetrics.ChunkSizeX, coordinates.Z / HexMetrics.ChunkSizeZ);
     }

     private void EnsureChunk(int x, int z)
     {
         _loadedChunks.TryGetValue((x, z), out var chunk);
         if (chunk == null)
         {
             var hexGridChunk = new HexGridChunk(_material, x, z, _cells, _chunkBuilder);
             hexGridChunk.Visible = true;
             _loadedChunks.Add((x, z), hexGridChunk);
             AddChild(hexGridChunk);
         }
         else
         {
             chunk.Visible = true;
         }
     }

     public override void _Process(double delta)
     {
         var currentChunk = GetCurrentOccupiedChunk();
         if (currentChunk == _lastChunk) return;

         foreach (var hexGridChunk in _loadedChunks)
         {
             hexGridChunk.Value.Visible = false;
         }

         for (var z = -ChunksStreamedZ; z <= ChunksStreamedZ; z++)
         {
             for (var x = -ChunksStreamedX; x <= ChunksStreamedX; x++)
             {
                 if (currentChunk.x + x is < 0 or >= ChunkCountX ||
                     currentChunk.z + z is < 0 or >= ChunkCountZ) continue;

                 EnsureChunk(currentChunk.x + x, currentChunk.z + z);
             }
         }

         foreach (var hexGridChunk in _loadedChunks.ToList().Where(hexGridChunk => hexGridChunk.Value.Visible == false))
         {
             hexGridChunk.Value.Delete();
             _loadedChunks.Remove(hexGridChunk.Key);
         }

         _lastChunk = currentChunk;
     }

     private void CreateCells()
     {
         _cells = new HexCell[CellCountX * CellCountZ];

         var noise = new FastNoiseLite();
         noise.SetSeed(Godot.Random.Randi());
         noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
         noise.SetFractalOctaves(3);
         noise.SetFractalLacunarity(4);
         noise.SetFrequency(0.02f);

         for (var z = 0; z < CellCountZ; z++)
         {
             for (var x = 0; x < CellCountX; x++)
             {
                 CreateCell(x, z);
             }
         }

         for (var z = 0; z < CellCountZ; z++)
         {
             for (var x = 0; x < CellCountX; x++)
             {
                 var noiseValue = (int)(noise.GetNoise(x, z) * 15);

                 var cellIndex = x + z * CellCountX;
                 _cells[cellIndex].SetColor(noiseValue switch
                 {
                     < -1 => Color.Blue,
                     > 6 => Color.Snow,
                     > 3 => Color.DimGray,
                     > 0 => Color.Brown,
                     _ => Color.SandyBrown,
                 });

                 _cells[cellIndex].Elevation = noiseValue;
             }
         }
         
         SaveGrid();
     }

     private void CreateCell(int x, int z)
     {
         var position = new Vector3(
             (x + z * 0.5f - z / 2) * (HexMetrics.InnerRadius * 2),
             0,
             z * (HexMetrics.OuterRadius * 1.5f));

         var cellIndex = x + z * CellCountX;
         var cell = new HexCell();
         _cells[cellIndex] = cell;
         cell.Enabled = true;
         cell.Position = position;
         cell.Coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
         cell.CellOutlineTexture = _cellOutlineTexture;

         if (x > 0)
         {
             cell.SetNeighbor(HexDirection.W, _cells[cellIndex - 1]);
         }

         if (z > 0)
         {
             if ((z & 1) == 0)
             {
                 cell.SetNeighbor(HexDirection.SE, _cells[cellIndex - CellCountX]);
                 if (x > 0)
                 {
                     cell.SetNeighbor(HexDirection.SW, _cells[cellIndex - CellCountX - 1]);
                 }
             }
             else
             {
                 cell.SetNeighbor(HexDirection.SW, _cells[cellIndex - CellCountX]);
                 if (x < CellCountX - 1)
                 {
                     cell.SetNeighbor(HexDirection.SE, _cells[cellIndex - CellCountX + 1]);
                 }
             }
         }
     }

     private void SaveGrid()
     {
//         if (!OS.HasFeature("editor")) return;
//
//         var fs = FileAccess.Open("res://HexGrid1.map", FileAccess.ModeFlags.Write);
//         foreach (var cellData in _cells.Select(c => (c.Color, c.Elevation)).ToArray())
//         {
//             fs.Store32(cellData.Color.ToRgba32());
//             fs.Store32((uint)cellData.Elevation);
//         }
//
//         fs.Flush();
     }

     private bool LoadGrid()
     {
         try
         {
//             var fs = FileAccess.Open("res://HexGrid1.map", FileAccess.ModeFlags.Read);
//             var cellData = new (Color color, int elevation)[CellCountX * CellCountZ];
//
//             var data = fs.GetBuffer(32000000);
//             var pos = 0;
//
//             var index = 0;
//             while (pos < data.Length)
//             {
//                 var a8 = data[pos++];
//                 var b8 = data[pos++];
//                 var g8 = data[pos++];
//                 var r8 = data[pos++];
//                 var elevation = (int)data[pos++];
//                 elevation += data[pos++] << 8;
//                 elevation += data[pos++] << 16;
//                 elevation += data[pos++] << 24;
//                 cellData[index].color = Color.Color8(r8, g8, b8, a8);
//                 cellData[index].elevation = (int)elevation;
//                 index++;
//             }
//
//             _cells = new HexCell[CellCountX * CellCountZ];
//
//             for (var z = 0; z < CellCountZ; z++)
//             {
//                 for (var x = 0; x < CellCountX; x++)
//                 {
//                     CreateCell(x, z);
//                 }
//             }
//
//             for (var z = 0; z < CellCountZ; z++)
//             {
//                 for (var x = 0; x < CellCountX; x++)
//                 {
//                     var cellIndex = x + z * CellCountX;
//                     _cells[cellIndex].SetColor(cellData[z * CellCountX + x].color);
//                     _cells[cellIndex].Elevation = cellData[z * CellCountX + x].elevation;
//                 }
//             }
//
             return false;  // true;
         }
         catch (Exception e)
         {
             GD.Print("Unable to load map: " + e.Message);
//             if (!OS.HasFeature("editor")) GetTree().Quit();
             return false;
         }
     }

     private void FindPath(HexCell sourceCell, HexCell destCell)
     {
         PathFinder finder = new PathFinder();
         finder.Search(sourceCell, destCell);
     }

     private void OnTouch(Vector3 position)
     {
         position = ToLocal(position);
         var coordinates = HexCoordinates.FromPosition(position);
         var index = coordinates.X + coordinates.Z * CellCountX + coordinates.Z / 2;
//         if (!Input.IsKeyPressed(Key.Shift))
//         {
//             _searchFromCell?.DisableHighlight();
//             _searchFromCell = _cells[index];
//             _searchFromCell.EnableHighlight(Colors.Blue);
//         }
//         else
//         {
//             _searchToCell?.DisableHighlight();
//             _searchToCell = _cells[index];
//             _searchToCell.EnableHighlight(Colors.Red);
//         }
//
//         if (_searchFromCell != null && _searchToCell != null)
//         {
//             FindPath(_searchFromCell, _searchToCell);
//         }
     }

     private void OnMoveCellUp(Vector3 position)
     {
         position = ToLocal(position);
         var coordinates = HexCoordinates.FromPosition(position);
         var index = coordinates.X + coordinates.Z * CellCountX + coordinates.Z / 2;
         _cells[index].Elevation += 1;
     }

     private void OnMoveCellDown(Vector3 position)
     {
         position = ToLocal(position);
         var coordinates = HexCoordinates.FromPosition(position);
         var index = coordinates.X + coordinates.Z * CellCountX + coordinates.Z / 2;
         _cells[index].Elevation -= 1;
     }
}