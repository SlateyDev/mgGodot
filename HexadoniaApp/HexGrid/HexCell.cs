using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexadoniaApp.HexGrid;

public class HexCell
{
    public bool Enabled { get; set; }
    public Vector3 Position { get; set; }
    public HexCoordinates Coordinates { get; set; }
    public Color Color { get; set; }
    public HexGridChunk Chunk { get; set; }
    public Texture2D CellOutlineTexture { get; set; }
    // public Sprite3D _highlightSprite;

    private readonly HexCell[] _neighbors = new HexCell[6];
    private int _elevation;

    private int _distance = Int32.MaxValue;
    
    public int SearchHeuristic { get; set; }
    public int SearchPriority => _distance + SearchHeuristic;
    public HexCell NextWithSamePriority { get; set; }
    public HexCell PathFrom { get; set; }

    public Color? HighlightColor { get; set; }

    // public IEntity OccupyingEntity = null;
    
    public HexCell GetNeighbor(HexDirection direction)
    {
        return _neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        _neighbors[(int)direction] = cell;
        cell._neighbors[(int)direction.Opposite()] = this;
    }

    public void SetColor(Color value)
    {
        if (Color == value)
        {
            return;
        }

        Color = value;
        Refresh();
    }

    public void EnableHighlight(Color spriteColor)
    {
        HighlightColor = spriteColor;
        if (Chunk == null) return;

        // if (_highlightSprite == null)
        // {
        //     _highlightSprite = new Sprite3D();
        //     _highlightSprite.Name = "HighlightSprite";
        //     _highlightSprite.Rotation = new Vector3(Mathf.DegToRad(90), 0, 0);
        //     _highlightSprite.Scale = Vector3.One * 0.45f;
        //     _highlightSprite.Texture = CellOutlineTexture;
        //     Chunk.AddChild(_highlightSprite);
        // }
        //
        // _highlightSprite.Position = Position + new Vector3(0, 0.001f, 0);
        // _highlightSprite.Modulate = spriteColor;
    }

    public void DisableHighlight()
    {
        HighlightColor = null;
        if (Chunk == null) return;

        // if (_highlightSprite != null)
        // {
        //     _highlightSprite.Free();
        //     _highlightSprite = null;
        // }
    }
    
    public int Distance
    {
        get => _distance;
        set
        {
            _distance = value;
            // UpdateDistanceLabel();
        }
    }
    public int Elevation
    {
        get => _elevation;
        set
        {
            if (_elevation == value)
            {
                return;
            }

            _elevation = value;

            var newPosition = Position;
            newPosition.Y = _elevation * HexMetrics.ElevationStep;

            Position = newPosition;

            // if (_highlightSprite != null)
            // {
            //     _highlightSprite.Position = Position + new Vector3(0, 0.001f, 0);
            // }

            Refresh();
        }
    }

    private void Refresh()
    {
        if (Chunk == null) return;

        Chunk.Refresh();
        foreach (var neighbor in _neighbors)
        {
            if (neighbor != null && neighbor.Chunk != Chunk)
            {
                neighbor.Chunk?.Refresh();
            }
        }
    }
}