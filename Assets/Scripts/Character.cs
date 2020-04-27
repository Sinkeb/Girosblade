using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public GameObject personagem;
    public GameObject foice;
    public GameObject foicePivot, foicePivot2;
    public bool rotating = false;
    public float rotate = 90;
    public bool right = true;
    public int rotating90 = 0;
    public GameObject girospot;
    public bool colliding = false;
    public bool onGirospot = false;
    private int speed = 10;
    bool rotatingGirospot = false;
    public Material mat, matGhost;
    Material matt;
    public Material m2;
    public bool dummy = false;
    public bool ghost = false;
    float ghostTimer = 0;

    public bool repelimento = false;
    float repelimentoTimer = 0;

    int vidas = 3;

    Vector3 initialPosition;
    Quaternion initialRotation;

    Vector3 corpoInitialPosition;
    Quaternion corpoInitialRotation;

    public Vector3 direction = new Vector3(0,0,0);
    Vector3 esquerda = new Vector3(-1, 0, 0);
    Vector3 direita = new Vector3(1, 0, 0);
    Vector3 cima = new Vector3(0, 0, 1);
    Vector3 baixo = new Vector3(0, 0, -1);

    bool paredeCol = false;

    Material[] materiais;

    public Image[] spriteVidas;

    public int nPlayer = 1;
    // Start is called before the first frame update
    void Start()
    {
        rotate = 90;
        colliding = false;
        rotating = false;
        rotatingGirospot = false;
        initialPosition = foice.transform.localPosition;
        initialRotation = foice.transform.localRotation;

        corpoInitialPosition = transform.localPosition;
        corpoInitialRotation = transform.localRotation;

        matt = Instantiate(GetComponent<MeshRenderer>().material);
        mat = GetComponent<MeshRenderer>().material;

        if(nPlayer == 2)
        {
            GetComponent<MeshRenderer>().material = m2;
        }

    }

    void Update()
    {
        if (ghost)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            //GetComponent<BoxCollider>
            ghostTimer += Time.deltaTime;
            if(ghostTimer >= 3.5f)
            {
                Debug.Log("sair do ghost");
                ghostTimer = 0f;
                GiroGhostOff();
            }
        }
        if (repelimento)
        {
            repelimentoTimer += Time.deltaTime;
            if(repelimentoTimer >= 0.8f)
            {
                Debug.Log("Acabou Repelimento");
                repelimentoTimer = 0f;
                repelimento = false;
            }
        }
        //gameObject.GetComponent<CapsuleCollider>().tr
        if (!dummy && nPlayer == 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = esquerda;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = direita;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = cima;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = baixo;
            }
        }
        if (!dummy && nPlayer == 2)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                direction = esquerda;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                direction = direita;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                direction = cima;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                direction = baixo;
            }
        }
        if (Input.GetKeyDown(KeyCode.F)){
            GiroGhostOn();
        }

        if (!rotating && !rotatingGirospot && !dummy)
        {
            if (right) {
                //StartCoroutine(RotateAround(Vector3.up, 360, 1f, foice, personagem));
                foice.transform.RotateAround(personagem.transform.position,Vector3.up, 360 * Time.deltaTime);
            }
            else
            {
                //StartCoroutine(RotateAround(Vector3.up, -360, 1f, foice, personagem));
                //RotateAround(Vector3.up, -360, 1f, foice, personagem);
                foice.transform.RotateAround(personagem.transform.position, Vector3.up, -360 * Time.deltaTime);
            }
        }
        if(nPlayer == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space) && colliding && !dummy)
            {
                ActionKeyGirospot();
            }
        }
        else if(nPlayer == 2)
        {
            if (Input.GetKeyDown(KeyCode.B) && colliding && !dummy)
            {
                ActionKeyGirospot();
            }
        }
       
        if (Input.GetKeyDown(KeyCode.R) && !dummy)
        {
            foice.transform.Rotate(0, 0, 180);
            right = !right;
        }
        if(!rotatingGirospot)
        Move();
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
    public void Move()
    {

        //Vector3 Movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        transform.position += direction * speed * Time.deltaTime;


    }
    public void GiroGhostOn() {
        matt.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.25f);
        GetComponent<MeshRenderer>().material = matt;
        ghost = true;
        vidas--;
        spriteVidas[vidas].GetComponent<Image>().enabled = false;
        if (vidas <= 0)
        {
            morreu();
        }
    }
    public void GiroGhostOff()
    {
        //mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1);
        GetComponent<MeshRenderer>().material = mat;
        ghost = false;
        GetComponent<CapsuleCollider>().enabled = true;
    }
    void morreu()
    {
        Destroy(gameObject);
    }
    public void InverterDirecao()
    {
        direction = -direction;
        foice.transform.Rotate(0, 0, 180);
        right = !right;
    }
    public void ColisaoParede(Vector3 dir)
    {
        direction = new Vector3(direction.x * dir.x, 0, direction.z * dir.z);
        foice.transform.Rotate(0, 0, 180);
        right = !right;
        paredeCol = true;
    }
    public void SaiuParede()
    {
        paredeCol = false;
    }

    void ActionKeyGirospot()
    {
        if (!girospot.GetComponent<Girospot>().inativo)
        {
            rotatingGirospot = !rotatingGirospot;
            if (rotatingGirospot)
            {
                transform.localPosition = corpoInitialPosition;
                transform.localRotation = corpoInitialRotation;
                foice.transform.localRotation = initialRotation;
                foice.transform.localPosition = initialPosition;

                //transform.SetParent(girospot.transform);
                if (right)
                {
                    transform.position = girospot.GetComponent<Girospot>().getP1();
                }
                else
                {
                    foice.transform.Rotate(0, 0, 180);
                    transform.position = girospot.GetComponent<Girospot>().getP2();
                }
                onGirospot = true;
                GetComponent<CapsuleCollider>().enabled = false;
                girospot.GetComponent<Girospot>().PlayerConectado(right, gameObject);
            }
            else
            {
                //transform.SetParent(null);
                GetComponent<CapsuleCollider>().enabled = true;
                girospot.GetComponent<Girospot>().PlayerSolto();
                direction = gameObject.transform.localRotation * -Vector3.forward;
                onGirospot = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        
        if (other.gameObject.tag == "Girospot")
        {
            girospot = other.gameObject;
            if (!girospot.GetComponent<Girospot>().inativo)
            {
                colliding = true;
            }
            else
            {
                girospot = null;
                colliding = false;
            }
        }
        if (other.gameObject.tag == "Player" && !dummy && !ghost && !repelimento)
        {
            Debug.Log("Player no player");
            repelimento = true;
            //impulsionar ao contrario
            InverterDirecao();
        }
        if (other.gameObject.tag == "Foice" && !ghost && !repelimento)
        {
            Debug.Log("Player colidiu na foice");
            //impulsionar ao contrario
            InverterDirecao();
            GiroGhostOn();
            
        }

        if (other.gameObject.tag == "Shield")
        {
            InverterDirecao();

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

