using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    TileLayer floorLayer;
    TileLayer girospotLayer;
    GameObject[][] tiles;
    GameObject[] girospotA;
    public GameManager gameManager; 
    void Start()
    {
        //NewLayer();
    }
    public void NewLayer()
    {
        floorLayer = new TileLayer(9, 8, "floor", GlobalClass.floorSkin);
        girospotLayer = new TileLayer(9, 8, "girospot", GlobalClass.girospotSkin);
        InstantiateLayer(floorLayer);
        InstantiateGirospot(girospotLayer);
    }
    public void NewLayer(TileLayer fLayer, TileLayer gLayer)
    {
        floorLayer = fLayer;
        girospotLayer = gLayer;
        InstantiateLayer(floorLayer);
        InstantiateGirospot(girospotLayer);
    }

    public void InstantiateLayer(TileLayer layer)
    {
        Tile[][] __tiles = layer.getLayer();
        int x = layer.LayerWidth();
        int y = layer.LayerHeight();
        tiles = new GameObject[x][];
        for (int i = 0; i < x; i++)
        {
            tiles[i] = new GameObject[y];
            for (int j = 0; j < y; j++)
            {
                Tile __tile = __tiles[i][j];
                GameObject __tileGO = __tile.getMyTile();
                tiles[i][j] = Instantiate(__tileGO, layer.GetDistance(i, j, x, y, __tile.width(), __tile.height()), Quaternion.identity, gameObject.transform);
            }
        }
    }

    public void InstantiateGirospot(TileLayer layer)
    {
        Tile[][] __tiles = layer.getLayer();
        gameManager.girospots = new GameObject[layer.getGirospotCount()];
        Debug.Log("Layer giros Count" + gameManager.girospots.Length);
        int x = layer.LayerWidth();
        int y = layer.LayerHeight();
        int count = 0;
        int countX = 0;
        int countGirospot = 0;
        for (int i = x-1; i >= 0; i--)
        {
            for (int j = y-1; j >= 0; j--)
            {
                Tile __tile = __tiles[countX][count];
                GameObject __tileGO = __tile.getMyTile();
                if (__tile.IsGirospot())
                {
                    gameManager.girospots[countGirospot] = Instantiate(__tileGO, GetMiddle(tiles[i][j].transform.position.x, tiles[i][j].transform.position.z), Quaternion.identity, tiles[i][j].transform);
                    countGirospot++;
                }
                count++;
            }
            countX++;
            count = 0;
        }

    }
    public Vector3 GetMiddle(float x, float y)
    {
        return new Vector3(x - 1.5f, 0, y - 1.5f);
    }

    public GameObject[] getGirospot()
    {
        Debug.Log("Girospot A:" + girospotA.Length);
        return girospotA;
    }

}
