using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileLayer
{
    // Start is called before the first frame update
    string layerName;

    Tile[][] layer;

    int width, height;

    int girospotCount = 0;

    int[][] girospotM;


    public TileLayer(int x, int y, string name, int type)
    {
        layerName = name;
        width = x;
        height = y;
        if (name == "floor")
            CreateMap(x, y, type);
        else if (name == "girospot")
            CreateGirospotMap(x, y, type);
    }

    public void CreateMap(int x, int y, int type)
    {
        layer = new Tile[x][];

        for (int i = 0; i < x; i++)
        {
            layer[i] = new Tile[y];
            for (int j = 0; j < y; j++)
            {
                layer[i][j] = new Tile();
                layer[i][j].SetTile(layerName, type);
            }
        }
    }

    public void CreateGirospotMap(int x, int y, int type)
    {
        setArena();

        layer = new Tile[x][];

        for (int i = 0; i < x; i++)
        {
            layer[i] = new Tile[y];
            for (int j = 0; j < y; j++)
            {
                layer[i][j] = new Tile();
                if (girospotM[i][j] == 1)
                {
                    layer[i][j].SetTile(layerName, type, true);
                    girospotCount++;
                }
            }
        }
    }
    public Vector3 GetDistance(int i, int j, int x, int y, float width, float height)
    {
        return new Vector3(width * i - (x + width / 2), -1, height * j - (y + height / 2));
    }

    public Tile[][] getLayer()
    {
        return layer;
    }

    public int LayerWidth()
    {
        return width;
    }

    public int LayerHeight()
    {
        return height;
    }

    public string getLayerName()
    {
        return layerName;
    }

    public int getGirospotCount()
    {
        return girospotCount;
    }

    public void setArena()
    {
        switch (GlobalClass.HostArena)
        {
            case 0:
                girospotM = new int[][]
                {
            new int[] { 0,0,0,0,0,0,0,0 },
            new int[] { 0,0,1,0,0,0,1,0 },
            new int[] { 0,0,0,0,1,0,0,0 },
            new int[] { 0,0,1,0,0,0,1,0 },
            new int[] { 0,1,0,0,0,0,0,0 },
            new int[] { 0,0,0,0,1,0,0,0 },
            new int[] { 0,1,0,0,0,0,0,0 },
            new int[] { 0,0,0,0,0,0,0,0 },
            new int[] { 0,0,0,0,0,0,0,0 },
                };
                break;
            case 1:
                girospotM = new int[][]
                {
            new int[] { 0,0,0,0,0,0,0,0 },
            new int[] { 0,0,0,0,0,0,0,0 },
            new int[] { 0,0,1,0,0,1,0,0 },
            new int[] { 0,0,0,0,0,0,0,0 },
            new int[] { 0,0,0,0,0,0,0,0 },
            new int[] { 0,0,0,0,0,0,0,0 },
            new int[] { 0,0,1,0,0,1,0,0 },
            new int[] { 0,0,0,0,0,0,0,0 },
            new int[] { 0,0,0,0,0,0,0,0 },
                };
                break;
            case 2:
                girospotM = new int[][]
                {
            new int[] { 0,0,0,0,0,0,0,0 },
            new int[] { 0,0,0,1,0,0,1,0 },
            new int[] { 0,0,0,0,0,0,0,0 },
            new int[] { 0,0,0,1,0,0,0,0 },
            new int[] { 0,1,0,0,0,0,0,0 },
            new int[] { 0,0,0,0,1,0,0,0 },
            new int[] { 0,0,0,0,0,0,0,0 },
            new int[] { 0,0,0,1,0,0,1,0 },
            new int[] { 0,0,0,0,0,0,0,0 },
                };
                break;
        }
    }
}
