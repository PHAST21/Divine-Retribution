using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughProjectile : MonoBehaviour
{
	public Vector2 direction;
	public bool hasHit = false;
	public float speed = 10f;
	/*public GameObject player;*/
	public float projectileDmg = 1f;
	/*private CharacterController2D characterController;*/
	public bool passingthru=false;
	private CircleCollider2D col;
	// Start is called before the first frame update
	void Start()
	{
		/*		characterController = player.GetComponent<CharacterController2D>();
		*/
		col = GetComponent<CircleCollider2D>();
		col.enabled = true;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		/*if (!hasHit)*/
			GetComponent<Rigidbody2D>().velocity = direction * speed;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.tag == "Player")
		{
			collision.gameObject.SendMessage("AppyDamageProjectile", projectileDmg);
			Destroy(gameObject);
		}
		if (collision.gameObject.tag == "PassThruProjectile")
		{
			passingthru = true;
			StartCoroutine(CollisionDisable());
		}
		else
		{
			/*Destroy(gameObject);*/
		}
		if (collision.gameObject.tag == "Enemy")
        {
			/*StartCoroutine(CollisionDisable());*/
        }
	}
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PassThruProjectile")
        {
			passingthru = false;
        }
    }
    IEnumerator CollisionDisable()
	{
		col.enabled = false;
		yield return new WaitForSeconds(0.3f);
		col.enabled = true;
	}
}
