using Microsoft.Xna.Framework;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace Godot;

public class CapsuleShape2D : Shape2D
{
    public float Height = 30.0f;
    public float Radius = 10.0f;
    
    internal override Body CreateShape(World world, float density, Vector2 position, float rotation, BodyType bodyType)
    {
        var body = BodyFactory.CreateCapsule(world, Height, Radius, density, position, rotation, bodyType);
        return body;
    }
}