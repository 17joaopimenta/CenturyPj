using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrulhador : Inimigo
{
    #region Variaveis
    [SerializeField] GameObject luz;
    #endregion

    protected override void Start()
    {
        InvokeRepeating("TratativaIA", 0f, 3f);
    }

    #region Tratamento Inimigo
    protected override void TratativaIA()
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
                direcao.x = 0f;
        }
    }

    protected override void Morrer()
    {
        base.Morrer();
        luz.SetActive(false);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, raioPatrulha);
    }
    #endregion
}