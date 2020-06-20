using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoiceCollider : MonoBehaviour
{
    public Character pers;

    public void DesativarCollider()
    {
        GetComponent<MeshCollider>().enabled = false;
    }
    public void AtivarCollider()
    {
        GetComponent<MeshCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Girospot")
        {
            pers.girospot = other.gameObject;
            if (!pers.girospot.GetComponent<Girospot>().inativo)
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
        if (other.gameObject.tag == "Foice" && !pers.repelimento)
        {
            Character ot = other.gameObject.GetComponent<FoiceCollider>().pers;
            if (!ot.ghost && !pers.ghost)
            {
                Debug.Log("foice na foice");
                pers.repelimento = true;
                pers.InverterDirecao();
            }
        }
        /*if (other.gameObject.tag == "Player" && !pers.repelimento)
        {
            Character ot = other.gameObject.GetComponentInParent<Character>();
            if (!ot.ghost && !pers.ghost)
            {
                Debug.Log("foice no Player");
                pers.repelimento = true;
                pers.InverterDirecao();
            }
        }*/
        if (other.gameObject.tag == "Shield" && !pers.repelimento)
        {
            pers.repelimento = true;
            pers.InverterDirecao();
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
