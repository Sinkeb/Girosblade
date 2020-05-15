using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

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

    void Start()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, max_c);

        hostId = NetworkTransport.AddHost(topo, porta);

        comecou = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!comecou)
        {
            return;
        }
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
                Debug.Log("Player" + connectionId + "conectou");
                Onconnection(connectionId);
                break;
            case NetworkEventType.DataEvent:
                //Debug.Log("DataEvent");
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                Debug.Log("recebeu do id: " + connectionId + " " + msg);
                string[] sepEnvio = msg.Split('|');
                switch (sepEnvio[0])
                {
                    case "Direcao":
                        //Onconnection(connectionId);
                        //string msgE = "Direcao|" ;
                        //Enviar(msgE, reliableChannel);
                        /*if (connectionId == 1)
                        {
                            Enviar(msg, 2, reliableChannel);
                        }
                        else
                        {
                            Enviar(msg, 1, reliableChannel);
                        }*/
                        break;
                    case "Pronto":
                        if(connectionId == 1)
                        {
                            Enviar("Pronto|" + connectionId, 2, reliableChannel);
                        }
                        else
                        {
                            Enviar("Pronto|" + connectionId, 1, reliableChannel);
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
    private void Enviar(string mensagem, int playerID, int channelID)
    {
        Debug.Log("Enviando: " + mensagem);
        //System.Text.encodi
        byte[] msg = Encoding.Unicode.GetBytes(mensagem);
        NetworkTransport.Send(hostId, playerID, channelID, msg, mensagem.Length * sizeof(char), out error);
    }
    private void Onconnection(int id)
    {
        Enviar("Conectou|" + id, id, reliableChannel);
    }
}
