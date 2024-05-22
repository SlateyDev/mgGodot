using Microsoft.Xna.Framework;

namespace Godot;

public class SceneTree
{
    public Node Root { get; } = new();
    
    public void RunProcess(GameTime gameTime)
    {
        Root.Update(gameTime);
    }

    public void RunRender()
    {
        RunSubRender(Root);
    }

    private void RunSubRender(Node renderNode)
    {
        if (renderNode is IRenderable renderable)
        {
            renderable.Render();
        }

        foreach (var node in renderNode._children)
        {
            RunSubRender(node);
        }
    }
}