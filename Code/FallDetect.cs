using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetect : MonoBehaviour
{

    void Update()
    {
        transform.position = new Vector3(CarControl.carPos.x, -20);
    }
  
}
