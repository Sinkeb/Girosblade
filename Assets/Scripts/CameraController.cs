using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 initialP;
    void Start()
    {
        initialP = gameObject.transform.position;
    }
    public void setPosition(Vector3 nP) {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(nP.x, 14, nP.z), 10f * Time.deltaTime);
        Debug.Log("leeeerp");
        //gameObject.transform.position = new Vector3(nP.x, 12, nP.z);
    }
    public void resetPosition()
    {
        gameObject.transform.position = initialP;
    }
}
