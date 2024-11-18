using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCol : MonoBehaviour
{
    public bool isCollided;

    private void OnCollisionEnter2D(Collision2D _other)
    {
        if (_other.collider.CompareTag("HoleWall") || _other.collider.CompareTag("LimWall") || _other.collider.CompareTag("EnemyCol"))
        {
            isCollided = !isCollided;
        }
    }
}
