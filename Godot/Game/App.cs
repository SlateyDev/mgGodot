using Microsoft.Xna.Framework;
using System;
using VelcroPhysics.Dynamics;

namespace Godot.Game;

public class App : Microsoft.Xna.Framework.Game
{
    public static readonly World World;

    static App()
    {
        World = new World(new Vector2(0, 9.82f));
    }
    
    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {

    }

    protected override void BeginRun()
    {
        base.BeginRun();
    }

    protected override void Update(GameTime gameTime)
    {
        World.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
        base.Update(gameTime);
    }

    protected override bool BeginDraw()
    {
        return base.BeginDraw();
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }

    protected override void EndDraw()
    {
        base.EndDraw();
    }

    protected override void EndRun()
    {
        base.EndRun();
    }

    protected override void UnloadContent()
    {

    }
}
