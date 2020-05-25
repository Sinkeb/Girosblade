using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPage, multiPage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }
    public void RedeButton()
    {
        SceneManager.LoadScene(2);
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
        GlobalClass.nn = 1;
        SceneManager.LoadScene(2);
    }
    public void EntrarJogo()
    {
        GlobalClass.nn = 0;
        SceneManager.LoadScene(2);
    }
}
