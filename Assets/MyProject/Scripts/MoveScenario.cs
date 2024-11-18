using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScenario : MonoBehaviour
{
    [SerializeField] float velocityScenario;
    [SerializeField] Transform cameraPos;
    [SerializeField] float[] posLim;

    private void Update()
    {
        var xLim = cameraPos.position.x/velocityScenario;
        var _nPos = transform.position;
        _nPos.x = xLim;
        transform.position = _nPos; 
    }
}
