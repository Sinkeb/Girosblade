using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Girospot : MonoBehaviour
{
    public GameObject p1;
    public GameObject p2;
    GameObject player;
    public GameObject malha;
    bool playerConectado;
    bool right;
    Quaternion initialRotation;

    public bool inativo = false;
    float inativoTimer = 0f;

    public GameObject shield;
    public GameObject sShield;

    public Image loading;
    float maxTime = 5f;

    public Outline myOutline;

    void Start()
    {
        playerConectado = false;
        right = true;
        initialRotation = transform.localRotation;
        myOutline.enabled = false;
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
                Outline(false);
            }
            else
            {
                //transform.Rotate(Vector3.up, 360 * Time.deltaTime);
                player.transform.RotateAround(transform.position, Vector3.up, 360 * Time.deltaTime);
                Outline(false);
            }

        }
        if (inativo)
        {
            if(malha!=null)
            {
                Debug.Log("maaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaalha ");
                malha.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
            GetComponent<CapsuleCollider>().enabled = false;
            inativoTimer += Time.deltaTime;
            loading.GetComponent<Image>().fillAmount = inativoTimer / maxTime;
            if (inativoTimer >= maxTime)
            {
                if (malha != null)
                {

                    malha.GetComponent<MeshRenderer>().enabled = true;
                }
                else
                {
                    GetComponent<MeshRenderer>().enabled = true;
                }
                GetComponent<CapsuleCollider>().enabled = true;
                inativo = false;
                inativoTimer = 0f;
                loading.GetComponent<Image>().enabled = false;
            }
        }
    }

    public Vector3 getP1()
    {
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
        Outline(false);
        shield.GetComponent<CapsuleCollider>().enabled = true;
        sShield.GetComponent<SpriteRenderer>().enabled = true;
    }
    public void PlayerSolto()
    {
        transform.localRotation = initialRotation;
        playerConectado = false;
        player = null;
        inativo = true;
        Outline(false);
        loading.GetComponent<Image>().enabled = true;
        shield.GetComponent<CapsuleCollider>().enabled = false;
        sShield.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Outline(bool t)
    {
        myOutline.enabled = t;
    }
}
