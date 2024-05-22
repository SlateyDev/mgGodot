namespace Godot;

public class Random
{
    private static System.Random r = new System.Random();

    public static int Randi()
    {
        return r.Next();
    }
}