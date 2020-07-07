using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCollider : MonoBehaviour
{
    public Character pers;
    float timer = 0f;

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
    }
    void OnTriggerExit(Collider other)
    {
        if (pers.nPlayer == 1 && pers.GetRedeStat())
        {
            if (other.gameObject.tag == "Parede" || other.gameObject.tag == "Paredex" || other.gameObject.tag == "ParedeT")
            {
                pers.SaiuParede();
                timer = 0f;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        /*timer += Time.deltaTime;
        if (pers.paredeCol && timer > 0.2f)
        {
            if (other.gameObject.tag == "Paredex")
            {
                pers.ColisaoParede(new Vector3(-1, 1, 1));
            }
            if (other.gameObject.tag == "Parede")
            {
                pers.ColisaoParede(new Vector3(1, 1, -1));
            }
            if (other.gameObject.tag == "ParedeT")
            {
                pers.ColisaoParedeT(other.gameObject.transform.right);
            }
            timer = 0f;
        }*/
    }
}
