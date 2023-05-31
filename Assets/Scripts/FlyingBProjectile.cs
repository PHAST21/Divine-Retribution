using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBProjectile : MonoBehaviour
{
	public Vector2 direction;
	public bool hasHit = false;
	public float speed = 10f;
	private CharacterController2D characterController2D;
	private FlyingBoss bossEnemy;
	public float projectileDmg;
	private Rigidbody2D rb;
	public float rotateSpeed;
/*	private bool rightProjectile;*/
	[SerializeField] protected Transform target;
	private CircleCollider2D col;

	// Start is called before the first frame update
	private void Awake()
	{
		characterController2D = FindObjectOfType<CharacterController2D>();
		bossEnemy = FindObjectOfType<FlyingBoss>();
	}
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		/*if (bossEnemy.facingRight)
		{
			*//*rightProjectile = true;*//*
		}*/
		col = GetComponent<CircleCollider2D>();
		col.enabled = true;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		target = characterController2D.getPosition();
		Vector2 direction = (Vector2)target.position - rb.position;
		direction.Normalize();
		
		
			float rotateAmount = Vector3.Cross(direction, transform.up).z;
			rb.angularVelocity = -rotateSpeed * rotateAmount;
			rb.velocity = transform.up * speed;
		
		
	}

	void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.tag == "Player")
		{
			/*col.enabled = true;*/
			/*rb.isKinematic = false;*/
			characterController2D.AppyDamageProjectile(projectileDmg) /*transform.position*/;
			Destroy(gameObject);
		}
		else if (collision.gameObject.tag == "Enemy")
		{
			/*StartCoroutine(CollisionDisable());*/

		}
		else
		{
			/*col.enabled = true;*/
			/*rb.isKinematic = false;*/
			Destroy(gameObject);
		}
	}

	IEnumerator CollisionDisable()
	{
		col.enabled = false; ;
		yield return new WaitForSeconds(0.2f);
		col.enabled = true;
	}
}
