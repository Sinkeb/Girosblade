﻿using System.Collections;
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
    public bool preparados = false;
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
    public TextMeshProUGUI p1text, p2text;
    public TextMeshProUGUI HostIpText;

    //REDE
    private const int max_c = 2;
    private int porta = 8888;
    private int clientPort = 7777;
    private int hostId;
    private int webHostId;
    private int reliableChannel;
    private int unreliableChannel;
    private int connectionId;
    public int meuID;
    private bool conectou = false;
    //private bool comecou = false;
    private byte error;

    int broadcastKey = 1000;
    int broadcastVersion = 1;
    int broadcastSubVersion = 1;
    string broadcastData = "HELLO";

    bool isHost = false;
    bool isClient = false;

    void Start()
    {
        Application.targetFrameRate = 60;
        /*if(GlobalClass.nn == 1)
        {
            ser.GetComponent<Server>().enabled = true;
            ser.GetComponent<Server>().StartServer();
        }*/
        if (rede)
        {
            NetworkTransport.Init();
            ConnectionConfig cc = new ConnectionConfig();

            reliableChannel = cc.AddChannel(QosType.Reliable);
            unreliableChannel = cc.AddChannel(QosType.Unreliable);
            cc.SendDelay = 0;
            cc.MinUpdateTimeout = 1;
            //int teste = cc.AddChannel(QosType.)
            HostTopology topo = new HostTopology(cc, max_c);
            //NetworkTransport.host
            //if(GlobalClass.nn == 1)
            //{
                //criar host/server
            
            //if(hostId != -1)
            if (GlobalClass.nn == 1)//HOST
            {
                Debug.Log(GlobalClass.nn + "Global");
                hostId = NetworkTransport.AddHost(topo, porta);
                Debug.Log("hostId: " + hostId);
                isHost = true;
                Debug.Log("Socket aberto com ID: " + hostId);
                meuID = 2;
                p1text.text = "Inimigo";
                p2text.text = "Eu";
                p2text.color = Color.green;
                player1.GetComponent<Character>().nPlayer = 2;
                player2.GetComponent<Character>().nPlayer = 1;
                //player2.GetComponent<Character>().setMaterial(2);
                //player1.GetComponent<Character>().setMaterial(1);
                numP.text = "Player " + meuID.ToString();
                //Enviar("Teste|", reliableChannel);
                
                string ip = LocalIPAddress();
                HostIpText.text = ip;
                if (GlobalClass.broadcast)
                {
                    broadcastData = "Host:" + ip;
                    byte[] bmsg = GetBytes(broadcastData);
                    if (NetworkTransport.IsBroadcastDiscoveryRunning())
                    {
                        Debug.Log("Broadcast Already Running");
                        NetworkTransport.StopBroadcastDiscovery();
                        NetworkTransport.RemoveHost(0);
                    }
                    NetworkTransport.StopBroadcastDiscovery();
                    NetworkTransport.StartBroadcastDiscovery(hostId, clientPort, broadcastKey,
                    broadcastVersion, broadcastSubVersion, bmsg, bmsg.Length, 500, out error);
                }
                conectou = true;
            }
            else//CLIENTE
            {
                Debug.Log(GlobalClass.nn + "Global");

                isClient = true;

                hostId = NetworkTransport.AddHost(topo, clientPort);
                Debug.Log("host id: " + hostId);
                if (GlobalClass.broadcast)
                {
                    NetworkTransport.SetBroadcastCredentials(hostId, broadcastKey, broadcastVersion,
                    broadcastSubVersion, out error);
                }
                else
                {
                    connectionId = NetworkTransport.Connect(hostId, GlobalClass.ipAdress, porta, 0, out error);
                    //connectionId = NetworkTransport.Connect(hostId, "192.168.0.103", porta, 0, out error);
                    conectou = true;
                }
                HostIpText.text = GlobalClass.ipAdress;
                Debug.Log(connectionId + " ID");
            }
            //Debug.Log(hostId+ " hostID");
            //192.168.0.104
            //192.168.0.2
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isClient && !conectou)
        {
            int connectionId;
            int channelId;
            byte error;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType networkEvent;
            do
            {
                networkEvent = NetworkTransport.ReceiveFromHost(hostId, out connectionId, 
                    out channelId, recBuffer, bufferSize, out dataSize, out error);
                Debug.Log(networkEvent + " teste Broadcast");

                if(networkEvent == NetworkEventType.BroadcastEvent)
                {
                    string hostIP;
                    int hostPort;
                    NetworkTransport.GetBroadcastConnectionInfo(hostId, out hostIP, out hostPort, out error);

                    NetworkTransport.GetBroadcastConnectionMessage(hostId, recBuffer, bufferSize, 
                        out dataSize, out error);
                    OnReceivedBroadcast(hostIP, GetString(recBuffer));
                }
            } while (networkEvent != NetworkEventType.Nothing);
        }

        if (conectou)
        {
            NetworkEventType recData;
            do
            {
            int recHostId;
            int connectionId;
            int channelId;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            byte error;
            recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
            switch (recData)
            {
                case NetworkEventType.ConnectEvent:
                        if(meuID == 2)
                        {
                            EnviarPlayer("Conectou|" + connectionId, connectionId, reliableChannel);
                            Debug.Log("Conexao com : " + connectionId);
                            NetworkTransport.StopBroadcastDiscovery();
                            p1text.color = Color.green;
                            preparados = true;
                        }
                        else
                        {
                            Debug.Log("me conectei com id: " + connectionId);
                            p2text.color = Color.green;
                            preparados = true;
                        }
                        goto case NetworkEventType.Nothing;
                    case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    //Debug.Log(msg);
                    string[] sepEnvio = msg.Split('|');
                    switch (sepEnvio[0])
                    {
                            /*case "Preparados":
                                preparados = true;
                                //p1text.color = Color.green;
                                //p2text.color = Color.green;
                                break;*/
                        case "Conectou":
                                meuID = int.Parse(sepEnvio[1]);
                                numP.text ="Player " + meuID.ToString();
                                if (meuID == 1)
                                {
                                    player1.GetComponent<Character>().nPlayer = 1;
                                    player2.GetComponent<Character>().nPlayer = 2;
                                    //player2.GetComponent<Character>().setMaterial(2);
                                    //player1.GetComponent<Character>().setMaterial(1);
                                    p1text.text = "Eu";
                                    p1text.color = Color.green;
                                    p2text.text = "Inimigo";
                                }
                            break;
                        case "Direcao":
                            float x, y, z;
                            x = float.Parse(sepEnvio[2]);
                            y = float.Parse(sepEnvio[3]);
                            z = float.Parse(sepEnvio[4]);
                            JsonSerial dFoice = JsonUtility.FromJson<JsonSerial>(sepEnvio[5]);
                            Vector3 dir = new Vector3(x, y, z);
                                if (meuID == 2)
                                {
                                    player1.GetComponent<Character>().ChangeDirection(dir);
                                    player1.GetComponent<Character>().SetFoiceT(dFoice.pp, dFoice.rr);
                                }
                                else
                                {
                                    player2.GetComponent<Character>().ChangeDirection(dir);
                                    player2.GetComponent<Character>().SetFoiceT(dFoice.pp, dFoice.rr);
                                }
                                break;
                        case "Pronto":
                                if (meuID == 1)
                                {
                                    player2Ready = true;
                                    check2.enabled = true;
                                    //p2text.color = Color.green;
                                }
                                else
                                {
                                    //p1text.color = Color.green;
                                    player1Ready = true;
                                    check1.enabled = true;
                                    //EnviarPlayer("Preparados|", 1, reliableChannel);
                                    //preparados = true;
                                }
                                break;
                        case "Comecar":
                            ComecarPartida();
                            break;
                        case "Posicao":
                            float xp, yp, zp;
                            xp = float.Parse(sepEnvio[2]);
                            yp = float.Parse(sepEnvio[3]);
                            zp = float.Parse(sepEnvio[4]);
                            JsonSerial pFoice = JsonUtility.FromJson<JsonSerial>(sepEnvio[5]);
                            if (meuID == 2)
                            {
                                //player1
                                player1.GetComponent<Character>().SetPosition(xp, yp, zp);
                                player1.GetComponent<Character>().SetFoiceT(pFoice.pp, pFoice.rr);
                            }
                            else
                            {
                                //player2
                                player2.GetComponent<Character>().SetPosition(xp, yp, zp);
                                player2.GetComponent<Character>().SetFoiceT(pFoice.pp, pFoice.rr);
                            }
                                break;
                        case "Egiro":
                            if(meuID == 2)
                            {
                                //player1 entrou girospot
                                player1.GetComponent<Character>().GirospotEnter(girospots[int.Parse(sepEnvio[2])], bool.Parse(sepEnvio[3]));

                            }
                            else
                            {
                                //player2 entrou girospot
                                player2.GetComponent<Character>().GirospotEnter(girospots[int.Parse(sepEnvio[2])], bool.Parse(sepEnvio[3]));
                            }
                            break;
                        case "Sgiro":
                            if (meuID == 2)
                            {
                                //player1 saiu girospot
                                player1.GetComponent<Character>().GirospotExit();
                            }
                            else
                            {
                                //player2 saiu girospot
                                player2.GetComponent<Character>().GirospotExit();
                            }
                            break;
                            case "InvDir":
                                Debug.Log("Inverter");
                                if(meuID == 1)
                                {
                                    player1.GetComponent<Character>().InverterDirecao();
                                }
                                break;
                            case "TomouDano":
                                player1.GetComponent<Character>().GiroGhostOn();
                                break;
                            case "CausouDano":
                                player2.GetComponent<Character>().GiroGhostOn();
                                break;
                            case "Terminou":
                                terminarPartida(int.Parse(sepEnvio[1]));
                                break;

                        }
                        goto case NetworkEventType.Nothing;
                case NetworkEventType.DisconnectEvent:
                        //Debug.Log("DisconnectEvent");

                        goto case NetworkEventType.Nothing;

                    case NetworkEventType.BroadcastEvent:
                        //Debug.Log("BroadcastEvent");
                        goto case NetworkEventType.Nothing;
                    case NetworkEventType.Nothing:
                        //Debug.Log("Nothing");
                        continue;
                }
            } while (recData != NetworkEventType.Nothing);
            
        }
        if(player1Ready && player2Ready && rede && !jogando)
        {
            ComecarPartida();
        }
        if(player1Ready && player2Ready && !jogando && !rede)
        {
            comecou = true;
            startPanel.SetActive(false);
            Debug.Log("todos prontos");
        }
        if (comecou && !rede)
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
    public void OnReceivedBroadcast(string fromAddress, string data)
    {
        if (!conectou)
        {
            //Debug.Log("Got broadcast from [" + fromAddress + "] " + data);
            string[] itens = data.Split(':');
            Debug.Log("Host IP: " + itens[1]);
            HostIpText.text = itens[1];
            GlobalClass.ipAdress = itens[1];
            connectionId = NetworkTransport.Connect(hostId, GlobalClass.ipAdress, porta, 0, out error);
            conectou = true;
            Debug.Log(connectionId + " ID");
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
                EnviarPlayer(msg,1, reliableChannel);
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
        if (meuID == 2)
        {
            NetworkTransport.RemoveHost(hostId);
        }
        SceneManager.LoadScene(0);
    }
    
    private void Enviar(string mensagem, int channelID)
    {
        //Debug.Log("Enviando: " + mensagem);
        //System.Text.encodi
        byte[] msg = Encoding.Unicode.GetBytes(mensagem);
        NetworkTransport.Send(hostId, connectionId, channelID, msg, mensagem.Length * sizeof(char), out error);
        //NetworkTransport.start
    }
    private void EnviarPlayer(string mensagem, int playerID, int channelID)
    {
        //Debug.Log("Enviando: " + mensagem);
        //System.Text.encodi
        byte[] msg = Encoding.Unicode.GetBytes(mensagem);
        NetworkTransport.Send(hostId, playerID, channelID, msg, mensagem.Length * sizeof(char), out error);
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
    
    public void EntreiGirospot(GameObject giro, bool or)
    {
        int idgiro = getIndiceGirospot(giro);
        if (idgiro != -1)
        {
            Debug.Log(idgiro + " idGirospot entrei");
            string msg = "Egiro|" + meuID + "|" + idgiro + "|" + or;
            if(meuID == 1)
            {
                Enviar(msg, reliableChannel);
            }
            else
            {
                EnviarPlayer(msg,1, reliableChannel);
            }
        }
    }
    public void SaiGirospot(GameObject giro)
    {
        string msg = "Sgiro|" + meuID;
        if (meuID == 1)
        {
            Enviar(msg, reliableChannel);
        }
        else
        {
            EnviarPlayer(msg, 1, reliableChannel);
        }
    }
    int getIndiceGirospot(GameObject giro)
    {
        for (int i = 0; i < girospots.Length; i++)
        {
            if (giro == girospots[i])
            {
                return i;
            }
        }
        return -1;
    }
    void ComecarPartida()
    {
        comecou = true;
        startPanel.SetActive(false);
        Debug.Log("todos prontos");
        jogando = true;
        
        if(meuID ==1) {
            //player1.GetComponent<Character>().EntrarGirospot(girospots[0]);
            player1.GetComponent<Character>().ChangeDirection(new Vector3(-1, 0, 1));
            //player2.GetComponent<Character>().ChangeDirection(new Vector3(-1, 0, 1));
        }
        else
        {
            //player1.GetComponent<Character>().ChangeDirection(new Vector3(1, 0, 1));
            player2.GetComponent<Character>().ChangeDirection(new Vector3(1, 0, 1));
            //player2.GetComponent<Character>().EntrarGirospot(girospots[girospots.Length - 1]);
        }

        comecou = false;
        Debug.Log("comecou");
    }
    public void Perdi(int nPlayer)
    {
        endPanel.SetActive(true);
        jogando = false;
        player1Ready = false;
        player2Ready = false;
        if (nPlayer == 1)
        {
            winner.text = "Vitória! Parabéns";
        }
        else
        {
            winner.text = "Perdeu! Que pena";
        }
    }
    public void terminarPartida(int nn)
    {
        if(meuID == 2)
        {
            EnviarPlayer("Terminou|" + nn, 1, reliableChannel);
        }
        if(meuID == 1)
        {
            if (nn == 1)
            {
                //meuId 1 ganhou
                winner.text = "Venceu! Muito bem";
            }
            else
            {
                winner.text = "Perdeu! Que pena";
            }
        }
        else
        {
            if (nn == 1)
            {
                //meuId 1 ganhou
                winner.text = "Perdeu! Que pena";
            }
            else
            {
                winner.text = "Venceu! Muito bem";
            }
        }
        
        endPanel.SetActive(true);
        jogando = false;
        player1Ready = false;
        player2Ready = false;
    }
    public void EnviarPosicao(float x, float y, float z, Vector3 foiceP, Quaternion foiceR)
    {
        JsonSerial js = new JsonSerial();
        js.pp = foiceP;
        js.rr = foiceR;
        string dataAsJson = JsonUtility.ToJson(js);
        string msg = "Posicao|" + meuID + "|" + x + "|" + y + "|" + z + "|"+ dataAsJson;
        if(meuID == 1)
        {
            Enviar(msg, unreliableChannel);
        }
        else
        {
            EnviarPlayer(msg,1, unreliableChannel);
        }
    }
    public void EnviarDirecao(Vector3 direc, Vector3 foiceP, Quaternion foiceR)
    {
        JsonSerial js = new JsonSerial();
        js.pp = foiceP;
        js.rr = foiceR;
        string dataAsJson = JsonUtility.ToJson(js);
        string m = "Direcao|" + meuID + "|" + direc.x + "|" + direc.y + "|" + direc.z + "|" + dataAsJson;
        if (meuID == 1)
        {
            Enviar(m, unreliableChannel);
        }
        else
        {
            EnviarPlayer(m, 1, unreliableChannel);
        }
    }
    public void EnviarInverterDirecao()
    {
        EnviarPlayer("InvDir|", 1, reliableChannel);
    }
    public void ClienteTomouDano()
    {
        EnviarPlayer("TomouDano|", 1, reliableChannel);
    }
    public void ClienteMeAcertou()
    {
        EnviarPlayer("CausouDano|", 1, reliableChannel);
    }
    static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }
    static string GetString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length / sizeof(char)];
        System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }
}
