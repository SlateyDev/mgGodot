// #define USING_HEX_NEIGHBOUR_RAMPING

using Godot;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HexadoniaApp.HexGrid;

public class HexMesh : Node3D
{
	private readonly MeshInstance3D _hexMesh = new();
// 	private readonly StaticBody3D _staticBody = new();
// 	private readonly CollisionShape3D _collisionShape = new();
// 	private readonly ConcavePolygonShape3D _concaveShape = new();
	private List<Vector3> _vertices;
	private List<Vector3> _shapeVerts;
	private readonly List<Vector3> _terrainTypes;
	private List<Color> _colors;
	private List<short> _indexes;
	private List<Vector3> _vertexNormals;

// 	private ArrayMesh _arrayMesh = new();
	
	public HexMesh(Material material)
	{
		Name = "HexMesh";
// 		_collisionShape.Name = "CollisionShape3D";
// 		_collisionShape.Shape = _concaveShape;
// 		_staticBody.AddChild(_collisionShape);
	
		_hexMesh.Name = "HexMesh";
// 		_hexMesh.Mesh = _arrayMesh;
// 		_hexMesh.MaterialOverride = material;
// 		_staticBody.Name = "StaticBody3D";
 	
 		AddChild(_hexMesh);
// 		AddChild(_staticBody);
	}

	public void Triangulate(IEnumerable<HexCell> cells)
	{
		_vertices = ListPool<Vector3>.Get();
		_shapeVerts = ListPool<Vector3>.Get();
		_indexes = ListPool<short>.Get();
		_colors = ListPool<Color>.Get();
		_vertexNormals = ListPool<Vector3>.Get();

		lock (_shapeVerts)
		{
			foreach (var cell in cells)
			{
				if (cell.Enabled)
				{
					TriangulateCell(cell);
				}
			}
		}

// 		var arrays = new Array();
// 		arrays.Resize((int)Mesh.ArrayType.Max);
//
// 		arrays[(int)Mesh.ArrayType.Vertex] = _vertices.ToArray();
// 		arrays[(int)Mesh.ArrayType.Index] = _indexes.ToArray();
// 		arrays[(int)Mesh.ArrayType.Color] = _colors.ToArray();
// 		arrays[(int)Mesh.ArrayType.Normal] = _vertexNormals.ToArray();
// 		
// 		_arrayMesh.ClearSurfaces();
// 		_arrayMesh.CallDeferred("add_surface_from_arrays", (int)Mesh.PrimitiveType.Triangles, arrays);
		_hexMesh.Mesh = new Mesh
		{
			Vertices = _vertices.ToArray(),
			Colors = _colors.ToArray(),
			Normals = _vertexNormals.ToArray(),
			Indices = _indexes.ToArray(),
		};
		
		ListPool<Vector3>.Add(_vertices);
		ListPool<Vector3>.Add(_shapeVerts);
		ListPool<short>.Add(_indexes);
		ListPool<Color>.Add(_colors);
		ListPool<Vector3>.Add(_vertexNormals);
	}

	public void CreateCollider()
	{
		lock (_shapeVerts)
		{
// 			_concaveShape.Data = _shapeVerts.ToArray();
		}
	}

	private void TriangulateCell(HexCell cell)
	{
		var center = cell.Position;
		AddHexTop(center, cell.Color);
		foreach (var direction in (int[])Enum.GetValues(typeof(HexDirection)))
		{
			var v1 = center + HexMetrics.GetFirstSolidCorner((HexDirection)direction);
			var v2 = center + HexMetrics.GetSecondSolidCorner((HexDirection)direction);
			var v3 = center + HexMetrics.GetFirstCorner((HexDirection)direction) + new Vector3(0,-HexMetrics.bevelAmount,0);
			var v4 = center + HexMetrics.GetSecondCorner((HexDirection)direction) + new Vector3(0,-HexMetrics.bevelAmount,0);

			if (HexMetrics.bevelAmount != 0f)
			{
				AddQuad(v1, v2, v3, v4, cell.Color, cell.Color, cell.Color, cell.Color);
			}

			var neighbor = cell.GetNeighbor((HexDirection)direction);
			if (neighbor != null)
			{
				if (cell.Elevation > neighbor.Elevation)
				{
					var diff = (cell.Elevation - neighbor.Elevation) * HexMetrics.ElevationStep;
					var v5 = center + HexMetrics.GetFirstCorner((HexDirection)direction) + new Vector3(0,-HexMetrics.bevelAmount - diff,0);
					var v6 = center + HexMetrics.GetSecondCorner((HexDirection)direction) + new Vector3(0,-HexMetrics.bevelAmount - diff,0);

					AddQuad(v3, v4, v5, v6, cell.Color, cell.Color, cell.Color, cell.Color);
				}
			}
			else
			{
				var diff = (cell.Elevation - -40) * HexMetrics.ElevationStep;
				var v5 = center + HexMetrics.GetFirstCorner((HexDirection)direction) + new Vector3(0,-HexMetrics.bevelAmount - diff,0);
				var v6 = center + HexMetrics.GetSecondCorner((HexDirection)direction) + new Vector3(0,-HexMetrics.bevelAmount - diff,0);

				AddQuad(v3, v4, v5, v6, cell.Color, cell.Color, cell.Color, cell.Color);
			}
		}
	}

	private void AddHexTop(Vector3 center, Color cellColor)
	{
		var v1 = center + HexMetrics.Corners[(int)HexDirection.NE] * HexMetrics.SolidFactor;
		var v2 = center + HexMetrics.Corners[(int)HexDirection.E] * HexMetrics.SolidFactor;
		var v3 = center + HexMetrics.Corners[(int)HexDirection.SE] * HexMetrics.SolidFactor;
		var v4 = center + HexMetrics.Corners[(int)HexDirection.SW] * HexMetrics.SolidFactor;
		var v5 = center + HexMetrics.Corners[(int)HexDirection.W] * HexMetrics.SolidFactor;
		var v6 = center + HexMetrics.Corners[(int)HexDirection.NW] * HexMetrics.SolidFactor;
		
		var vertexIndex = (short)_vertices.Count;
		_colors.Add(cellColor);
		_vertices.Add(center);
		_colors.Add(cellColor);
		_vertices.Add(v1);
		_colors.Add(cellColor);
		_vertices.Add(v2);
		_colors.Add(cellColor);
		_vertices.Add(v3);
		_colors.Add(cellColor);
		_vertices.Add(v4);
		_colors.Add(cellColor);
		_vertices.Add(v5);
		_colors.Add(cellColor);
		_vertices.Add(v6);

		_shapeVerts.Add(center);
		_shapeVerts.Add(v2);
		_shapeVerts.Add(v1);

		_shapeVerts.Add(center);
		_shapeVerts.Add(v3);
		_shapeVerts.Add(v2);

		_shapeVerts.Add(center);
		_shapeVerts.Add(v4);
		_shapeVerts.Add(v3);

		_shapeVerts.Add(center);
		_shapeVerts.Add(v5);
		_shapeVerts.Add(v4);

		_shapeVerts.Add(center);
		_shapeVerts.Add(v6);
		_shapeVerts.Add(v5);

		_shapeVerts.Add(center);
		_shapeVerts.Add(v1);
		_shapeVerts.Add(v6);

		_indexes.Add(vertexIndex);
		_indexes.Add((short)(vertexIndex + 2));
		_indexes.Add((short)(vertexIndex + 1));

		_indexes.Add(vertexIndex);
		_indexes.Add((short)(vertexIndex + 3));
		_indexes.Add((short)(vertexIndex + 2));

		_indexes.Add(vertexIndex);
		_indexes.Add((short)(vertexIndex + 4));
		_indexes.Add((short)(vertexIndex + 3));

		_indexes.Add(vertexIndex);
		_indexes.Add((short)(vertexIndex + 5));
		_indexes.Add((short)(vertexIndex + 4));

		_indexes.Add(vertexIndex);
		_indexes.Add((short)(vertexIndex + 6));
		_indexes.Add((short)(vertexIndex + 5));

		_indexes.Add(vertexIndex);
		_indexes.Add((short)(vertexIndex + 1));
		_indexes.Add((short)(vertexIndex + 6));

		var normal = new Plane(center, v2, v1).Normal;
		_vertexNormals.Add(normal);
		_vertexNormals.Add(normal);
		_vertexNormals.Add(normal);
		_vertexNormals.Add(normal);
		_vertexNormals.Add(normal);
		_vertexNormals.Add(normal);
		_vertexNormals.Add(normal);
	}

	private void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Color c1, Color c2, Color c3, Color c4)
	{
		var vertexIndex = (short)_vertices.Count;
		_colors.Add(c1);
		_vertices.Add(v1);
		_colors.Add(c2);
		_vertices.Add(v2);
		_colors.Add(c3);
		_vertices.Add(v3);
		_colors.Add(c4);
		_vertices.Add(v4);

		var normal = new Plane(v1, v2, v3).Normal;
		_vertexNormals.Add(normal);
		_vertexNormals.Add(normal);
		_vertexNormals.Add(normal);
		_vertexNormals.Add(normal);

		_shapeVerts.Add(v1);
		_shapeVerts.Add(v2);
		_shapeVerts.Add(v3);

		_indexes.Add(vertexIndex);
		_indexes.Add((short)(vertexIndex + 1));
		_indexes.Add((short)(vertexIndex + 2));

		_shapeVerts.Add(v2);
		_shapeVerts.Add(v4);
		_shapeVerts.Add(v3);

		_indexes.Add((short)(vertexIndex + 1));
		_indexes.Add((short)(vertexIndex + 3));
		_indexes.Add((short)(vertexIndex + 2));
	}
}
