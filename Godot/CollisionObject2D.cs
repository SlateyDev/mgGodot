using System.Collections.Generic;
using VelcroPhysics.Dynamics;

namespace Godot;

public abstract class CollisionObject2D : Node2D
{
    internal readonly List<Body> Bodies = [];
}