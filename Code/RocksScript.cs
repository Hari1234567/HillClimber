using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocksScript : MonoBehaviour
{
    public Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        transform.localScale *= Random.Range(0.2f, 1f);
        transform.localScale += new Vector3(0, 0, 1);
    }
    private void Update()
    {
        if (CarControl.carPos.x - transform.position.x > 60)
        {
            Destroy(gameObject);
        }
    }


}
