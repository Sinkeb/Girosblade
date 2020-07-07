using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoiceCollider : MonoBehaviour
{
    public Character pers;
    public GameObject faisca;
    public GameManager manager;
    public AudioClip foiceF, foiceG;
    AudioSource audioS;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }
    public void DesativarCollider()
    {
        GetComponent<MeshCollider>().enabled = false;
    }
    public void AtivarCollider()
    {
        GetComponent<MeshCollider>().enabled = true;
    }
    public void playFoiceF() {
        audioS.clip = foiceF;
        audioS.Play();
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
        if (other.gameObject.tag == "Foice" && manager.meuID == 2 && pers.nPlayer == 1)
        {
            Character ot = other.gameObject.GetComponent<FoiceCollider>().pers;
            if (!ot.ghost && !pers.ghost)
            {
                Debug.Log("foice na foice");
                pers.repelimento = true;
                pers.InverterDirecao();
                //ot.InverterDirecao();
                audioS.clip = foiceF;
                audioS.Play();
                manager.EnviarFoiceCol(other.ClosestPoint(gameObject.transform.position));
                Instantiate(faisca, other.ClosestPoint(gameObject.transform.position), Quaternion.identity);
                //Instantiate(faisca, gameObject.transform.position, Quaternion.identity);
                //mandar para o outro inverted dir e instanciar faisca na posicao vec3

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
            
            audioS.clip = foiceG;
            audioS.Play();
            
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
