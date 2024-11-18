using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jogador : Personagem
{
    public static Jogador instance;

    #region variaveis
    [Header("Ataque")]

    [SerializeField] private GameObject balas;
    [SerializeField] private int numeroBalasMax;
    [SerializeField] private List<GameObject> balasLists;
    private int indexBalasLists;
    [SerializeField] private Balas balasScript;
    [SerializeField] private float distBalaMaxima;
    private int conTiro;

    [Header("Tratativa de Vida")]

    public bool recebendoDano;
    [HideInInspector] public bool recebendoCura;
    [SerializeField] private float timerDesativar;
    private float timer;
    //private float subTimer = 4.1f;
    public bool desativarHitStop;
    
    [SerializeField] private string sceneName;
    #endregion

    #region Metodos Unity
    protected override void Awake()
    {
        instance = this;
        base.Awake();
        IniciadorBalas();
        /*if (timer <= 0)*/ timer = 1/timerDesativar;
        /*while (timer > 0.02f)
        {
            timer -= subTimer / 2;
        }*/
    }

    protected override void Update()
    {
        base.Update();
        if (PauseMenu.instance.isPaused) return;
        if (GameManager.instance.naoMover)
        {
            PodeMover();
            return;
        }
        Inputs();
        Animacoes();
    }
    #endregion

    private void PodeMover()
    {
        direcao.x = 0;
        anim.SetFloat("VelocidadeX", 0);

    }
    void IniciadorBalas()
    {
        balasLists = new List<GameObject>();
        for (int i = 0; i < numeroBalasMax; i++)
        {
            Instantiate(balas);
        }
        var _gameObj = GameObject.FindGameObjectsWithTag("Shoot");
        foreach (GameObject obj in _gameObj)
        {
            balasLists.Add(obj);
            obj.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    private void Inputs()
    {
        if (morto)
        {
            TratativaMorte();
            return;
        }

        #region Movimentacao
        direcao.x = Input.GetAxis("Horizontal") * 1.5f;
        
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && EstaNoChao())
        {
            podePular = true;
            Debug.Log(EstaNoChao());
        }
        #endregion

        #region Ataque
        if (Input.GetKeyDown(KeyCode.P) && Time.time >= ataqueSegundo && podeAtacar)
        {
            ataqueSegundo = Time.time + proximoAtaque;
            ControleDisparo();
        }
        #endregion
    }

    void TratativaMorte()
    {
        ResetarJogo();
        direcao.x = 0;
        gameObject.layer = LayerMask.NameToLayer("Dead");
        TocarAudioMorte();
    }

    void ControleDisparo()
    {
        conTiro++;
        if (conTiro > numeroBalasMax) conTiro = 1;
        indexBalasLists = conTiro - 1;
        anim.SetTrigger("Ataque");
        Atacar();
    }

    protected override void Atacar()
    {
        balasLists[indexBalasLists].GetComponent<SpriteRenderer>().enabled = true;
        balasLists[indexBalasLists].GetComponent<Balas>().deslColl = false;
        balasLists[indexBalasLists].GetComponent<Balas>().MoverBala(new Vector2(transform.localScale.x,0f));
        sfx[(int)AudioTipos.Tiro].Play();
        
    }

    public void ControleReceberDano(int _dano)
    { 
        if (!recebendoDano && timer == 1/timerDesativar)
        {
            ReceberDano(_dano);
            recebendoDano = true;
            sfx[(int)AudioTipos.Dano].Play();
        }

        else if (recebendoDano && vida > 0)
            StartCoroutine(HitsTop());
        

        Debug.Log("Recebendo > " + timer);
        
    }

    IEnumerator HitsTop()
    {
        yield return new WaitForSeconds(1f);
        recebendoDano = false;
        desativarHitStop = true;
    }

    void Animacoes()
    {
        if (rb.velocity.x < -0.2f || rb.velocity.x > 0.2f)
            anim.SetFloat("VelocidadeX", 1);

        else
            anim.SetFloat("VelocidadeX", 0);

        anim.SetBool("EstaNoChao", EstaNoChao());
        
        anim.SetBool("Dano", recebendoDano);
        
        anim.SetFloat("VelocidadeY",rb.velocity.y);

        anim.SetBool("Morto",morto);
    }

    void TocarAudioMorte()
    {
        if (tocarAudio < 1)
        {
            tocarAudio++;
            sfx[(int)AudioTipos.Morto].Play();
        }
    }

    void ResetarJogo()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
            SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter2D(Collider2D _outro)
    {
        if (_outro.CompareTag("Finish"))
        {
             morto = true;
        }
    }
}
