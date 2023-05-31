using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrowableWeapon : MonoBehaviour
{
	public Vector2 direction;
	private Vector3 vector3;
	public bool hasHit = false;
	public float speed = 10f;
	/*public GameObject player;*/
	public float projectileDmg;
	/*private CharacterController2D characterController;*/
	private CircleCollider2D col;

	// Start is called before the first frame update
	void Start()
	{
		/*		characterController = player.GetComponent<CharacterController2D>();*/
		col = GetComponent<CircleCollider2D>();
		col.enabled = true;
		vector3 = gameObject.transform.localScale;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!hasHit)
			GetComponent<Rigidbody2D>().velocity = direction * speed;
        if (direction.x < 0)
        {
			gameObject.transform.localScale = new Vector3(-vector3.x,vector3.y,vector3.z);
        }
	}

	void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.tag == "Player")
		{
			collision.gameObject.SendMessage("AppyDamageProjectile", projectileDmg);
			Destroy(gameObject);
		}
		else if (collision.gameObject.tag != "Enemy")
		{
			Destroy(gameObject);
		}
		else if(collision.gameObject.tag == "Enemy")
        {
			/*StartCoroutine(CollisionDisable());*/
        }
	}

	IEnumerator CollisionDisable()
	{
		col.enabled = false; ;
		yield return new WaitForSeconds(0.2f);
		col.enabled = true;
	}
}
