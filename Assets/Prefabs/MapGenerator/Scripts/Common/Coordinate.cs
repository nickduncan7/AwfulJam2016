using UnityEngine;

[System.Serializable]
public struct Coordinate
{
    public int q;

    public int r;

    public int s;

    public Coordinate(int q = -1, int r = -1, int s = -1)
    {
        this.q = q;
        this.r = r;
        this.s = s;
    }

    public override bool Equals(object other)
    {
        if (other is Coordinate)
        {
            return ((Coordinate)other).q == q && ((Coordinate)other).r == r;
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + q.GetHashCode();
        hash = hash * 23 + r.GetHashCode();
        return hash;
    }

    public static Coordinate ToHexCoordinate(Coordinate other)
    {
        return new Coordinate(other.q, other.r, -other.q - other.r);
    }

    public static int Distance(Coordinate a, Coordinate b)
    {
        var hex_a = Coordinate.ToHexCoordinate(a);
        var hex_b = Coordinate.ToHexCoordinate(b);
        return (Mathf.Abs(hex_a.q - hex_b.q) + Mathf.Abs(hex_a.r - hex_b.r) + Mathf.Abs(hex_a.s - hex_b.s))/2;
    }

    public static bool operator ==(Coordinate x, Coordinate y)
    {
        return Equals(x, y);
    }

    public static bool operator !=(Coordinate x, Coordinate y)
    {
        return !(x == y);
    }
}
