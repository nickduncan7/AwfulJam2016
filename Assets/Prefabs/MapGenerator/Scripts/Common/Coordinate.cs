[System.Serializable]
public struct Coordinate
{
    public int q;

    public int r;

    public Coordinate(int q = -1, int r = -1)
    {
        this.q = q;
        this.r = r;
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

    public static bool operator ==(Coordinate x, Coordinate y)
    {
        return Equals(x, y);
    }

    public static bool operator !=(Coordinate x, Coordinate y)
    {
        return !(x == y);
    }
}
