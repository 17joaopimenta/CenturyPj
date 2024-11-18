using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Final : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            GameManager.instance.TelaFinal(_other);
            GameManager.instance.ChegouNoFinal(_other);
            Debug.Log("Adeus");
        }
    }
}
