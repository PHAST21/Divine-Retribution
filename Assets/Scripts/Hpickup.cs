using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hpickup : MonoBehaviour
{
    private CharacterController2D player;
    private GameManager GM;
    private GameObject HealthBar;
    [SerializeField]private int PickupNo;
    public int hpIncAmt=5;
    public Vector2 HPBarWidth;

    private void Start()
    {
        HealthBar = GameObject.FindGameObjectWithTag("HealthBar");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        if (GM.Hupgrades[PickupNo])
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.maxHp += hpIncAmt;
            player.healthUp = true;
            HPBarWidth = HealthBar.GetComponent<RectTransform>().sizeDelta;
            /*HPBarPosition = HealthBar.GetComponent<RectTransform>().anchoredPosition;*/
            HPBarWidth.x += hpIncAmt;
            /*HPBarPosition.x += 7;*/
            HealthBar.GetComponent<RectTransform>().sizeDelta = HPBarWidth;
            /*HealthBar.GetComponent<RectTransform>().anchoredPosition = HPBarPosition;*/
            StartCoroutine(HPickup());
        }
    }
    public IEnumerator HPickup()
    {
        GM.healChange = true;
        GM.Hupgrades[PickupNo] = true;
        yield return new WaitForSeconds(0.1f);
        GM.healChange = false;
        Destroy(this.gameObject);
    }
}
