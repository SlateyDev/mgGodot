using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Godot;

public class Engine
{
    public static GraphicsDevice GraphicsDevice { get; set; }
    
    public static Camera3D CurrentCamera { get; set; }
    public static Matrix WorldMatrix { get; set; }
}