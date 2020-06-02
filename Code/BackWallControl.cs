using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackWallControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (UIScript.raceMode)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CarControl.carPos.x - transform.position.x > 25 || CarControl.carPos.x - transform.position.x<0)
        {
            transform.position = new Vector3(CarControl.carPos.x - 25, transform.position.y);
        }
    }
}
