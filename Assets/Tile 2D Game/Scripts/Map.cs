using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileTypes
{
    Empty = -1,

    // 0, 14
    Grass = 15,
    Trees,
    Hills,
    Mountains,
    Towns,
    Castle,
    Monster,
    Dungeon,
}

public class Map
{
    public int rows = 0;
    public int cols = 0;

    public Tile[] tiles;

    public Tile[] CoastTiles =>
        tiles.Where(t => t.autoTileId >= 0 && t.autoTileId < (int)TileTypes.Grass).ToArray();

    public Tile[] LandTiles => tiles.Where(t => t.autoTileId >= (int)TileTypes.Grass).ToArray();

    public void Init(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;

        tiles = new Tile[rows * cols];
        for (int i = 0; i < tiles.Length; ++i)
        {
            tiles[i] = new Tile
            {
                id = i,
                row = i / cols,
                col = i % cols
            };
        }

        for (int r = 0; r < rows; ++r)
        {
            for (int c = 0; c < cols; ++c)
            {
                int index = r * cols + c;
                var adjacents = tiles[index].adjacents;
                if ((r - 1) >= 0)
                {
                    adjacents[(int)Sides.Top] = tiles[index - cols];
                }
                if ((c + 1) < cols)
                {
                    adjacents[(int)Sides.Right] = tiles[index + 1];
                }
                if ((c - 1) >= 0)
                {
                    adjacents[(int)Sides.Left] = tiles[index - 1];
                }
                if ((r + 1) < rows)
                {
                    adjacents[(int)Sides.Bottom] = tiles[index + cols];
                }
            }
        }

        for (int i = 0; i < tiles.Length; ++i)
        {
            tiles[i].UpdateAutoTileId();
        }
    }

    public bool CreateIsland(float erodePercent, float erodeIterations, float lakePercent)
    {
        DecorateTiles(LandTiles, lakePercent, TileTypes.Empty);

        for (int i = 0; i < erodeIterations; ++i)
        {
            DecorateTiles(CoastTiles, erodePercent, TileTypes.Empty);
        }

        DecorateTiles(LandTiles, 0.15f, TileTypes.Trees);
        DecorateTiles(LandTiles, 0.10f, TileTypes.Hills);
        DecorateTiles(LandTiles, 0.15f, TileTypes.Mountains);
        DecorateTiles(LandTiles, 0.05f, TileTypes.Dungeon);
        DecorateTiles(LandTiles, 0.05f, TileTypes.Towns);

        var availableLand = LandTiles.Where(t => t.autoTileId == (int)TileTypes.Grass).ToArray();
        var towns = tiles.Where(t => t.autoTileId == (int)TileTypes.Towns).ToArray();

        if (availableLand.Length == 0 || towns.Length == 0)
        {
            return true;
        }

        ShuffleTiles(availableLand);
        Tile castleTile = availableLand[0];
        castleTile.autoTileId = (int)TileTypes.Castle;

        ShuffleTiles(towns);
        Tile startTown = towns[0];

        return HasPathAStar(startTown, castleTile);
    }

    public bool HasPathAStar(Tile startTile, Tile targetTile)
    {
        foreach (var tile in tiles)
        {
            tile.ResetPathfinding();
        }

        List<Tile> openList = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();

        startTile.g = 0;
        startTile.h = GetManhattanDistance(startTile, targetTile);
        openList.Add(startTile);

        while (openList.Count > 0)
        {
            Tile current = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].f < current.f || (openList[i].f == current.f && openList[i].h < current.h))
                {
                    current = openList[i];
                }
            }

            if (current == targetTile)
            {
                return true;
            }

            openList.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in current.adjacents)
            {
                // 이웃이 없거나, 이미 탐색했거나, 이동 불가능한 타일(바다, 산 등)이면 스킵
                if (neighbor == null || closedSet.Contains(neighbor) || !neighbor.IsWalkable)
                {
                    continue;
                }

                // 지형에 따른 이동 비용 가중치 설정 (예: 언덕은 2, 일반 평지는 1)
                int moveCost = (neighbor.autoTileId == (int)TileTypes.Hills) ? 2 : 1;
                int tentativeG = current.g + moveCost;

                if (tentativeG < neighbor.g)
                {
                    neighbor.parent = current;
                    neighbor.g = tentativeG;
                    neighbor.h = GetManhattanDistance(neighbor, targetTile);

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }
        return false;
    }

    private int GetManhattanDistance(Tile a, Tile b)
    {
        return Mathf.Abs(a.row - b.row) + Mathf.Abs(a.col - b.col);
    }

    public void DecorateTiles(Tile[] tiles, float percent, TileTypes tileType)
    {
        int total = Mathf.FloorToInt(tiles.Length * percent);

        ShuffleTiles(tiles);

        for (int i = 0; i < total; ++i)
        {
            if (tileType == TileTypes.Empty)
            {
                tiles[i].ClearAdjacents();
            }
            tiles[i].autoTileId = (int)tileType;
        }
    }

    public void ShuffleTiles(Tile[] tiles)
    {
        // 피셔 예이츠 셔플 알고리즘
        for (int i = tiles.Length - 1; i > 0; --i)
        {
            int index = Random.Range(0, i + 1);
            Tile temp = tiles[i];
            tiles[i] = tiles[index];
            tiles[index] = temp;
        }
    }
}
