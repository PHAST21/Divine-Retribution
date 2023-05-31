using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bosswall : MonoBehaviour
{
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        if (gm.boss1Defeated)
        {
            gameObject.SetActive(false);
        }
    }
}
