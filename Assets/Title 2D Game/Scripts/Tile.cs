public enum Sides
{
    Top,
    Left,
    Right,
    Bottom,
}

public class Tile
{
    public int id;
    public Tile[] adjacents = new Tile[4];

    public int autoTileId;

    public void UpdateAutoTileId()
    {
        autoTileId = 0;
        for (int i = 0; i < adjacents.Length; ++i)         // adjacents(4)번 돌리기 0001/0010/0100/1000
        {
            if (adjacents[i] != null)
            {
                autoTileId |= 1 << i;                      // 16번 돌리기 0001/0010/0011 등
            }
        }
    }

    public void RemoveAdjacents(Tile tile)
    {
        for(int i = 0; i < adjacents.Length; ++i)
        {
            if(adjacents[i] == null)
            {
                continue;
            }
            if(adjacents[i].id == tile.id)
            {
                adjacents[i] = null;
                break;
            }
        }

        UpdateAutoTileId();
    }

    public void ClearAdjacents()
    {
        for(int i = 0; i < adjacents.Length; ++i)
        {
            if(adjacents[i] == null)
            {
                continue;
            }
            adjacents[i].RemoveAdjacents(this);
            adjacents[i] = null;  // 나의 이웃 끊기
        } 

        UpdateAutoTileId();
    }
}
