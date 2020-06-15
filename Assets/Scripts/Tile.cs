using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    // Start is called before the first frame update
    GameObject myTile;

    float cellSizeX, cellSizeY;

    bool isGirospot = false;

    public void SetTile(string type, int num)
    {
        myTile = Resources.Load<GameObject>(type + "/" + num);
        Vector3 temp = myTile.GetComponentInChildren<MeshRenderer>().bounds.size;
        cellSizeX = temp.x;
        cellSizeY = temp.z;
    }
    public void SetTile(string type, int num, bool isGiro)
    {
        isGirospot = isGiro;
        if (isGirospot)
            myTile = Resources.Load<GameObject>(type + "/" + num);
    }

    public GameObject getMyTile()
    {
        return myTile;
    }

    public bool IsGirospot()
    {
        return isGirospot;
    }

    public float height()
    {
        return cellSizeX;
    }
    public float width()
    {
        return cellSizeY;
    }
}
