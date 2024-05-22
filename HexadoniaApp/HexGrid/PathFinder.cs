using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HexadoniaApp.HexGrid;

public class PathFinder
{
    public int maxWalkElevationDifference = 1;
    public int hillCost = 1;
    public int waterDepthCost = 2;
    public int waterDepthBlocked = -2;
    private HexCellPriorityQueue _searchFrontier;
    
    public Stack<HexCell> Search(HexCell sourceCell, HexCell destCell)
    {
        // var hexGrid = ((SceneTree)Engine.GetMainLoop()).Root.GetNode<HexGrid>("World3D/HexGrid");
        //
        // foreach (var cell in hexGrid.Cells)
        // {
        //     cell.Distance = int.MaxValue;
        //     cell.DisableHighlight();
        // }
        // sourceCell.EnableHighlight(Colors.Blue);
        // destCell.EnableHighlight(Colors.Red);
        //
        // if (_searchFrontier == null)
        // {
        //     _searchFrontier = new HexCellPriorityQueue();
        // }
        // else
        // {
        //     _searchFrontier.Clear();
        // }
        //
        // sourceCell.Distance = 0;
        // _searchFrontier.Enqueue(sourceCell);
        // while (_searchFrontier.Count > 0)
        // {
        //     var currentCell = _searchFrontier.Dequeue();
        //
        //     if (currentCell == destCell)
        //     {
        //         var Path = new Stack<HexCell>();
        //         
        //         Path.Push(currentCell);
        //         currentCell = currentCell.PathFrom;
        //         while (currentCell != sourceCell)
        //         {
        //             Path.Push(currentCell);
        //             currentCell.EnableHighlight(Colors.Green);
        //             currentCell = currentCell.PathFrom;
        //         }
        //
        //         return Path;
        //     }
        //
        //     for (var d = HexDirection.NE; d <= HexDirection.NW; d++)
        //     {
        //         var neighbor = currentCell.GetNeighbor(d);
        //         if (neighbor == null)
        //         {
        //             continue;
        //         }
        //
        //         if (neighbor.Elevation < waterDepthBlocked)
        //         {
        //             continue;
        //         }
        //
        //         if (Math.Abs(neighbor.Elevation - currentCell.Elevation) > maxWalkElevationDifference)
        //         {
        //             continue;
        //         }
        //
        //         var distance = currentCell.Distance + 1;
        //         distance += Math.Abs(neighbor.Elevation - currentCell.Elevation) * hillCost;
        //         if (neighbor.Elevation < -1)
        //         {
        //             distance += -neighbor.Elevation * waterDepthCost;
        //         }
        //
        //         if (distance >= neighbor.Distance)
        //         {
        //             continue;
        //         }
        //
        //         if (neighbor.Distance == int.MaxValue)
        //         {
        //             neighbor.Distance = distance;
        //             neighbor.PathFrom = currentCell;
        //             neighbor.SearchHeuristic = neighbor.Coordinates.DistanceTo(destCell.Coordinates);
        //             _searchFrontier.Enqueue(neighbor);
        //             if (neighbor != destCell)
        //             {
        //                 neighbor.EnableHighlight(Colors.DimGray);
        //             }
        //         }
        //         else
        //         {
        //             var oldPriority = neighbor.SearchPriority;
        //             neighbor.Distance = distance;
        //             neighbor.PathFrom = currentCell;
        //             _searchFrontier.Change(neighbor, oldPriority);
        //         }
        //     }
        // }

        return null;
    }
}
