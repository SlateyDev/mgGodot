using Microsoft.Xna.Framework;

namespace Godot;

public class Node3D : Node
{
    public Transform3D Transform3D;
    public bool Visible { get; set; } = true;

    public Vector3 ToLocal(Vector3 worldPosition)
    {
        return Vector3.Zero;
    }

    public Transform3D GlobalTransform => Transform3D;
}