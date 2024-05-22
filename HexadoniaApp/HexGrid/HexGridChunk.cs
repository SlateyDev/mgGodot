using Godot;
using System;
using System.Collections.Generic;

namespace HexadoniaApp.HexGrid;

public class HexGridChunk : Node3D
{
    private HexMesh _hexMesh;
    private readonly Material _material; 
    private readonly HexCell[] _cells;
    private bool _refreshEnabled;

    private ChunkBuilder _chunkBuilder;

    public HexGridChunk(Material hexMaterial, int xChunk, int zChunk, IReadOnlyList<HexCell> cells, ChunkBuilder chunkBuilder)
    { 
        _chunkBuilder = chunkBuilder;
        Name = "HexGridChunk";
        Visible = false;   //TODO: Testing by setting to invisible at the start
    
        _material = hexMaterial;
        _cells = new HexCell[HexMetrics.ChunkSizeX * HexMetrics.ChunkSizeZ];
        
        for (var z = 0; z < HexMetrics.ChunkSizeZ; z++)
        {
            for (var x = 0; x < HexMetrics.ChunkSizeX; x++)
            {
                var xCell = xChunk * HexMetrics.ChunkSizeX + x;
                var zCell = zChunk * HexMetrics.ChunkSizeZ + z;
                var globalCell = cells[zCell * HexGrid.ChunkCountX * HexMetrics.ChunkSizeX + xCell];
                globalCell.Chunk = this;
                _cells[z * HexMetrics.ChunkSizeX + x] = globalCell;
        
                // if (globalCell.HighlightColor != null)
                // {
                //     globalCell._highlightSprite = new Sprite3D();
                //     globalCell._highlightSprite.Name = "HighlightSprite";
                //     globalCell._highlightSprite.Rotation = new Vector3(Mathf.DegToRad(90), 0, 0);
                //     globalCell._highlightSprite.Scale = Vector3.One * 0.45f;
                //     globalCell._highlightSprite.Texture = globalCell.CellOutlineTexture;
                //     AddChild(globalCell._highlightSprite);
                //
                //     globalCell._highlightSprite.Position = globalCell.Position + new Vector3(0, 0.001f, 0);
                //     globalCell._highlightSprite.Modulate = (Color)globalCell.HighlightColor;
                // }
            }
        }
    }

    public HexGridChunk()
    {
        _refreshEnabled = true;
    }

    public override void _Process(double delta)
    {
        // if (!_refreshEnabled || !Visible) return;
        
        if (_hexMesh == null)
        {
            _hexMesh = new HexMesh(_material);
            AddChild(_hexMesh);
        }
        
        _refreshEnabled = false;
        _chunkBuilder.QueueChunk(this);
        Console.WriteLine("Queued chunk");
    }
    
    public void Refresh()
    {
        _refreshEnabled = true;
    }

    public void Triangulate()
    {
        _hexMesh.Triangulate(_cells);
    }

    public void CreateCollider()
    {
        _hexMesh.CreateCollider();
    }

    public void Delete()
    {
        foreach (var hexCell in _cells)
        {
            hexCell.Chunk = null;
        }
        
//TODO: How to remove from scene?
        // QueueFree();
    }
}