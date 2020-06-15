using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPage, multiPage;
    public Toggle broad;
    public GameObject ipInput;
    public TMP_InputField floorSkin;
    public TMP_InputField girospotSkin;
    public GameObject botConect;

    //public void StartButton()
    //{
    //    SceneManager.LoadScene(1);
    //}
    //public void RedeButton()
    //{
    //    SceneManager.LoadScene(2);
    //}
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
        Application.Quit();
    }
    public void MultiPage()
    {
        mainPage.SetActive(false);
        multiPage.SetActive(true);
    }
    public void MainPage()
    {
        multiPage.SetActive(false);
        mainPage.SetActive(true);
    }
    public void CriarJogo()
    {
        GlobalClass.floorSkin = int.Parse(floorSkin.text);
        GlobalClass.girospotSkin = int.Parse(girospotSkin.text);
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
        GlobalClass.floorSkin = int.Parse(floorSkin.text);
        GlobalClass.girospotSkin = int.Parse(girospotSkin.text);
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
        GlobalClass.floorSkin = int.Parse(floorSkin.text);
        GlobalClass.girospotSkin = int.Parse(girospotSkin.text);
        ipInput.SetActive(true);
        GlobalClass.broadcast = false;
        GlobalClass.nn = 0;
        GlobalClass.ipAdress = ipInput.GetComponent<TMP_InputField>().text;
        Debug.Log(GlobalClass.ipAdress);
        SceneManager.LoadScene(2);
    }
}
