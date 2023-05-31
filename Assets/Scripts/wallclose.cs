using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallclose : MonoBehaviour
{
    public GameObject trigger;
    // Start is called before the first frame update

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            trigger.SetActive(false);
            Destroy(gameObject);
        }
    }


}
