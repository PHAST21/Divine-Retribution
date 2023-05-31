using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weirdassscript : MonoBehaviour
{
    public Animator animator;
    private Color OGColor;
    private SpriteRenderer sprite;
    private CharacterController2D player;
    private Attack atk;
    // Start is called before the first frame update
    void Start()
    {
        OGColor = GetComponent<SpriteRenderer>().color;
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        atk = GameObject.FindGameObjectWithTag("Player").GetComponent<Attack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)&&player.abilities[1]&&player.canDash)
        {
            animator.SetBool("IsDashing", true);
        }
    }
    public void DoDashDamage()
    {
        atk.DoDashDamage();
    }
    public IEnumerator FlickerState(float time)
    {
        
        yield return new WaitForSeconds(time / 8);
        sprite.color = Color.white;
        yield return new WaitForSeconds(time / 8);
        sprite.color = Color.black;
        yield return new WaitForSeconds(time / 8);
        sprite.color = Color.white;
        yield return new WaitForSeconds(time / 8);
        sprite.color = Color.black;
        yield return new WaitForSeconds(time / 8);
        sprite.color = Color.white;
        yield return new WaitForSeconds(time / 8);
        sprite.color = Color.black;
        yield return new WaitForSeconds(time / 8);
        sprite.color = Color.white;
        yield return new WaitForSeconds(time / 8);
        sprite.color = OGColor;

    }
}
