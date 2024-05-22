using System;
using Microsoft.Xna.Framework;

namespace HexadoniaApp.HexGrid;

public class HexCoordinates
{
    private int x, z;

    public int X => x;
    public int Z => z;
    public int Y => -X - Z;

    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x - z / 2, z);
    }

    public static HexCoordinates FromPosition(Vector3 position)
    {
        var x = position.X / (HexMetrics.InnerRadius * 2f);
        var y = -x;

        var offset = position.Z / (HexMetrics.OuterRadius * 3f);
        x -= offset;
        y -= offset;

        var iX = (int)x;
        var iY = (int)y;
        var iZ = (int)(-x - y);

        if (iX + iY + iZ == 0) return new HexCoordinates(iX, iZ);

        var dX = Math.Abs(x - iX);
        var dY = Math.Abs(y - iY);
        var dZ = Math.Abs(-x - y - iZ);

        if (dX > dY && dX > dZ)
        {
            iX = -iY - iZ;
        }
        else if (dZ > dY)
        {
            iZ = -iX - iY;
        }

        return new HexCoordinates(iX, iZ);
    }

    public int DistanceTo(HexCoordinates other)
    {
        return ((x < other.x ? other.x - x : x - other.x) +
                (Y < other.Y ? other.Y - Y : Y - other.Y) +
                (z < other.z ? other.z - z : z - other.z)) / 2;
    }

    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }

    public string ToStringOnSeparateLines()
    {
        return $"{X}\n{Y}\n{Z}";
    }
}