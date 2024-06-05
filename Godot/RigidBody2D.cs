using System.Linq;
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
        //     var body = collisionShape2D.Shape.CreateShape(world, 0, default(Vector2), 0, BodyType.Dynamic);
        }
    }
}