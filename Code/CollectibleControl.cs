using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CollectibleControl : MonoBehaviour
{
    public static int diamondCount=10000;
    TextMeshProUGUI diamondMeter;
    public static int coinCount=75000;
    TextMeshProUGUI coinMeter;
    // Start is called before the first frame update
    void Start()
    {
        diamondMeter = GameObject.Find("DiamondMeter").GetComponent<TextMeshProUGUI>();
        coinMeter = GameObject.Find("CoinMeter").GetComponent<TextMeshProUGUI>();
       
        diamondMeter.SetText("X" + diamondCount);
       
        coinMeter.SetText("X" + coinCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (UIScript.raceMode)
        {
            if (BotController.botX > CarControl.carPos.x)
            {
                diamondMeter.SetText("2/2");
            }
            else
            {
                diamondMeter.SetText("1/2");
            }
            if (CarControl.carPos.x / VehicleCreate.finishX >= 0 && CarControl.carPos.x / VehicleCreate.finishX <= 1) {
                coinMeter.SetText(((int)(CarControl.carPos.x * 100 / VehicleCreate.finishX)).ToString()+"%");
            } 
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Diamond")
        {
            GetComponents<AudioSource>()[2].Play();
            collision.gameObject.GetComponent<Animator>().SetTrigger("Exit");
            collision.enabled = false;
            diamondCount++;
            diamondMeter.SetText("X" + diamondCount);
            Destroy(collision.gameObject, 1);
        }
        if (collision.tag == "Coin")
        {
            GetComponents<AudioSource>()[2].Play();

            coinCount += collision.gameObject.GetComponent<CoinControl>().getCoinValue();
            collision.enabled = false;
            coinMeter.SetText("X" + coinCount);
            collision.GetComponent<Animator>().SetTrigger("Exit");
            Destroy(collision.gameObject, 1);
            
        }
        if(collision.tag == "gasCan")
        {
            GetComponents<AudioSource>()[3].Play();
            CarControl.fuelAmt = 100;
            collision.enabled = false;
            collision.GetComponent<Animator>().SetTrigger("Exit");
            Destroy(collision.gameObject, 1);

        }

        if (collision.gameObject.tag == "fallDetector")
        {
            Debug.Log("ADSAD");
            CarControl.gameOver = true;
        }


    }
}
