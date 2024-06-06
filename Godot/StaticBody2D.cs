using System;
using System.Linq;
using Godot.Game;
using Microsoft.Xna.Framework;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Utilities;

namespace Godot;

public class StaticBody2D : CollisionObject2D
{
    internal override void ReadyInternal()
    {
        base.ReadyInternal();
        
        foreach (var collisionShape2D in _children.OfType<CollisionShape2D>())
        {
            Console.WriteLine($"Creating StaticBody2D @ {collisionShape2D.GlobalTransform.Position}");
            var body = collisionShape2D.Shape.CreateShape(App.World, 0, ConvertUnits.ToSimUnits(collisionShape2D.GlobalTransform.Position), collisionShape2D.GlobalTransform.Rotation, BodyType.Static);
            Bodies.Add(body);
        }
    }
}