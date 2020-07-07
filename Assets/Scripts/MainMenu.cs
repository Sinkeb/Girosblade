using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPage, multiPage, arenasPage;
    public Toggle broad;
    public GameObject ipInput;
    public TMP_InputField floorSkin;
    public TMP_InputField girospotSkin;
    public GameObject botConect;
    public Sprite[] arenaS;
    public Image arenaI;
    public Sprite[] skins;
    public Image skinAtual;
    public int skinA = 0;
    AudioSource audioS;
    //public void StartButton()
    //{
    //    SceneManager.LoadScene(1);
    //}
    //public void RedeButton()
    //{
    //    SceneManager.LoadScene(2);
    //}
    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }
    void play()
    {
        audioS.Play();
    }
    public void toggleChange(bool bbb)
    {
        if (broad.isOn)
        {
            if (ipInput.activeSelf)
            {
                ipInput.SetActive(false);
                botConect.SetActive(false);
            }
        }
    }
    public void QuitButton()
    {
        play();
        Application.Quit();
    }
    public void MultiPage()
    {
        play();
        mainPage.SetActive(false);
        multiPage.SetActive(true);
    }
    public void MainPage()
    {
        play();
        multiPage.SetActive(false);
        mainPage.SetActive(true);
    }
    public void ArenaPage()
    {
        play();
        multiPage.SetActive(false);
        arenasPage.SetActive(true);
    }
    public void ArenaToMultiPage()
    {
        play();
        arenasPage.SetActive(false);
        multiPage.SetActive(true);
    }
    public void CriarJogo()
    {
        play();
        //GlobalClass.floorSkin = int.Parse(floorSkin.text);
        //GlobalClass.girospotSkin = int.Parse(girospotSkin.text);
        if (broad.isOn)
        {
            GlobalClass.broadcast = true;
            GlobalClass.nn = 1;
            SceneManager.LoadScene(2);
        }
        else
        {
            GlobalClass.broadcast = false;
            GlobalClass.nn = 1;
            SceneManager.LoadScene(2);
        }

    }
    public void EntrarJogo()
    {
        play();
        //GlobalClass.floorSkin = int.Parse(floorSkin.text);
        //GlobalClass.girospotSkin = int.Parse(girospotSkin.text);
        if (broad.isOn)
        {
            GlobalClass.broadcast = true;
            GlobalClass.nn = 0;
            SceneManager.LoadScene(2);
        }
        else
        {
            ipInput.SetActive(!ipInput.activeSelf);
            botConect.SetActive(!botConect.activeSelf);
        }

    }
    public void BotaoEntrar()
    {
        play();
        GlobalClass.floorSkin = int.Parse(floorSkin.text);
        GlobalClass.girospotSkin = int.Parse(girospotSkin.text);
        ipInput.SetActive(true);
        GlobalClass.broadcast = false;
        GlobalClass.nn = 0;
        GlobalClass.ipAdress = ipInput.GetComponent<TMP_InputField>().text;
        Debug.Log(GlobalClass.ipAdress);
        SceneManager.LoadScene(2);
    }

    public void ChangeArena(bool side)
    {
        play();
        if (side)
        {
            if (GlobalClass.HostArena < 2)
                GlobalClass.HostArena++;
            else
                GlobalClass.HostArena = 0;
        }
        else
        {
            if (GlobalClass.HostArena > 0)
                GlobalClass.HostArena--;
            else
                GlobalClass.HostArena = 2;
        }
        arenaI.sprite = arenaS[GlobalClass.HostArena];
    }
    public void ChangeSkin(bool side)
    {
        play();
        if (side)
        {
            if(skinA < skins.Length - 1)
            {
                skinA++;
            }
            else
            {
                skinA = 0;
            }
        }
        else
        {
            if(skinA > 0)
            {
                skinA--;
            }
            else
            {
                skinA = skins.Length - 1;
            }
        }
        skinAtual.sprite = skins[skinA];
        GlobalClass.pSkin = skinA;
    }
}
