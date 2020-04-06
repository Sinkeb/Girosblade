using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girospot : MonoBehaviour
{
    public GameObject p1;
    public GameObject p2;
    GameObject player;

    bool playerConectado;
    bool right;
    Quaternion initialRotation;

    void Start()
    {
        playerConectado = false;
        right = true;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (playerConectado && player != null)
        {
            //rotate
            if (right)
            {
                //transform.Rotate(Vector3.up, -360 * Time.deltaTime);
                player.transform.RotateAround(transform.position, Vector3.up, -360 * Time.deltaTime);
            }
            else
            {
                //transform.Rotate(Vector3.up, 360 * Time.deltaTime);
                player.transform.RotateAround(transform.position, Vector3.up, 360 * Time.deltaTime);
            }

        }
    }

    public Vector3 getP1() {
        return p1.transform.position;
    }
    public Vector3 getP2()
    {
        return p2.transform.position;
    }

    public void PlayerConectado(bool r, GameObject p)
    {
        playerConectado = true;
        right = r;
        player = p;
    }
    public void PlayerSolto()
    {
        transform.localRotation = initialRotation;
        playerConectado = false;
        player = null;
    }
}
