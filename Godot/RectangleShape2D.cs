using Microsoft.Xna.Framework;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace Godot;

public class RectangleShape2D : Shape2D
{
    public Vector2 Size = new Vector2(20, 20);

    internal override Body CreateShape(World world, float density, Vector2 position, float rotation, BodyType bodyType)
    {
        return BodyFactory.CreateRectangle(world, Size.X, Size.Y, density, position, rotation, bodyType);
    }
}