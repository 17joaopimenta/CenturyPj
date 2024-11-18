using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Posicoes : MonoBehaviour
{
    public enum Colocacao
    {
        primeiro,
        segundo,
        terceiro
    }

    public Colocacao colocacao;
    private string[] textoColocao = {"1st","2nd","3rd"};

    public int pontuacao;
    public string nome,arm;
    private TextMeshProUGUI texto;

    private void Awake()
    {
        nome = PlayerPrefs.GetString(arm,nome);
        PlayerPrefs.SetString("Campeao", arm);
        arm = PlayerPrefs.GetString("Campeao");
        pontuacao = PlayerPrefs.GetInt(Ranking.instance.armPontuacao,pontuacao);
        texto = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        EscreverPlacar();
    }

    private void EscreverPlacar()
    {
        texto.text = textoColocao[(int)colocacao] + ": " + nome + " > " + pontuacao;
    }
}
