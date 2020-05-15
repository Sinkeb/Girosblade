using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;

public class GameManager : MonoBehaviour
{
    public bool rede = false;
    public TextMeshProUGUI numP;
    public GameObject[] girospots;
    public GameObject player1, player2;

    public bool comecou = false;
    public bool jogando = false;
    bool player1Ready = false;
    bool player2Ready = false;

    public TextMeshProUGUI tempo;
    float time = 0f;
    string minutos;
    string segundos;
    public GameObject endPanel;
    public TextMeshProUGUI winner;

    public GameObject startPanel;
    public Image check1, check2;

    //REDE
    private const int max_c = 2;
    private int porta = 8888;
    private int hostId;
    private int webHostId;
    private int reliableChannel;
    private int unreliableChannel;
    private int connectionId;
    private int meuID;
    private bool conectou = false;
    //private bool comecou = false;
    private byte error;

    void Start()
    {
        if (rede)
        {
            NetworkTransport.Init();
            ConnectionConfig cc = new ConnectionConfig();

            reliableChannel = cc.AddChannel(QosType.Reliable);
            unreliableChannel = cc.AddChannel(QosType.Unreliable);
            HostTopology topo = new HostTopology(cc, max_c);
            //hostId = NetworkTransport.AddHost(topology, 8888);
            hostId = NetworkTransport.AddHost(topo, 0);
            //Debug.Log(hostId+ " hostID");
            //192.168.0.104
            //192.168.0.2
            //connectionId = NetworkTransport.Connect(hostId, ip, porta, 0, out error);
            string ip = LocalIPAddress();
            Debug.Log(ip);
            connectionId = NetworkTransport.Connect(hostId, ip, porta, 0, out error);
            //Debug.Log(connectionId + " ID");
            conectou = true;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (conectou)
        {
            int recHostId;
            int connectionId;
            int channelId;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            byte error;
            NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
            switch (recData)
            {
                case NetworkEventType.Nothing:
                    //Debug.Log("Nothing");
                    break;
                case NetworkEventType.ConnectEvent:
                    Debug.Log("Conectado");
                    //meuID = connectionId;
                    numP.text = meuID.ToString();
                    
                    //string msgE = "Conectou|" + connectionId;
                    //Enviar(msgE, reliableChannel);
                    break;
                case NetworkEventType.DataEvent:
                    //Debug.Log("DataEvent");
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    string[] sepEnvio = msg.Split('|');
                    switch (sepEnvio[0])
                    {
                        case "Conectou":
                            meuID = int.Parse(sepEnvio[1]);
                            numP.text = meuID.ToString();
                            if (meuID == 1)
                            {
                                player1.GetComponent<Character>().nPlayer = 1;
                                player2.GetComponent<Character>().nPlayer = 2;
                            }
                            else
                            {
                                player1.GetComponent<Character>().nPlayer = 2;
                                player2.GetComponent<Character>().nPlayer = 1;
                            }
                            break;
                        case "Direcao":
                            float x, y, z;
                            x = float.Parse(sepEnvio[2]);
                            y = float.Parse(sepEnvio[3]);
                            z = float.Parse(sepEnvio[4]);
                            Vector3 dir = new Vector3(x, y, z);
                            player2.GetComponent<Character>().ColisaoParede(dir);
                            break;
                        case "Pronto":
                            if(int.Parse(sepEnvio[1]) != meuID)
                            {
                                if (meuID == 1)
                                {
                                    player2Ready = true;
                                    check2.enabled = true;
                                }
                                else
                                {
                                    player1Ready = true;
                                    check1.enabled = true;
                                }
                            }
                            break;
                       
                    }

                    break;
                case NetworkEventType.DisconnectEvent:
                    //Debug.Log("DisconnectEvent");

                    break;

                case NetworkEventType.BroadcastEvent:
                    //Debug.Log("BroadcastEvent");
                    break;
            }
        }

        if(player1Ready && player2Ready && !jogando)
        {
            comecou = true;
            startPanel.SetActive(false);
            Debug.Log("todos prontos");
        }
        if (comecou)
        {
            jogando = true;
            player1.GetComponent<Character>().EntrarGirospot(girospots[0]);
            player2.GetComponent<Character>().EntrarGirospot(girospots[girospots.Length - 1]);
            comecou = false;
            Debug.Log("comecou");
        }
        if (jogando)
        {
            time += Time.deltaTime;
            minutos = ((int) time / 60).ToString();
            segundos = (time % 60).ToString("f0");
            tempo.text = minutos + ":" + segundos;
        }
        
    }
    public void PlayerReady(int nPlayer, GameObject p)
    {
        if(nPlayer == 1 && !rede)
        {
            player1Ready = true;
            player1 = p;
            check1.enabled = true;
        }
        else if(nPlayer == 2 && !rede) {
            player2Ready = true;
            player2 = p;
            check2.enabled = true;
        }
        if (rede)
        {
            if(meuID == 1)
            {
                player1Ready = true;
                //player1 = p;
                check1.enabled = true;
                string msg = "Pronto|" + meuID;
                Enviar(msg, reliableChannel);
            }
            else
            {
                player2Ready = true;
                //player1 = p;
                check2.enabled = true;
                string msg = "Pronto|" + meuID;
                Enviar(msg, reliableChannel);
            }
            
        }
        /* if(meuID == 1)
             player1.GetComponent<Character>().nPlayer = 1;
             player2.GetComponent<Character>().nPlayer = 2;
           else
            player1.GetComponent<Character>().nPlayer = 2;
            player2.GetComponent<Character>().nPlayer = 1;*/
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Perdi(int nPlayer)
    {
        endPanel.SetActive(true);
        jogando = false;
        player1Ready = false;
        player2Ready = false;
        if(nPlayer == 1)
        {
            winner.text = "Player 2 Ganhou!";
        }
        else
        {
            winner.text = "Player 1 Ganhou!";
        }
    }
    private void Enviar(string mensagem, int channelID)
    {
        Debug.Log("Enviando: " + mensagem);
        //System.Text.encodi
        byte[] msg = Encoding.Unicode.GetBytes(mensagem);
        NetworkTransport.Send(hostId, connectionId, channelID, msg, mensagem.Length * sizeof(char), out error);
    }
    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
    public void EnviarDirecao(int id, Vector3 direc)
    {
        string m = "Direcao|" + connectionId + "|" + direc.x + "|" + direc.y + "|" + direc.z;
        Enviar(m, reliableChannel);
    }
}
