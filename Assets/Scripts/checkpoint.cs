using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    public GameObject txt;

    private void Awake()
    {
        txt.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<CharacterController2D>().SetRespawnPoint(transform.position);
            collision.GetComponent<Attack>().RefillFlask();
            txt.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            txt.SetActive(false);
        }
    }

}
