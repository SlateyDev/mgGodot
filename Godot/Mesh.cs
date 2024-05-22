using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Godot;

public class Mesh
{
    private int _version = 0;
    private Vector3[] _vertices;
    private Vector2[] _uvs1;
    private Color[] _colors;
    private Vector3[] _normals;

    private short[] _indices;
    
    // public VertexPositionColor[] Vertices { get; set; }

    public int Version => _version;

    public Vector3[] Vertices
    {
        get => _vertices;
        set
        {
            _vertices = value;
            _version++;
        }
    }

    public Color[] Colors
    {
        get => _colors;
        set
        {
            _colors = value;
            _version++;
        }
    }

    public Vector3[] Normals
    {
        get => _normals;
        set
        {
            _normals = value;
            _version++;
        }
    }

    public Vector2[] UV1s
    {
        get => _uvs1;
        set
        {
            _uvs1 = value;
            _version++;
        }
    }

    public short[] Indices
    {
        get => _indices;
        set
        {
            _indices = value;
            _version++;
        }
    }
}