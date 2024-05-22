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
        RunSubRender(Root, new Transform3D());
    }

    private void RunSubRender(Node renderNode, Transform3D currentTransform)
    {
        if (renderNode is IRenderable renderable)
        {
            renderable.Render(currentTransform);
        }

        foreach (var node in renderNode._children)
        {
            if (node is Node3D node3D)
            {
                currentTransform *= node3D.Transform3D;
            }
            RunSubRender(node, currentTransform);
        }
    }
}