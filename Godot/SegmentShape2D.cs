using Microsoft.Xna.Framework;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace Godot;

public class SegmentShape2D : Shape2D
{
    public Vector2 A = new Vector2(0, 0);
    public Vector2 B = new Vector2(0, 10);

    internal override Body CreateShape(World world, float density, Vector2 position, float rotation, BodyType bodyType)
    {
        var body = BodyFactory.CreateEdge(world, A, B);
        body.BodyType = bodyType;
        return body;
    }
}