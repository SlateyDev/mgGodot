using System.Collections.Generic;

namespace HexadoniaApp.HexGrid;

public class ListPool<T>
{
    private static readonly Stack<List<T>> Stack = new();

    public static List<T> Get()
    {
        return Stack.Count > 0 ? Stack.Pop() : new List<T>();
    }

    public static void Add(List<T> list)
    {
        list.Clear();
        Stack.Push(list);
    }
}