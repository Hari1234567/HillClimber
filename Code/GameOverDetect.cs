using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverDetect : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
     
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground")|| collision.gameObject.layer == LayerMask.NameToLayer("bridge"))
        {
            if (gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
            {
                BotController.botDead = true;
            }else
            CarControl.gameOver = true;
        }
    }
}
