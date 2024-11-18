using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] string sceneName;
    [SerializeField] AudioSource uiSound;

   /* public bool isPaused;
    public GameObject pausePanel;
    public string cena;*/

    Jogador jogador;

    private void Awake()
    {
        /*instance = this;
        isPaused = false;
        jogador = GameObject.FindGameObjectWithTag("Player").GetComponent<Jogador>();*/
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
        uiSound.Play();
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }

    public void MenuGo()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("sair");
    }

    /**/

}
