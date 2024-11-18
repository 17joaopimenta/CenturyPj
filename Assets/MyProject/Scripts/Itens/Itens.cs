using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itens : MonoBehaviour
{
    [SerializeField] protected float valor;
    [SerializeField] protected Jogador personagem;

    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            Efeitos(outro);
        }
    }

    protected virtual void Efeitos(Collider2D outro)
    {

    }
}
