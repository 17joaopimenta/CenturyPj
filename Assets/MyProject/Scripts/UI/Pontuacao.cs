using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pontuacao : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        text.text = "Parabéns! \r\n Sua Pontuação :" + GameManager.instance.sumPontos;
    }

    public void Carregar()
    {
        SceneManager.LoadScene("Menu");
    }
}
