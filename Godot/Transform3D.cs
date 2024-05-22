using Microsoft.Xna.Framework;

namespace Godot;

public struct Transform3D
{
    public Transform3D()
    {
        Position = Vector3.Zero;
        Rotation = Quaternion.Identity;
        Scale = Vector3.One;
        Origin = Vector3.Zero;
    }

    public Transform3D(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
        Origin = Vector3.Zero;
    }

    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;

    public Vector3 Origin { get; set; }
}