using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Net;
using System.Net.Sockets;

public class Server : MonoBehaviour
{
    private const int max_c = 2;
    private int porta = 8888;
    private int hostId;
    private int webHostId;
    private int reliableChannel;
    private int unreliableChannel;
    private bool comecou = false;
    private byte error;

    bool p1Ready = false, p2Ready = false;


    public TextMeshProUGUI tIP;

    void Start()
    {
        /*NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);
        cc.SendDelay = 0;
        cc.MinUpdateTimeout = 1;
        HostTopology topo = new HostTopology(cc, max_c);

        hostId = NetworkTransport.AddHost(topo, porta);

        comecou = true;*/

        //tIP.text = LocalIPAddress();
    }
    public void StartServer()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);
        cc.SendDelay = 0;
        cc.MinUpdateTimeout = 1;
        HostTopology topo = new HostTopology(cc, max_c);

        hostId = NetworkTransport.AddHost(topo, porta);
        Debug.Log("Socket aberto com ID: " + hostId);

        comecou = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!comecou)
        {
            return;
        }
        if (p1Ready && p2Ready)
        {
            Enviar("Comecar|",1,reliableChannel);
            Enviar("Comecar|",2,reliableChannel);
            p1Ready = false;
            p2Ready = false;
        }
        
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
                case NetworkEventType.Nothing:
                    //Debug.Log("Nothing");

                    break;
                case NetworkEventType.ConnectEvent:
                    Debug.Log("Player" + connectionId + "conectou");
                    if (connectionId >= 2)
                    {
                        Enviar("Preparados|", 1, reliableChannel);
                        Enviar("Preparados|", 2, reliableChannel);
                    }
                    Onconnection(connectionId);
                    break;
                case NetworkEventType.DataEvent:
                    //Debug.Log("DataEvent");
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    //Debug.Log("recebeu do id: " + connectionId + " " + msg);
                    string[] sepEnvio = msg.Split('|');
                    switch (sepEnvio[0])
                    {
                        /*case "Direcao":
                            //Onconnection(connectionId);
                            //string msgE = "Direcao|" ;
                            //Enviar(msgE, reliableChannel);
                            if (int.Parse(sepEnvio[1]) == 1)
                            {
                                Enviar(msg, 2, reliableChannel);
                            }
                            else
                            {
                                Enviar(msg, 1, reliableChannel);
                            }
                            break;*/
                        case "Pronto":
                            if (connectionId == 1)
                            {
                                p1Ready = true;
                                Enviar("Pronto|" + connectionId, 2, reliableChannel);
                            }
                            else
                            {
                                p2Ready = true;
                                Enviar("Pronto|" + connectionId, 1, reliableChannel);
                            }
                            break;
                        case "Posicao":
                            if (connectionId == 2)
                            {
                                Enviar(msg, 1, unreliableChannel);
                            }
                            else
                            {
                                Enviar(msg, 2, unreliableChannel);
                            }
                            break;
                        case "Egiro":
                            if (connectionId == 1)
                            {
                                Enviar(msg, 2, reliableChannel);
                            }
                            else
                            {
                                Enviar(msg, 1, reliableChannel);
                            }

                            break;
                        case "Sgiro":
                            if (connectionId == 1)
                            {
                                Enviar(msg, 2, reliableChannel);
                            }
                            else
                            {
                                Enviar(msg, 1, reliableChannel);
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
        } while (recData != NetworkEventType.Nothing);
    }
    private void Enviar(string mensagem, int playerID, int channelID)
    {
        //Debug.Log("Enviando: " + mensagem);
        //System.Text.encodi
        byte[] msg = Encoding.Unicode.GetBytes(mensagem);
        NetworkTransport.Send(hostId, playerID, channelID, msg, mensagem.Length * sizeof(char), out error);
    }
    private void Onconnection(int id)
    {
        Debug.Log("OnConnectionServer");
        Enviar("Conectou|" + id, id, reliableChannel);
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
}
