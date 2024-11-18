using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public bool isPaused;
    public GameObject pausePanel;
    public string cena;

    Jogador jogador;

    private void Awake()
    {
        instance = this;
        isPaused = false;
        jogador = GameObject.FindGameObjectWithTag("Player").GetComponent<Jogador>();
    }

    void Update()
    {
        PausarJogo();
    }

    #region Funcoes Pause
    public void Resume()
    {
        PauseScreen();
    }

    public void GoMenu()
    {
        SceneManager.LoadScene(cena);
    }

    #endregion

    #region Tratativa do Pause
    public void PausarJogo()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseScreen();
        }
    }

    public void PauseScreen()
    {
        if (jogador == null) return;
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (isPaused)
        {
            jogador.PauseGame();

            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Inimigo>().PauseGame();
            }
        }
        else
        {
            jogador.NoPauseGame();

            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Inimigo>().NoPauseGame();
            }
        }
    }
    #endregion
}
