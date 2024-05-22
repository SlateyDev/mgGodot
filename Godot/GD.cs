using System;
using System.Text;

namespace Godot;

public class GD
{
    public static void Print(params object[] values)
    {
        var sb = new StringBuilder();
        foreach (var value in values)
        {
            sb.Append(value);
        }
        Console.WriteLine(sb);
    }
}