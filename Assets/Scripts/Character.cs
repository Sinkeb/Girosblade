using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject personagem;
    public GameObject foice;
    public GameObject foicePivot;
    public bool rotating = false;
    public float rotate = 90;
    public bool right = true;
    public int rotating90 = 0;
    public GameObject girospot;
    public bool colliding = false;
    // Start is called before the first frame update
    void Start()
    {
        rotate = 90;
        colliding = false;
        rotating = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && !rotating)
        {
            if (right)
                StartCoroutine(RotateAround(Vector3.up, 90.0f, 0.5f, foice, personagem));
            else
            {
                StartCoroutine(RotateAround(Vector3.up, -90.0f, 0.5f, foice, personagem));
            }
        }
        if(Input.GetKey(KeyCode.Space) && colliding && girospot != null && !rotating)
        {
            foicePivot.transform.position = girospot.transform.position;
            if (right)
                transform.RotateAround(foicePivot.transform.position, Vector3.up, -360*Time.deltaTime);
            else
                transform.RotateAround(foicePivot.transform.position,Vector3.up, 360 * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            foicePivot.transform.Rotate(0, 0, 180);
            right = !right;
        }
    }



    IEnumerator RotateAround(Vector3 axis, float angle, float duration, GameObject p_me, GameObject p_object)
    {
        float elapsed = 0.0f;
        float rotated = 0.0f;
        while (elapsed < duration)
        {
            float step = angle / duration * Time.deltaTime;
            p_me.transform.RotateAround(p_object.transform.position, axis, step);
            elapsed += Time.deltaTime;
            rotated += step;
            rotating = true;
            yield return null;
        }
        p_me.transform.RotateAround(p_object.transform.position, axis, angle - rotated);
        rotating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Girospot")
        {
            girospot = other.gameObject;
            colliding = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Girospot")
        {
            girospot = null;
            colliding = false;
        }
    }
    //private void OnTriggerEnter(Collision collision)
    //{
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Girospot")
    //    {
    //        girospot = null;
    //        colliding = false;
    //    }
    //}
}

