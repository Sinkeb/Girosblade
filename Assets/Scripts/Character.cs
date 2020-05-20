﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

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
    float giroTime = 0f;
    float giroTimeMax = 6f;

    private int speed = 10;
    bool rotatingGirospot = false;
    public Material mat, matGhost;
    Material matt;
    public Material m2;
    public GameObject cabo, foiceee;
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
    GameManager manager;



    //REDE
    /*int hostId;
    private int reliableChannel;
    private int unreliableChannel;
    private int connectionId;*/

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rotate = 90;
        colliding = false;
        rotating = false;
        rotatingGirospot = false;
        initialPosition = foice.transform.localPosition;
        initialRotation = foice.transform.localRotation;

        corpoInitialPosition = transform.localPosition;
        corpoInitialRotation = transform.localRotation;

        //matt = Instantiate(GetComponent<MeshRenderer>().material);
        
        if (nPlayer == 2)
        {
            GetComponent<MeshRenderer>().material = m2;
            foiceee.GetComponent<MeshRenderer>().material = m2;
            cabo.GetComponent<MeshRenderer>().material = m2;
            //GetComponentInChildren<MeshRenderer>().material = mat;
            mat = m2;
        }
        else
        {
            GetComponent<MeshRenderer>().material = mat;
            foiceee.GetComponent<MeshRenderer>().material = mat;
            cabo.GetComponent<MeshRenderer>().material = mat;
        }

    }

    void Update()
    {
        if (!manager.comecou && manager.preparados)
        {
            if (Input.GetKeyDown(KeyCode.Space) && nPlayer ==1)
            {
                manager.PlayerReady(nPlayer, gameObject);
            }
            /*else if (Input.GetKeyDown(KeyCode.B) && nPlayer == 2)
            {
                manager.PlayerReady(nPlayer, gameObject);
            }*/
        }
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

        if (onGirospot && nPlayer == 1)
        {
            giroTime += Time.deltaTime;
            if(giroTime >= giroTimeMax)
            {
                giroTime = 0f;
                SairGirospot();
            }
        }
        else if(onGirospot && manager.rede && nPlayer == 1)
        {
            giroTime += Time.deltaTime;
            if (giroTime >= giroTimeMax)
            {
                giroTime = 0f;
                SairGirospot();
            }
        }
       
        //gameObject.GetComponent<CapsuleCollider>().tr
        if (!dummy && nPlayer == 1 && manager.jogando)
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
            if (Input.GetKeyDown(KeyCode.R) && !dummy)
            {
                foice.transform.Rotate(0, 0, 180);
                right = !right;
            }
            if (Input.GetKeyDown(KeyCode.Space) && colliding && !dummy)
            {
                ActionKeyGirospot();
            }
        }
        /*if (!dummy && nPlayer == 2 && manager.jogando)
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
            if (Input.GetKeyDown(KeyCode.Q) && !dummy)
            {
                foice.transform.Rotate(0, 0, 180);
                right = !right;
            }
            if (Input.GetKeyDown(KeyCode.B) && colliding && !dummy)
            {
                ActionKeyGirospot();
            }
        }*/
        /*if (Input.GetKeyDown(KeyCode.F)){
            GiroGhostOn();
        }*/

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
        
       
        
        if(!rotatingGirospot && manager.jogando && nPlayer == 1)
        {
            Move();
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
    public void Move()
    {

        //Vector3 Movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        transform.position += direction * speed * Time.deltaTime;
        manager.EnviarPosicao(transform.position.x, transform.position.y, transform.position.z);


    }
    public bool GetRedeStat()
    {
        return manager.rede;
    }
    public void GiroGhostOn() {
        //matt.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.25f);
        GetComponent<MeshRenderer>().material = matGhost;
        foiceee.GetComponent<MeshRenderer>().material = matGhost;
        cabo.GetComponent<MeshRenderer>().material = matGhost;
        //GetComponentInChildren<MeshRenderer>().material = matGhost;

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
        foiceee.GetComponent<MeshRenderer>().material = mat;
        cabo.GetComponent<MeshRenderer>().material = mat;
        //GetComponentInChildren<MeshRenderer>().material = mat;

        ghost = false;
        GetComponent<CapsuleCollider>().enabled = true;
    }
    void morreu()
    {
        manager.Perdi(nPlayer);
        Destroy(gameObject);
    }
    public void InverterDirecao()
    {
        repelimento = true;
        direction = -direction;
        foice.transform.Rotate(0, 0, 180);
        right = !right;
    }
    public void ColisaoParede(Vector3 dir)
    {
        if(nPlayer == 1)
        {
            direction = new Vector3(direction.x * dir.x, 0, direction.z * dir.z);
            foice.transform.Rotate(0, 0, 180);
            right = !right;
            paredeCol = true;
            if (manager.rede)
            {
                //manager.EnviarDirecao(nPlayer, direction);
            }
        }
        
        //colidiu com a parede - mandar pro SERVER
        //nPlayer, vec3 direcao,
        
    }
    public void SetPosition(float x, float y, float z)
    {
        transform.position = new Vector3(x, y, z);
    }
    public void ChangeDirection(Vector3 vecs)
    {
        //Vector3 teste = new Vector3(direction.x * vecs.x, 0, direction.z * vecs.z);
        if (vecs != direction)
        {
            direction = vecs;
            foice.transform.Rotate(0, 0, 180);
            right = !right;
            //paredeCol = true;
        }
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
                EntrarGirospot(girospot);
            }
            else
            {
                //transform.SetParent(null);
                giroTime = 0f;
                SairGirospot();
            }
        }
    }
    public void EntrarGirospot(GameObject giros)
    {
        girospot = giros;
        rotatingGirospot = true;
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
        foiceee.GetComponent<FoiceCollider>().DesativarCollider();
        girospot.GetComponent<Girospot>().PlayerConectado(right, gameObject);
        //mensagem Egiro = entrar girospot;
        manager.EntreiGirospot(girospot);
    }
    public void SairGirospot()
    {
        rotatingGirospot = false;
        GetComponent<CapsuleCollider>().enabled = true;
        foiceee.GetComponent<FoiceCollider>().AtivarCollider();
        girospot.GetComponent<Girospot>().PlayerSolto();
        direction = gameObject.transform.localRotation * -Vector3.forward;
        onGirospot = false;
        //mensagem Sgiro = sair girospot
        manager.SaiGirospot(girospot, direction);
    }
    public void GirospotExit(Vector3 dir)
    {
        rotatingGirospot = false;
        GetComponent<CapsuleCollider>().enabled = true;
        foiceee.GetComponent<FoiceCollider>().AtivarCollider();
        girospot.GetComponent<Girospot>().PlayerSolto();
        direction = dir;
        onGirospot = false;
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
        /*
        if (other.gameObject.tag == "Foice" && !ghost && !repelimento)
        {
            Character ot = other.gameObject.GetComponentInParent<Character>();
            if (!ot.ghost)
            {
                Debug.Log("Player colidiu na foice");
                //impulsionar ao contrario
                InverterDirecao();
                GiroGhostOn();
            }
        }*/

        if (other.gameObject.tag == "Shield" && !repelimento)
        {
            repelimento = true;
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

    void ConnectTo()
    {
        /*NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, 1);*/
    }
    }

