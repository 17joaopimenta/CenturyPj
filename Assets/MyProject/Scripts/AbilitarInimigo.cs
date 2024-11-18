using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitarInimigo : MonoBehaviour
{
    [Header("Ativador")]
    [SerializeField] Vector3 boxSize;
    [SerializeField] Transform boxPos;
    [SerializeField] float boxAngle;
    [SerializeField] LayerMask boxLayer;

    [Header("Inimigos")]
    [SerializeField] GameObject[] inimigos;

    
    private void Update()
    {
        GerarColisor();
    }

    private void GerarColisor()
    {
        var test = Physics2D.OverlapBox(boxPos.position, boxSize, boxAngle, boxLayer);
        if (test) GerarInimigos();
    }

    void GerarInimigos()
    {
        foreach (var ini in inimigos)
        {
            if (ini == null) return;
            ini.SetActive(true);
        }
        Debug.Log("Colidiu");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxPos.position, boxSize);
    }
}
