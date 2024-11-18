using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSeguidora : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    [SerializeField] private Vector3 playerPosPlus;
    [SerializeField] private bool isEnter;

    [SerializeField] private float xMin;
    [SerializeField] private float xMax;
    [SerializeField] private float yMin;
    [SerializeField] private float yMax;


    //private Collider2D collision;
    //private Collider2D collision2;

    //[SerializeField] private int seguir; // se for 1, segue o jogador, se for 0, nao segue

    private void Update()
    {
        SeguirJogador();


    }

    private void SeguirJogador()
    {
        float posX = Mathf.Clamp(playerPos.position.x, xMin, xMax);
        float posY = Mathf.Clamp(playerPos.position.y, yMin, yMax);
        Vector3 _nPos = new Vector3(posX, posY, -10f); // atualiza a pos. da camera
        transform.position = _nPos;
    }
}
