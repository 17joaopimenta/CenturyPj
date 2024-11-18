using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Interface Vida
    enum EstadoVida
    {
        Vazia,
        Cheia
    }
    [SerializeField] EstadoVida estadoDaVida;

    [SerializeField] private Jogador jogador;
    [SerializeField] private List<GameObject> vidasGameObj;
    [SerializeField] private GameObject vidaOriginal;
    [SerializeField] private GameObject[] pegarVidasNaCena;
    [SerializeField] private Sprite[] estadoVida;
    [SerializeField] private GameObject gameOver;
    #endregion

    #region Timer
    [SerializeField] private TextMeshProUGUI timer;
    private float delay;
    [SerializeField] private float timerCounter, delaySoma;
    #endregion

    private int quantInimigo, sumInimigoMorto;
    [SerializeField] private GameObject telaFinal, telaVitoria;
    [SerializeField] private TextMeshProUGUI[] criterioPontos;
    private bool naoInvocar;
    public bool naoMover;
    public int sumPontos;
    private int sumVidas;

    [Header("Musica")]
    [SerializeField] AudioSource musicaTutorial;
    [SerializeField] AudioSource musicaJogo;

    private void Awake()
    {
        instance = this;
        delay = delaySoma;
        gameOver.SetActive(false);
        quantInimigo = GameObject.FindGameObjectsWithTag("Enemy").Length;
        IniciadorVida();
        foreach (TextMeshProUGUI criterios in criterioPontos)
        {
            criterios.text = "";
        }
        sumVidas = jogador.vida;
        Debug.Log("VidasJogador : " + sumVidas);
    }

    private void Start()
    {
        if (!Tutorial.instance.podeAlterarPosPlayer) musicaTutorial.Play();
        else
        {
            if (musicaTutorial.isPlaying) musicaTutorial.Pause();
            musicaJogo.Play();
        }
        InvokeRepeating("TrocarMusica", 0f, 0.01f);
    }

    void TrocarMusica()
    {
        if (Tutorial.instance.desativarTutorial && musicaTutorial.isPlaying)
        {
            Debug.Log("Trocar");
            musicaTutorial.Pause();
            musicaJogo.Play();
        }
    }

    void IniciadorVida()
    {
        for (int i = 0; i < pegarVidasNaCena.Length; i++)
        {
            if (i < jogador.vida)
                estadoDaVida = EstadoVida.Cheia;
            else
                estadoDaVida = EstadoVida.Vazia;
            
            pegarVidasNaCena[i].GetComponent<Image>().sprite = estadoVida[(int)estadoDaVida];
        }

    }

    public void SomarQuantInimigo(int _subQtd)
    {
        sumInimigoMorto = Mathf.Min(sumInimigoMorto + _subQtd, quantInimigo);
    }

    private void Update()
    {
        Debug.Log("Quantidade inimigo total : "+quantInimigo);
        TratativaVida();
        AtivarGameOver();
        DigitarHora();
    }

    void DigitarHora()
    {
        timer.text = timerCounter.ToString("00");
        if (!naoInvocar && !PauseMenu.instance.isPaused && Tutorial.instance.ativarTimer) Invoke("DecrementarTempo",0f);
    }

    void DecrementarTempo()
    {
        if ((int)Time.time >= delay)
        {
            delay = delaySoma + Time.time;
            if (timerCounter > 0) timerCounter -= 1f;
        }
    }
    public void TelaFinal(Collider2D _flag)
    {
        if (ChegouNoFinal(_flag))
        {
            Debug.Log("Tela Final");
            naoInvocar = true;
            naoMover = true;
            telaFinal.SetActive(true);
            InformarItens();
            CancelInvoke("DecrementarTempo");
        }
    }

    void InformarItens()
    {
        StartCoroutine(EscreverItens());
        sumPontos = (int)timerCounter*10 + jogador.vida*10 + sumInimigoMorto*5;
    }

    IEnumerator EscreverItens()
    {

        yield return new WaitForSeconds(0.5f);
        criterioPontos[0].text = "Time : " + ((int)timerCounter * 10).ToString("00");

        yield return new WaitForSeconds(0.5f);
        criterioPontos[1].text = "Life : " + (jogador.vida * 10);

        yield return new WaitForSeconds(0.5f);
        criterioPontos[2].text = "Dead Enemies : " + sumInimigoMorto * 5;

        yield return new WaitForSeconds(0.7f);
        criterioPontos[3].text = "Total : " + sumPontos;

        yield return new WaitForSeconds(1.0f);
        foreach (var item in criterioPontos)
        {
            item.gameObject.SetActive(false);
        }
        telaFinal.SetActive(false);

        yield return new WaitForSeconds(1.5f);
        telaVitoria.SetActive(true);

    }

    public bool ChegouNoFinal(Collider2D _flag)
    {
        return _flag.CompareTag("Player");
    }

    private void TratativaVida()
    {
        if (jogador.recebendoDano)
        {
            pegarVidasNaCena[jogador.vida].GetComponent<Image>().sprite = estadoVida[(int)EstadoVida.Vazia];
        }

        if (jogador.recebendoCura)
        {
            pegarVidasNaCena[jogador.vida - 1].GetComponent<Image>().sprite = estadoVida[(int)EstadoVida.Cheia];
            sumVidas = jogador.vida;
            jogador.recebendoCura = false;
        }
    }

    private void AtivarGameOver()
    {
        if (jogador.morto)        
            gameOver.SetActive(true);
    }
}
