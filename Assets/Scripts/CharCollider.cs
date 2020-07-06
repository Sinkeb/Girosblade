using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCollider : MonoBehaviour
{
    public Character pers;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(pers.nPlayer == 1 && pers.GetRedeStat())
        {
            if (other.gameObject.tag == "Paredex")
            {
                pers.ColisaoParede(new Vector3(-1, 1, 1));
            }
            if (other.gameObject.tag == "Parede")
            {
                pers.ColisaoParede(new Vector3(1, 1, -1));
            }
            if(other.gameObject.tag == "ParedeT")
            {
                pers.ColisaoParedeT(other.gameObject.transform.right);
            }
        }
        else if(!pers.GetRedeStat())
        {
            if (other.gameObject.tag == "Paredex")
            {
                pers.ColisaoParede(new Vector3(-1, 1, 1));
            }
            if (other.gameObject.tag == "Parede")
            {
                pers.ColisaoParede(new Vector3(1, 1, -1));
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (pers.nPlayer == 1 && pers.GetRedeStat())
        {
            if (other.gameObject.tag == "Parede" || other.gameObject.tag == "Paredex" || other.gameObject.tag == "ParedeT")
            {
                pers.SaiuParede();
            }
        }else if (!pers.GetRedeStat())
        {
            if (other.gameObject.tag == "Parede" || other.gameObject.tag == "Paredex")
            {
                pers.SaiuParede();
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        /*if (pers.paredeCol)
        {
            if (other.gameObject.tag == "Paredex")
            {
                pers.ColisaoParede(new Vector3(-1, 1, 1));
            }
            if (other.gameObject.tag == "Parede")
            {
                pers.ColisaoParede(new Vector3(1, 1, -1));
            }
        }*/
    }
}
