using Microsoft.Xna.Framework;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace Godot;

public class CircleShape2D : Shape2D
{
    public float Radius = 10.0f;
    
    internal override Body CreateShape(World world, float density, Vector2 position, float rotation, BodyType bodyType)
    {
        var body = BodyFactory.CreateCircle(world, Radius, density, position, bodyType);
        body.Rotation = rotation;
        return body;
    }
}