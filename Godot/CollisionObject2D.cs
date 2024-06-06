using System.Collections.Generic;
using VelcroPhysics.Dynamics;

namespace Godot;

public abstract class CollisionObject2D : Node2D
{
    public readonly List<Body> Bodies = [];
}