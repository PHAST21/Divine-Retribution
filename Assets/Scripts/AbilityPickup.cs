using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    private CharacterController2D player;
    private GameManager GM;
    [SerializeField]private int abilityNo;
    /*private GameObject HealthBar;
    public Vector2 HPBarWidth, HPBarPosition;*/

    private void Start()
    {
        /*HealthBar = GameObject.FindGameObjectWithTag("HealthBar");*/
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        if (GM.abilities[abilityNo] == true)
        {
            Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.abilities[abilityNo] = true;
            GM.abilities[abilityNo] = true;
            StartCoroutine(APickup());
        }
    }
    public IEnumerator APickup()
    {
        player.AbilityUnlockFunc(abilityNo);
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
}
