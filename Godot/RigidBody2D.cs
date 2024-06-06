using System.Collections.Generic;
using System.Linq;
using Godot.Game;
using Microsoft.Xna.Framework;
using VelcroPhysics.Dynamics;

namespace Godot;

public class RigidBody2D : PhysicsBody2D
{
    internal override void ReadyInternal()
    {
        base.ReadyInternal();
        
        foreach (var collisionShape2D in _children.OfType<CollisionShape2D>())
        {
             var body = collisionShape2D.Shape.CreateShape(App.World, 0, collisionShape2D.GlobalTransform.Position, collisionShape2D.GlobalTransform.Rotation, BodyType.Dynamic);
             Bodies.Add(body);
        }
    }
}