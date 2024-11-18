using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Personagem : MonoBehaviour
{
    

    protected enum AudioTipos 
    { 
        Pulo, 
        Tiro, 
        Dano, 
        Morto, 
        Curar,
        Pavil,
        Explosao
    }

    #region Variaveis
    protected Rigidbody2D rb;

    [Header ("Tratativa Vida")]
    [SerializeField] public int vidaMaxima;
    [Range(0,5)]public int vida;

    public bool morto;

    [SerializeField] public float velocidade;

    [SerializeField] protected float forcaPulo;
    protected bool podePular;

    protected int cura;
    [SerializeField] protected bool podeCurar;

    [SerializeField] protected bool podeReceberDano;

    [SerializeField] protected int dano;
    protected float ataqueSegundo;
    [SerializeField] protected float proximoAtaque;
    public bool podeAtacar;
    
    [SerializeField] protected float areaChao; // = distChao
    [SerializeField] protected LayerMask camadaChao;
    [SerializeField] protected Transform posPe;
    [SerializeField] protected Vector3 posPe2;

    protected Vector2 direcao;

    [SerializeField] protected Animator anim;

    [Header("MenuManager"),Tooltip("Velocidade para evitar do personagem ficar flutuando")]
    private Vector2 velocityOnPause;

    [Header("Audio")]
    [SerializeField] protected AudioSource[] sfx;
    protected int tocarAudio;
    #endregion

    #region Metodos Unity
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        //anim.SetFloat("VelocidadeY", rb.velocity.y);
        Virar();
        //if (recebendoDano) 
    }

    protected void FixedUpdate()
    {
        Movimentacao();
        

        if(podePular)
            Pular();
    }

    #endregion

    #region Recebimento de Dano e Cura
    public virtual void ReceberDano(int dano)
    {
        if (!podeReceberDano) return;

        vida = Mathf.Max(vida - dano, 0);

        if (vida == 0) Morrer();
    }

    public void ReceberCura(int cura)
    {
        if (!podeCurar) return;

        vida = Mathf.Min(vida + cura, vidaMaxima);

        sfx[(int)AudioTipos.Curar].Play();
    }

    #endregion


    protected bool EstaNoChao()
    {
        return Physics2D.OverlapCircle(posPe.position + posPe2, areaChao, camadaChao);
    }

    protected virtual void Atacar()
    {

    }

    protected virtual void Pular()
    {
        podePular = false;
        Vector2 _velocity = rb.velocity;
        _velocity.y = 0f;
        rb.velocity = _velocity;
        rb.AddForce(Vector2.up * forcaPulo);
        sfx[(int)AudioTipos.Pulo].Play();
    }

    protected virtual void Movimentacao()
    {
        rb.velocity = new Vector2(direcao.x * velocidade, rb.velocity.y);
    }

    protected virtual void Morrer()
    {
        morto = true;
    }
    
    protected virtual void Virar()
    {
        if ((transform.localScale.x > 0f && rb.velocity.x < -0.2f) ||
            (transform.localScale.x < 0f && rb.velocity.x > 0.2f))
        {
            Debug.Log("MoonWalk");
            Vector3 _localScale = transform.localScale;
            _localScale.x *= -1f;
            transform.localScale = _localScale;
            Debug.Log(rb.velocity.x);
        }
    }

    public virtual void PauseGame() // metodo com o intuito de deixar parado os personagens quando o pause for ativado
    {
        velocityOnPause = rb.velocity;
        rb.velocity = Vector3.zero;
        rb.simulated = false;
    }

    public virtual void NoPauseGame() // reverte o PauseGame
    {
        rb.simulated = true;
        rb.velocity = velocityOnPause;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(posPe.position + posPe2, areaChao);
    }

}
