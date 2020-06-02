using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishScript : MonoBehaviour
{
    public static int win = 0;
    private void Start()
    {
        win = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            win = -1;
            CarControl.gameOver = true;
            BotController.botDead = true;
            BotController.won = true;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            win = 1;
            CarControl.gameOver = true;
            BotController.botDead = true;
            CarControl.won = true;
        }
    }
}
