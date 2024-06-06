using Microsoft.Xna.Framework;

namespace Godot;

public class Node3D : Node
{
    public Transform3D Transform = Transform3D.Identity;
    public bool Visible { get; set; } = true;

    public Vector3 ToLocal(Vector3 worldPosition)
    {
        return Vector3.Zero;
    }

    public Vector3 ToGlobal(Vector3 localPosition)
    {
        if (Parent is Node3D parentNode3D)
        {
            return parentNode3D.ToGlobal(localPosition) + Transform.Position;
        }

        return Transform.Position + localPosition;
    }

    public Vector3 GlobalPosition => ToGlobal(Vector3.Zero);

    public Transform3D GlobalTransform
    {
        get
        {
            if (Parent is Node3D node)
            {
                return Transform * node.GlobalTransform;
            }
            return Transform;
        }
    }
}