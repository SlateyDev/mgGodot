using System.Linq;
using Godot;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HexadoniaApp;

public class HexGame : Godot.Game.App
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Model _campfire;
    private Matrix[] _campfireTransforms;
    private Texture2D _campfireTexture;

    private readonly Camera3D _camera3D = new();

    private Mesh _testMesh;
    private MeshInstance3D _testMeshInstance3D;

    private HexGrid.HexGrid _grid = new();

    private SceneTree _sceneTree = new();

    private bool _orbit = false;
    
    private Matrix _worldMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);

    private Keys[] _forwardKeys = new [] { Keys.Up, Keys.W };
    private Keys[] _backwardKeys = new [] { Keys.Down, Keys.S };
    private Keys[] _leftKeys = new [] { Keys.Left, Keys.A };
    private Keys[] _rightKeys = new [] { Keys.Right, Keys.D };
    private Keys[] _jumpKeys = new [] { Keys.Space };
    
    public HexGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        Engine.GraphicsDevice = GraphicsDevice;

        _camera3D.Transform3D.Position = new Vector3(0, 10f, 20f);

        _testMesh = new Mesh
        {
            Vertices = new[]
            {
                new Vector3(0, 1, 0),
                new Vector3(-1, -1, 0),
                new Vector3(1, -1, 0),
            },
            Colors = new[]
            {
                Color.Red,
                Color.Green,
                Color.Blue,
            }
        };

        _testMeshInstance3D = new MeshInstance3D()
        {
            Mesh = _testMesh
        };
        // _testMeshInstance3D.CreateVertexBuffer();
        
        _sceneTree.Root.AddChild(_testMeshInstance3D);
        _sceneTree.Root.AddChild(_grid);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _campfire = Content.Load<Model>("Campfire");
        _campfireTransforms = new Matrix[_campfire.Bones.Count];
        _campfire.CopyAbsoluteBoneTransformsTo(_campfireTransforms);
        _campfireTexture = Content.Load<Texture2D>("Campfire_MAT_BaseColor_01");
    }

    protected override void UnloadContent()
    {
        _grid.Shutdown();

        Content.Unload();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        // _camera.Transform.Position += Vector3.Forward * 5 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        if (_leftKeys.Any(key => Keyboard.GetState().IsKeyDown(key)))
        {
            _camera3D.Transform3D.Position -= Vector3.UnitX;
            _camera3D.Target -= Vector3.UnitX;
        }

        if (_rightKeys.Any(key => Keyboard.GetState().IsKeyDown(key)))
        {
            _camera3D.Transform3D.Position += Vector3.UnitX;
            _camera3D.Target += Vector3.UnitX;
        }

        if (_forwardKeys.Any(key => Keyboard.GetState().IsKeyDown(key)))
        {
            _camera3D.Transform3D.Position -= Vector3.UnitZ;
            _camera3D.Target -= Vector3.UnitZ;
        }

        if (_backwardKeys.Any(key => Keyboard.GetState().IsKeyDown(key)))
        {
            _camera3D.Transform3D.Position += Vector3.UnitZ;
            _camera3D.Target += Vector3.UnitZ;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            _orbit = !_orbit;
        }

        if (_orbit)
        {
            var rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
            _camera3D.Transform3D.Position = Vector3.Transform(_camera3D.Transform3D.Position, rotationMatrix);
        }

        _camera3D.UpdateViewMatrix();

        _sceneTree.RunProcess(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        Engine.CurrentCamera = _camera3D;
        Engine.WorldMatrix = _worldMatrix;
        
        GraphicsDevice.Clear(Color.Bisque);
        
        var rasterizerState = new RasterizerState();
        rasterizerState.CullMode = CullMode.None;
        GraphicsDevice.RasterizerState = rasterizerState;

        _sceneTree.RunRender();
        
        foreach (var mesh in _campfire.Meshes)
        {
            var localWorld = _campfireTransforms[mesh.ParentBone.Index] * Matrix.CreateScale(0.1f, 0.1f, 0.1f);

            foreach (var part in mesh.MeshParts)
            {
                var e = (BasicEffect)part.Effect;
                e.Texture = _campfireTexture;
                e.TextureEnabled = true;

                e.World = localWorld;
                e.View = Engine.CurrentCamera.ViewMatrix;
                e.Projection = Engine.CurrentCamera.ProjectionMatrix;

                e.EnableDefaultLighting();
            }
            
            mesh.Draw();
        }

        _spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(-10, -10, 0));
        _spriteBatch.Draw(_campfireTexture, new Rectangle(10,10,266,266), new Rectangle(0, 0, 256, 256), Color.White);
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }

    private void DrawTerrain(Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix)
    {
        
    }
}