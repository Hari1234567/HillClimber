using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightMode : MonoBehaviour
{
 
    void Start()
    {
        gameObject.SetActive(UIScript.nightMode);
    }

}
