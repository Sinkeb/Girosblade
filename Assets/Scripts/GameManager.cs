using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] girospots;
    GameObject player1, player2;

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


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
        if(nPlayer == 1)
        {
            player1Ready = true;
            player1 = p;
            check1.enabled = true;

        }
        else if(nPlayer == 2) {
            player2Ready = true;
            player2 = p;
            check2.enabled = true;
        }
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
}
