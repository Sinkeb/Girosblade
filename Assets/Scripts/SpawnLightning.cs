using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnLightning : MonoBehaviour
{
    public Image[] raios;
    public float interval;
    public float LightTime = 0.3f;
    bool spawned = false;
    float cTime = 0f;
    int atual;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cTime >= interval)
        {
            Spawn();
        }
        else
        {
            cTime += Time.deltaTime;
        }
        if (spawned)
        {
            if(cTime >= LightTime)
            {
                raios[atual].enabled = false;
                spawned = false;
            }
        }
    }

    public void Spawn()
    {
        atual = Random.Range(0, 4);
        raios[atual].enabled = true;
        spawned = true;
        cTime = 0f;
    }
}
