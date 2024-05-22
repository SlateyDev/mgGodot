using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Godot;

public class Node
{
    public string Name { get; set; }
    private Node _parent = null;
    private TimeSpan? _lastGameTime;
    internal readonly List<Node> _children = new();

    public void AddChild(Node child)
    {
        if (child._parent != null)
        {
            throw new Exception("Child already has a parent");
        }
        child._parent = this;
        _children.Add(child);
    }

    public virtual void _Process(double delta) {}

    internal void Update(GameTime gameTime)
    {
        double delta = 0f;
        if (_lastGameTime != null)
        {
            delta = (gameTime.TotalGameTime - (TimeSpan)_lastGameTime).TotalSeconds;
        }
        _lastGameTime = gameTime.TotalGameTime;
        _Process(delta);
        foreach (var node in _children)
        {
            node.Update(gameTime);
        }
    }
}