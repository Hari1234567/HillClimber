using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!CarControl.gameOver)
        gameObject.transform.position = CarControl.carPos- new Vector3(0, 0, 10);
    }
}
