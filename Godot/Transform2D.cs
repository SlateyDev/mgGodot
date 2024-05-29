using Microsoft.Xna.Framework;

namespace Godot;

public class Transform2D
{
    public static Transform2D Identity = new Transform2D(Vector2.Zero, 0.0f, Vector2.One);

    public Transform2D()
    {
    }

    public Transform2D(Vector2 position, float rotation, Vector2 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
        // Quaternion.CreateFromYawPitchRoll()
    }

    public Vector2 Position = Vector2.Zero;
    public float Rotation = 0;
    public Vector2 Scale = Vector2.One;
    
    public static Transform2D operator *(Transform2D t1, Transform2D t2)
    {
        return new Transform2D(t1.Position + t2.Position, t1.Rotation + t2.Rotation, t1.Scale * t2.Scale);
    }
}