using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Godot;

public class MeshInstance3D : Node3D, IRenderable
{
    private VertexBuffer _vertexBuffer;
    private IndexBuffer _indexBuffer;

    private int _lastMeshVersion = 0;
    private bool _hasIndexBuffer;
    public Mesh Mesh { get; set; }
    public Effect Effect { get; set; }

    public MeshInstance3D()
    {
        Effect = new BasicEffect(Engine.GraphicsDevice);
        ((BasicEffect)Effect).Alpha = 1.0f;
        ((BasicEffect)Effect).VertexColorEnabled = true;
        ((BasicEffect)Effect).LightingEnabled = true;
        ((BasicEffect)Effect).DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
        ((BasicEffect)Effect).DirectionalLight0.Direction = new Vector3(0f,1f,0f);
        ((BasicEffect)Effect).DirectionalLight0.SpecularColor = new Vector3(0, 1, 0);
    }

    public void CreateVertexBuffer()
    {
        _vertexBuffer?.Dispose();
        _indexBuffer?.Dispose();

        var hasColors = Mesh.Colors != null && Mesh.Colors.Length == Mesh.Vertices.Length;
        
        if (Mesh.Normals != null && Mesh.Normals.Length == Mesh.Vertices.Length)
        {
            if (Mesh.UV1s != null && Mesh.UV1s.Length == Mesh.Vertices.Length)
            {
                var vertData = Mesh.Vertices.Select<Vector3, VertexPositionColorNormalTexture>((o, i) => new VertexPositionColorNormalTexture(o, hasColors ? Mesh.Colors[i] : Color.White, Mesh.Normals[i], Mesh.UV1s[i])).ToArray();
    
                _vertexBuffer = new VertexBuffer(Engine.GraphicsDevice, typeof(VertexPositionColorNormalTexture), Mesh.Vertices.Length, BufferUsage.WriteOnly);
                _vertexBuffer.SetData(vertData);
            }
            else
            {
                var vertData = Mesh.Vertices.Select<Vector3, VertexPositionColorNormal>((o, i) => new VertexPositionColorNormal(o, hasColors ? Mesh.Colors[i] : Color.White, Mesh.Normals[i])).ToArray();
    
                _vertexBuffer = new VertexBuffer(Engine.GraphicsDevice, typeof(VertexPositionColorNormal), Mesh.Vertices.Length, BufferUsage.WriteOnly);
                _vertexBuffer.SetData(vertData);
            }
        }
        else
        {
            if (Mesh.UV1s != null && Mesh.UV1s.Length == Mesh.Vertices.Length)
            {
                var vertData = Mesh.Vertices.Select<Vector3, VertexPositionColorTexture>((o, i) => new VertexPositionColorTexture(o, hasColors ? Mesh.Colors[i] : Color.White, Mesh.UV1s[i])).ToArray();
    
                _vertexBuffer = new VertexBuffer(Engine.GraphicsDevice, typeof(VertexPositionColorTexture), Mesh.Vertices.Length, BufferUsage.WriteOnly);
                _vertexBuffer.SetData(vertData);
            }
            else
            {
                var vertData = Mesh.Vertices.Select<Vector3, VertexPositionColor>((o, i) => new VertexPositionColor(o, hasColors ? Mesh.Colors[i] : Color.White)).ToArray();
    
                _vertexBuffer = new VertexBuffer(Engine.GraphicsDevice, typeof(VertexPositionColor), Mesh.Vertices.Length, BufferUsage.WriteOnly);
                _vertexBuffer.SetData(vertData);
            }
        }

        _hasIndexBuffer = Mesh.Indices != null && Mesh.Indices.Length > 2;
        if (_hasIndexBuffer)
        {
            _indexBuffer = new IndexBuffer(Engine.GraphicsDevice, IndexElementSize.SixteenBits, Mesh.Indices.Length, BufferUsage.WriteOnly);
            _indexBuffer.SetData(Mesh.Indices);
        }
    }

    public void Render(Transform3D worldPosition)
    {
        ((BasicEffect)Effect).Projection = Engine.CurrentCamera.ProjectionMatrix;
        ((BasicEffect)Effect).View = Engine.CurrentCamera.ViewMatrix;
        // ((BasicEffect)Effect).World = Engine.WorldMatrix;
        ((BasicEffect)Effect).World = worldPosition.Matrix;

        if (Mesh == null) return;

        if (_lastMeshVersion != Mesh.Version)
        {
            CreateVertexBuffer();
            _lastMeshVersion = Mesh.Version;
        }
        Engine.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
        if (_hasIndexBuffer)
        {
            Engine.GraphicsDevice.Indices = _indexBuffer;
        }

        foreach (var pass in Effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            if (_hasIndexBuffer)
            {
                Engine.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _indexBuffer.IndexCount / 3);
            }
            else
            {
                Engine.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertexBuffer.VertexCount / 3);
            }
        }
    }
}