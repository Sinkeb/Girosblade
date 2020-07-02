using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowCollision : MonoBehaviour
{
    public Character Paizao;
    void Start()
    {    }
    void Update()
    {    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "SlowC" && !Paizao.onGirospot)
        {
            Paizao.GiroSlow();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SlowC" && !Paizao.onGirospot)
        {
            Paizao.GiroFast();
        }
    }
}
