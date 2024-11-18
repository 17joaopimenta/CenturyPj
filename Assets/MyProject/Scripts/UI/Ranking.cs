using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ranking : MonoBehaviour
{
    public static Ranking instance;

    [SerializeField] private TextMeshProUGUI textoDigitado;
    [SerializeField] private TextMeshProUGUI suaPontuacao;
    private string pegarNome;
    [SerializeField] private GameObject placar, nickName;
    [SerializeField] private Posicoes[] posController;
    [SerializeField] private AudioSource[] efeitoSonoroOutRank;
    [SerializeField] private AudioSource efeitoSonoroInRank;
    [SerializeField] private GameObject[] telaResultados;
    private int pontuacaoPega;
    private int nCampeao;
    public string armName, armPontuacao;

    private void Awake()
    {
        instance = this;
        foreach (var tela in telaResultados)
        {
            tela.gameObject.SetActive(false);
        }
        OrganizarPosRanking();
    }
    
    void OrganizarPosRanking()
    {
        for (int i = 0; i < posController.Length - 1; i++)
        {
            if (posController[i].pontuacao < posController[i+1].pontuacao)
            {
                var _altPos = posController[i+1].pontuacao;
                var _altNom = posController[i+1].nome;
                
                posController[i+1].pontuacao = posController[i].pontuacao;
                posController[i].pontuacao = _altPos;

                posController[i+1].nome = posController[i].nome;
                posController[i].nome = _altNom;
            }
        }
    }

    private void Update()
    {
        AtualizarRanking();
        MostrarPontos();
    }

    public void PegarNome()
    {
        pegarNome = textoDigitado.text;
        
        PlayerPrefs.SetInt(pegarNome + "i", GameManager.instance.sumPontos);
        armName = "Campeao" + nCampeao.ToString();
        armPontuacao = "PontosCampeao"+nCampeao.ToString();
        nCampeao++;
        PlayerPrefs.SetString(armName, pegarNome);
        pontuacaoPega = PlayerPrefs.GetInt(armName, GameManager.instance.sumPontos);
        
        Debug.Log(PlayerPrefs.GetString(armName) + " : " + pontuacaoPega);
    }

    public void AtivarRanking()
    {
        if (pegarNome.Length - 1 < 3) return ;
        placar.SetActive(true);
        nickName.SetActive(false);
    }

    private void AtualizarRanking()
    {
        /*if (pontuacaoPega < posController[2].pontuacao)
        {
            //StartCoroutine(OutRank());
            //AudioOutRank();
        }
        else
        {*/
            if (pontuacaoPega > posController[2].pontuacao &&
                pontuacaoPega < posController[1].pontuacao)
            {
                posController[2].pontuacao = pontuacaoPega;
                posController[2].nome = pegarNome;
                posController[2].arm = "campeao2";
                //PlayerPrefs.SetString("salvarCampeao", posController[2].arm);
                PlayerPrefs.SetString("campeao2", pegarNome);
            }
            else if (pontuacaoPega < posController[0].pontuacao &&
                     pontuacaoPega > posController[1].pontuacao &&
                     pontuacaoPega > posController[2].pontuacao)
            {
                posController[1].pontuacao = pontuacaoPega;
                posController[1].nome = pegarNome;
                posController[1].arm = "campeao1";
                //PlayerPrefs.SetString("salvarCampeao", posController[1].arm);
                PlayerPrefs.SetString("campeao1", pegarNome);
            }
            else if (pontuacaoPega > posController[0].pontuacao)
            {
                posController[0].pontuacao = pontuacaoPega;
                posController[0].nome = pegarNome;
                posController[0].arm = "campeao0";
                //PlayerPrefs.SetString("salvarCampeao", posController[0].arm);
                PlayerPrefs.SetString("campeao0", pegarNome);
            }
            PlayerPrefs.SetInt(armPontuacao, pontuacaoPega);
            //StartCoroutine(InRank());
            //AudioInRank();
        //}
    }

    /*IEnumerator InRank()
    {
        yield return new WaitForSeconds(4f);
        telaResultados[1].SetActive(true);
    }

    void AudioInRank()
    {
        if (telaResultados[1].activeSelf)
        {
            efeitoSonoroInRank.Play();
        }
    }

    IEnumerator OutRank()
    {
        yield return new WaitForSeconds(4f);
        telaResultados[0].SetActive(true);
    }

    void AudioOutRank()
    {
        if (telaResultados[0].activeSelf)
        {
            efeitoSonoroOutRank[0].Play();
        }
    }*/

    private void MostrarPontos()
    {
        suaPontuacao.text = "your score : " + pontuacaoPega;
    }
}
