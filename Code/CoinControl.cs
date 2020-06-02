using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinControl : MonoBehaviour
{
    public Sprite[] coins;
    int coinValue;
    // Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(0, coins.Length);
        GetComponent<SpriteRenderer>().sprite = coins[index];
        if (index == 0)
        {
            coinValue = 1;
        }
        if (index == 1)
        {
            coinValue = 10;
        }
        if (index == 2)
        {
            coinValue = 50;
        }
        if (index == 3)
        {
            coinValue = 100;
        }
    }
    public int getCoinValue()
    {
        return coinValue;
    }

  
}
