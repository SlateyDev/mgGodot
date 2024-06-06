using Microsoft.Xna.Framework;

namespace Godot;

public class Camera3D : Node3D
{
    private Vector3 _target = Vector3.Zero;
    private float _fieldOfView = 45.0f;
    private float _nearPlane = 0.1f;
    private float _farPlane = 1000.0f;
    private float _aspectRatio = 800.0f / 480.0f;

    public Vector3 Target
    {
        get => _target;
        set
        {
            _target = value;
            UpdateViewMatrix();
        }
    }
    public float FieldOfView
    {
        get => _fieldOfView;
        set
        {
            _fieldOfView = value;
            UpdateProjectionMatrix();
        }
    }
    public float NearPlane
    {
        get => _nearPlane;
        set
        {
            _nearPlane = value;
            UpdateProjectionMatrix();
        }
    }
    public float FarPlane
    {
        get => _farPlane;
        set
        {
            _farPlane = value;
            UpdateProjectionMatrix();
        }
    }
    public float AspectRatio
    {
        get => _aspectRatio;
        set
        {
            _aspectRatio = value;
            UpdateProjectionMatrix();
        }
    }

    private Matrix _viewMatrix;
    private Matrix _projectionMatrix;
    
    public Matrix ViewMatrix => _viewMatrix;
    public Matrix ProjectionMatrix => _projectionMatrix;

    private void UpdateProjectionMatrix()
    {
        _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(_fieldOfView), _aspectRatio, _nearPlane, _farPlane);
    }

    public void UpdateViewMatrix()
    {
        _viewMatrix = Matrix.CreateLookAt(Transform.Position, _target, Vector3.Up);
    }

    public Camera3D()
    {
        UpdateProjectionMatrix();
        UpdateViewMatrix();
    }
}