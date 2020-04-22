using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoiceCollider : MonoBehaviour
{
    public Character pers;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Foice")
        {
            Character ot = other.gameObject.GetComponent<FoiceCollider>().pers;
            if (!ot.ghost && !pers.ghost)
            {
                Debug.Log("foice na foice");
                pers.InverterDirecao();
            }
        }
        if (other.gameObject.tag == "Player")
        {
            Character ot = other.gameObject.GetComponent<Character>();
            if (!ot.ghost && !pers.ghost)
            {
                Debug.Log("foice no Player");
                pers.InverterDirecao();
            }
        }
    }
}
