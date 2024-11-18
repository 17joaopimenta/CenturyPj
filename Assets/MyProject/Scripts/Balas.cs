using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Balas : MonoBehaviour
{
    public static Balas instance;

    [SerializeField] private float velocidadeTiro;
    private Vector2 direction;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform pegarPosicao;
    public bool deslColl, isCollided;
    [SerializeField] private int danoBala;

    private void Awake()
    {
        instance = this;
        pegarPosicao = GameObject.FindGameObjectWithTag("PosTiro").GetComponent<Transform>();
        DefinirEstInicial();
    }

    private void FixedUpdate()
    {
        rb.velocity = direction;
    }

    public void MoverBala(Vector2 dir)
    {
        if (!rb.simulated) rb.simulated = true;
        GetComponent<Collider2D>().enabled = !deslColl;
        transform.localScale = new Vector3(dir.x,1f);
        DefinirPosInicial();
        dir.x *= velocidadeTiro;
        direction = dir;
        InvokeRepeating("Voltar", 0f, 0.1f);
    }

    /*private void MudarEscala()
    {
        if ((transform.localScale.x > 0f && rb.velocity.x < -0.2f) ||
            (transform.localScale.x < 0f && rb.velocity.x > 0.2f))
        {
            Vector3 _localScale = transform.localScale;
            _localScale.x *= -1f;
            transform.localScale = _localScale;
        }
    }*/

    public void Voltar()
    {
        if (Vector2.Distance(pegarPosicao.position,transform.position) > 10)
        {
            rb.simulated = false;
            DefinirEstInicial();
        }
    }

    public void DefinirPosInicial()
    {
        if (pegarPosicao == null)
            pegarPosicao = GameObject.FindGameObjectWithTag("PosTiro").GetComponent<Transform>();
        transform.position = pegarPosicao.position;
    }

    public void DefinirEstInicial()
    {
        if (GetComponent<Collider2D>().isActiveAndEnabled) GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        deslColl = true;
    }

    private void OnTriggerEnter2D(Collider2D _outro)
    {
        if (_outro.CompareTag("Enemy"))
        {
            MatarInimigo(_outro);
            GameManager.instance.SomarQuantInimigo(1);
        }
        else if (_outro.CompareTag("TrainingEnemy"))
        {
            MatarTreino(_outro);
            Tutorial.instance.TrocarTexto(_outro);
        }
    }

    void MatarInimigo(Collider2D _outro)
    {
        _outro.gameObject.GetComponent<Inimigo>().ReceberDano(danoBala);
        DefinirEstInicial();
        MoverBala(new Vector2(0f, 0f));
        Debug.Log("Tomei Tiro, Dano : " + danoBala);
    }

    void MatarTreino(Collider2D _outro)
    {
        isCollided = true;
        _outro.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        DefinirEstInicial();
        MoverBala(new Vector2(0f, 0f));
        Debug.Log("Tomei Tiro, Dano : " + danoBala);
    }
}
