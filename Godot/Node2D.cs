namespace Godot;

public class Node2D : Node
{
    public Transform2D Transform = Transform2D.Identity;

    public bool Visible { get; set; } = true;
    
    public Transform2D GlobalTransform
    {
        get
        {
            if (Parent is Node2D node)
            {
                return Transform * node.GlobalTransform;
            }
            return Transform;
        }
    }}