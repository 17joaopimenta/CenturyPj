using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : Personagem
{
    #region Variaveis
    protected float proximaPatrulha;
    [SerializeField] protected float intervaloPatrulha;
    [SerializeField] protected float raioPatrulha;
    public int danoDiferenciado;

    protected float posInicialX;

    protected Vector2 destino;

    [SerializeField] protected EnemyCol iscol;
    #endregion

    protected virtual void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
        Colidiu();
    }

    protected virtual void TratativaIA()
    {
        Patrulhar();
    }

    protected virtual void Colidiu() 
    {
        if (iscol.isCollided)
        { 
            direcao.x *= -1;
            iscol.isCollided = false;
        }
    }

    protected virtual void Patrulhar()
    {
        if (Time.time >= proximaPatrulha)
        {
            proximaPatrulha = Time.time + intervaloPatrulha;
            destino = transform.position;
            destino.x = posInicialX + Random.Range(-raioPatrulha, raioPatrulha);
            if (destino.x > transform.position.x)
                direcao.x = 1f;
            else if (destino.x < transform.position.x)
                direcao.x = -1f;
            else
                direcao.x = 1f;
        }
    }

    protected override void Virar()
    {
        if ((transform.localScale.x > 0f && rb.velocity.x > 0.2f) ||
            (transform.localScale.x < 0f && rb.velocity.x < -0.2f))
        {
            Vector3 _localScale = transform.localScale;
            _localScale.x *= -1f;
            transform.localScale = _localScale;
            Debug.Log(rb.velocity.x);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            ColisaoPlayer(_other);
        }
    }

    /*protected virtual void OnTriggerEnter2D(Collis2D _other)
    {
        if (_other.collider.CompareTag("HoleWall") || _other.collider.CompareTag("LimWall"))
        {
            direcao *= -1f;
        }
        if (_other.collider.CompareTag("Player"))
        {
            ColisaoPlayer(_other);
        }
    }*/

    protected virtual void ColisaoPlayer(Collider2D _other)
    {
        if (_other.gameObject.GetComponent<Jogador>().morto) return;
        if (Jogador.instance.desativarHitStop) Jogador.instance.desativarHitStop = false;
        InvokeRepeating("HitsStop", 0f,0.1f);
    }

    protected virtual void ColisaoPlayer()
    {
        //if (_other.gameObject.GetComponent<Jogador>().morto) return;
        if (Jogador.instance.desativarHitStop) Jogador.instance.desativarHitStop = false;
        InvokeRepeating("HitsStop", 0f, 0.1f);
    }

    void HitsStop()
    {
        if (!Jogador.instance.desativarHitStop) Jogador.instance.ControleReceberDano(dano); 
    }

    protected override void Morrer()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        rb.simulated = false;
    }
}