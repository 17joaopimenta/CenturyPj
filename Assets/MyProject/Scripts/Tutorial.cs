using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;
using Unity.VisualScripting;

public class Tutorial : MonoBehaviour
{
    //[SerializeField] GameObject[] textos;

    public static Tutorial instance;

    enum txt {Run, Jump, Shoot}
    [SerializeField] txt textos;
    [SerializeField] GameObject[] text;
    [SerializeField] GameObject ground;
    [SerializeField] GameObject[] wall;
    [SerializeField] GameObject enemyTest;
    [SerializeField] GameObject skipTutorial;
    private GameObject jogador;
    public bool ativarTimer;
    private float timerTutoriaSkip;
    [SerializeField] private float timerTutoriaSkipSum;
    [SerializeField] GameObject normalEnemy;

    [SerializeField] string[] message;
    [SerializeField] TextMeshProUGUI messageTutorial;

    [Header("Area Checar Player")]
    [SerializeField] Vector2 posArea;
    [SerializeField] Vector2 area;
    [SerializeField] LayerMask areaLayer;
    private int desativarTutorialInt;
    [SerializeField] float posPlayer;
    public bool desativarTutorial, podeAlterarPosPlayer;


    private void Awake()
    {
        instance = this;
        textos = txt.Run;
        normalEnemy.SetActive(false);
        timerTutoriaSkip = timerTutoriaSkipSum;
        jogador = GameObject.FindGameObjectWithTag("Player");
        messageTutorial.text = message[Random.Range(0, message.Length)];
        desativarTutorialInt = PlayerPrefs.GetInt("alterarPosPlayer", 0);
        /*StartCoroutine(AtivarSkip());*/
    }

    private void Start()
    {
        podeAlterarPosPlayer = System.Convert.ToBoolean(desativarTutorialInt);
        Debug.Log("Alterar Posicao Jogador? "+podeAlterarPosPlayer);
        if (podeAlterarPosPlayer)
        {
            var _nPos = Jogador.instance.transform.position;
            _nPos.y = posPlayer;
            Jogador.instance.transform.position = _nPos;
        }
    }

    bool Parado()
    {
        if (jogador.GetComponent<Rigidbody2D>().velocity.x == 0 && jogador.GetComponent<Rigidbody2D>().velocity.y == 0
            && !Input.GetKeyDown(KeyCode.P))
        {
            return true;
        }
        return false;
    }

    private void Update()
    {
        IniciadorTutorial();
        TrocarTexto();
        TratativaTutorial();
    }

    void IniciadorTutorial()
    {
        if (Parado() && Time.time >= timerTutoriaSkip && !desativarTutorial)
        {
            timerTutoriaSkip = Time.time + timerTutoriaSkipSum;
            skipTutorial.SetActive(true);
        }
        else if (!Parado() || desativarTutorial)       
            skipTutorial.SetActive(false);
        
    }

    bool PlayerColidiu()
    {
        return Physics2D.OverlapBox(posArea,area,0f,areaLayer);
    }

    void TratativaTutorial()
    {
        if (PlayerColidiu())
        {
            ativarTimer = true;
            normalEnemy.SetActive(true);
            skipTutorial.SetActive(false);
            desativarTutorial = true;
            PlayerPrefs.SetInt("alterarPosPlayer", 1);
        }
    }


    private void TrocarTexto()
    {
        switch (textos)
        {
            case txt.Run:
                if (jogador.GetComponent<Rigidbody2D>().velocity.x < -0.2f || jogador.GetComponent<Rigidbody2D>().velocity.x > 0.2f)
                {
                    text[(int)txt.Run].SetActive(false);
                    text[(int)txt.Jump].SetActive(true);
                    textos = txt.Jump;
                }
                break;

            case txt.Jump:
                if (jogador.GetComponent<Rigidbody2D>().velocity.y > 0.2f)
                {
                    text[(int)txt.Jump].SetActive(false);
                    text[(int)txt.Shoot].SetActive(true);
                    enemyTest.SetActive(true);
                    textos = txt.Shoot;
                }
                break;

            /*case txt.Shoot:
                TratativaShoot();
                break;*/
        }
    }

    public void TrocarTexto(Collider2D bala)
    {
        if (textos == txt.Shoot)
        {
            if (bala.CompareTag("TrainingEnemy"))
            {
                text[(int)txt.Shoot].SetActive(false);
                Invoke("DesativarInimigoEChao", 2f);   
            }
        }
    }

    public void ChecarColisao(Collider2D bala)
    {
        if (Balas.instance.isCollided)
        {
            text[(int)txt.Shoot].SetActive(false);
            Invoke("AtivarInimigoEChao", 2f);
        }
    }

    public void DesativarInimigoEChao()
    {
        ground.SetActive(false);
        wall[0].SetActive(false);
        wall[1].SetActive(false);
        if (text[(int)txt.Run].activeSelf) text[(int)txt.Run].SetActive(false);
        if (text[(int)txt.Jump].activeSelf) text[(int)txt.Jump].SetActive(false);
        if (text[(int)txt.Shoot].activeSelf) text[(int)txt.Shoot].SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(posArea,area);
    }

}
