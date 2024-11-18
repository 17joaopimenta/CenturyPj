using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cura : Itens
{
    protected override void Efeitos(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            Curar();
            DesabilitarItem();
        }
    }

    protected void DesabilitarItem()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    private void Curar()
    {
        personagem = GameObject.FindGameObjectWithTag("Player").GetComponent<Jogador>();
        personagem.ReceberCura((int)valor);
        personagem.recebendoCura = true;
    }
}
