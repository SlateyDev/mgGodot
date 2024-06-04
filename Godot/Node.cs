using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Godot;

public class Node
{
    private SceneTree _tree;
    public SceneTree Tree
    {
        get { return _tree; }
        internal set {
            _tree = value;
            if (value != null)
            {
                _EnterTree();
            }
            foreach(var node in _children)
            {
                node.Tree = value;
            }
            if (value != null)
            {
                _Ready();
            }
            else
            {
                _ExitTree();
            }
        }
    }
    public string Name { get; set; }
    protected Node Parent = null;
    private TimeSpan? _lastGameTime;
    internal readonly List<Node> _children = new();

    public void AddChild(Node child, bool forceReadableName = false)
    {
        if (child.Parent != null)
        {
            throw new Exception("Child already has a parent");
        }
        child.Parent = this;
        _children.Add(child);
        child.Tree = _tree;
    }

    public virtual void _EnterTree() { }
    public virtual void _ExitTree() { }

    public virtual void _Ready() { }
    public virtual void _Process(double delta) { }
    public virtual void _PhysicsProcess(double delta) { }

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