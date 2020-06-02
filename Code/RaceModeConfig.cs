using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceModeConfig : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(!UIScript.raceMode);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
