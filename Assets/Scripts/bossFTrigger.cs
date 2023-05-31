using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossFTrigger : MonoBehaviour
{
    public GameObject FlyBoss;
    private FlyingBoss flyboss;
    public GameObject BossFightUI;
    private void Start()
    {
        flyboss = FlyBoss.GetComponent<FlyingBoss>();
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        flyboss.bossFightStarted = true;
        BossFightUI.SetActive(true);
        Destroy(gameObject);
    }
}
