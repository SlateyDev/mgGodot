using Microsoft.Xna.Framework;

namespace Godot;

public class WorldBoundaryShape2D : Shape2D
{
    public float Distance = 0.0f;
    public Vector2 Normal = new Vector2(0, -1);
}