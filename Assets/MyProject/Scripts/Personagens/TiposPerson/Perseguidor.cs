using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Perseguidor : Inimigo
{
    [SerializeField] private float distParar, distPerseguir;
    private Transform alvo;
    private float distX,distY;
    [SerializeField] private GameObject explosion;
    private GameObject[] collWall;
    private bool podeExplodir, perseguindo;
    private float velocidadePerseguir, velocidadeNormal;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        alvo = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        explosion.SetActive(false);
        velocidadeNormal = velocidade; 
        velocidadePerseguir = velocidade * 1.7f;
    }

    protected override void Update()
    {
        base.Update();
        Debug.Log("Colidiu : " + iscol.isCollided);
        SeguirJogador();
        Debug.Log("Velocidade Perseguidor : " + rb.velocity.x);
    }

    protected void SeguirJogador()
    {
        distX = alvo.position.x - transform.position.x;
        distY = alvo.position.y - transform.position.y;
        if (Mathf.Abs(distX) > distPerseguir)
        { 
            TratativaIA();
            if (direcao.x == 0) direcao.x = 1f;
            anim.SetBool("IsFollow", false);
            velocidade = velocidadeNormal;
            perseguindo = false;
            if (sfx[(int)AudioTipos.Pavil].isPlaying) sfx[(int)AudioTipos.Pavil].Pause();
            /*if (sfx[(int)AudioTipos.Pavil].isPlaying) sfx[(int)AudioTipos.Pavil].Pause();*/
        }

        else
        {
            anim.SetBool("IsFollow", true);
            perseguindo = true;
            if (!sfx[(int)AudioTipos.Pavil].isPlaying)
            {
                sfx[(int)AudioTipos.Pavil].Play();
            }

            if (Mathf.Abs(distX) > distParar || Mathf.Abs(distY) > distParar)
            {
                Perseguir();
                velocidade = velocidadePerseguir;
            }

            else if (Mathf.Abs(distX) <= distParar)
            { 
                Morrer(); 
            } 
            
        }
    }

    protected override void OnTriggerEnter2D(Collider2D _other)
    {
        base.OnTriggerEnter2D(_other);
        if (_other.CompareTag("Shoot"))
        {
            Morrer();
        }
    }

    protected override void Morrer()
    {
        base.Morrer();
        if (sfx[(int)AudioTipos.Pavil].isPlaying) sfx[(int)AudioTipos.Pavil].Pause();
        StartCoroutine(Kaboom());
    }

    IEnumerator Kaboom()
    {
        explosion.SetActive(true);
        if (Mathf.Abs(distX) <= distParar && Mathf.Abs(distY) <= distParar) ColisaoPlayer();
        Debug.Log("BOOOM!");
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
        Debug.Log("Sem BOOOM :(");
    }

    void Perseguir()
    {
        if (distX > 0.5f)
        {
            direcao.x = 1f;
        }
        else if (distX < -0.5f)
        {
            direcao.x = -1f;
        }
        else
        {
            direcao.x = 0f;
        }
        Debug.Log("Direcao : "+direcao.x);
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,distPerseguir);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,distParar);
    }
}