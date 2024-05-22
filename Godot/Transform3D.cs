using Microsoft.Xna.Framework;

namespace Godot;

public struct Transform3D
{
    public static Transform3D Identity = new Transform3D(Vector3.Zero, Quaternion.Identity, Vector3.One);

    public Transform3D()
    {
    }

    public Transform3D(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
        // Quaternion.CreateFromYawPitchRoll()
    }

    public Vector3 Position = Vector3.Zero;
    public Quaternion Rotation = Quaternion.Identity;
    public Vector3 Scale = Vector3.One;
    
    public Matrix Matrix => Matrix.CreateScale(Scale) *
                            Matrix.CreateFromQuaternion(Rotation) *
                            Matrix.CreateTranslation(Position);

    public static Transform3D operator *(Transform3D t1, Transform3D t2)
    {
        var result = t1.Matrix * t2.Matrix;
        result.Decompose(out var scale, out var rotation, out var translation);
        
        return new Transform3D(translation, rotation, scale);
    }
}