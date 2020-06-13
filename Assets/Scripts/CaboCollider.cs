using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaboCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public Character pers;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Girospot")
        {
            pers.girospot = other.gameObject;
            if (!other.GetComponent<Girospot>().inativo)
            {
                pers.colliding = true;
                if (pers.nPlayer == 1)
                    other.GetComponent<Girospot>().Outline(true);
            }
            else
            {
                pers.girospot = null;
                pers.colliding = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Girospot")
        {
            pers.girospot = null;
            pers.colliding = false;
            if (pers.nPlayer == 1)
                other.GetComponent<Girospot>().Outline(false);
        }
    }
}
