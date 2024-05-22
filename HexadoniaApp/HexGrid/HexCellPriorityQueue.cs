using System;
using System.Collections.Generic;

namespace HexadoniaApp.HexGrid;

public class HexCellPriorityQueue
{
    private readonly List<HexCell> _list = new();
    private int _minimum = int.MaxValue;

    public int Count { get; private set; }

    public void Enqueue(HexCell cell)
    {
        Count += 1;
        var priority = cell.SearchPriority;
        if (priority < _minimum)
        {
            _minimum = priority;
        }
        while (priority >= _list.Count)
        {
            _list.Add(null);
        }
        cell.NextWithSamePriority = _list[priority];
        _list[priority] = cell;
    }
    
    public HexCell Dequeue()
    {
        Count -= 1;
        for (; _minimum < _list.Count; _minimum++)
        {
            var cell = _list[_minimum];
            if (cell == null) continue;
            _list[_minimum] = cell.NextWithSamePriority;
            return cell;
        }
        return null;
    }
    
    public void Change(HexCell cell, int oldPriority)
    {
        if (cell.NextWithSamePriority != null)
        {
            if (cell.NextWithSamePriority.SearchPriority != oldPriority)
            {
                throw new Exception("WTF");
            }
        }
        var current = _list[oldPriority];
        var next = current.NextWithSamePriority;
        if (current == cell)
        {
            _list[oldPriority] = next;
        }
        else
        {
            while (next != cell)
            {
                current = next;
                next = current.NextWithSamePriority;
            }
            current.NextWithSamePriority = cell.NextWithSamePriority;
        }
        Enqueue(cell);
        Count -= 1;
    }
    
    public void Clear()
    {
        _list.Clear();
        Count = 0;
        _minimum = int.MaxValue;
    }
}