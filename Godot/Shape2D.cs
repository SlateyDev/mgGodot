using Microsoft.Xna.Framework;
using VelcroPhysics.Dynamics;

namespace Godot;

public abstract class Shape2D : Resource
{
    internal abstract Body CreateShape(World world, float density, Vector2 position, float rotation,
        BodyType bodyType);
}