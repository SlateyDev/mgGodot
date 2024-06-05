using Microsoft.Xna.Framework;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace Godot;

// public class WorldBoundaryShape2D : Shape2D
// {
//     public float Distance = 0.0f;
//     public Vector2 Normal = new Vector2(0, -1);
//     
//     internal override Body CreateShape(World world, float density, Vector2 position, float rotation, BodyType bodyType)
//     {
//         var body = BodyFactory.CreateEdge(world, Height, Radius, density, position, rotation, bodyType);
//         body.BodyType = bodyType;
//         return body;
//     }
// }