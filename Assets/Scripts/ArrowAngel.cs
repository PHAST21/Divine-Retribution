using UnityEngine;
using System.Collections;

public class ArrowAngel : MonoBehaviour
{

	public float life = 10;
	public float MaxHP = 10;
	/*private bool isPlat;
	private bool isObstacle;*/
	/*private Transform fallCheck;*/
	private Transform wallCheck;
	public LayerMask turnLayerMask;
	private Rigidbody2D rb;
	public GameObject player;
	public Animator enemyAnimator;
	public Transform attackCheck;
	public SpriteRenderer enemySprite;
	public bool facingRight = true;
	public float dmgValue = 1;
	public GameObject throwableObject;
	public Transform projectileSpawn;

	public float speed = 5f;
	public float attackCooldown = 3f;
	public float attackDistance = 2f;
	public bool isInvincible = false;
	private bool isHitted = false;
	private bool canAttack = true;
	[SerializeField] private bool isChasing;
	private float dist;
	private Color OGcolor;
	private bool isFlickerEnabled = false;
	[SerializeField] private float chaseDist = 6;
	/*[SerializeField] private bool isAttacking = false;*/
	[SerializeField] private float attackStun = 0.5f;
	void Awake()
	{
		/*fallCheck = transform.Find("FallCheck");
		wallCheck = transform.Find("WallCheck");*/
		rb = GetComponent<Rigidbody2D>();
		OGcolor = enemySprite.color;
		life = MaxHP;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		dist = Vector2.Distance(transform.position, player.transform.position); //calculates distance b/w player and enemy
		if (life <= 0)
		{
			life = 0;
			transform.GetComponent<Animator>().SetBool("IsDead", true);
			StartCoroutine(DestroyEnemy());
		}
		if (!isHitted && life > 0)
		{
			if (dist < chaseDist && dist > 1)
			{
				isChasing = true;
			}
			else
			{
				isChasing = false;
			}
			if (transform.position.x > player.transform.position.x)
			{
				transform.localScale = new Vector3(-1, 1, 1);
				facingRight = true;
			}
			else
			{
				transform.localScale = new Vector3(1, 1, 1);
				facingRight = false;
			}
			if (isChasing)
			{
				Vector2 targetPos = new Vector2(player.transform.position.x, player.transform.position.y+3);
				enemyAnimator.SetBool("isChasing", true);
				transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
			}
			else
			{
				enemyAnimator.SetBool("isChasing", false);
			}
		}
		/*//Debug.Log(Mathf.Abs(this.transform.position.x - Player.transform.position.x));
		isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Default"));
		isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);

		if (!isHitted && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f) //movement function starts
		{
			if (isPlat && !isObstacle && !isHitted)
			{
				if (isChasing) //chaser AI
				{
					if (transform.position.x > player.transform.position.x)
					{
						transform.localScale = new Vector3(-1, 1, 1);
						rb.velocity = new Vector2(-speed, rb.velocity.y);
						facingRight = true;
					}
					if (transform.position.x < player.transform.position.x)
					{
						transform.localScale = new Vector3(1, 1, 1);
						facingRight = false;
						rb.velocity = new Vector2(speed, rb.velocity.y);
					}
				}
				else
				{
					if (dist < chaseDist)
					{
						isChasing = true;
					}
					*//*if (facingRight)
					{
						rb.velocity = new Vector2(-speed, rb.velocity.y);
					}
					else
					{
						rb.velocity = new Vector2(speed, rb.velocity.y);
					}*//*
				}
			}
			else
			{
				Flip();
			}

		}*/
		if (canAttack == true)
		{
			if (Mathf.Abs(dist) < attackDistance)
			{
				canAttack = false;
				enemyAnimator.SetBool("preAttack", true);
				StartCoroutine(AttackStunState());

			}
		}
	}
	void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void ApplyDamage(float damage)
	{
		if (!isInvincible)
		{
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			transform.GetComponent<Animator>().SetBool("Hit", true);
			life -= damage;
			GetComponent<ArrowAngelDmgTxt>().DmgTextEnable();
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(direction * 500f, 100f));
			StartCoroutine(HitTime());
		}
	}
	void AttackFlicker()
	{
		isFlickerEnabled = true;
	}
	public void flydead()
	{
		if (life < 1)
		{
			rb.bodyType = RigidbodyType2D.Dynamic;
		}
	}
	/*	void DoDamage()
		{
			Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
			for (int i = 0; i < collidersEnemies.Length; i++)
			{
				if (collidersEnemies[i].gameObject.tag == "Player" && life > 0)
				{
					if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
					{
						*//*dmgValue = -dmgValue;*//*
					}
					collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().ApplyDamage(dmgValue, transform.position);
				}
			}
		}*/

	//Ranged Projectile
	void DoDamage()
	{
		GameObject throwableWeapon = Instantiate(throwableObject, projectileSpawn.position, Quaternion.identity) as GameObject;
		Vector2 direction = new Vector2(transform.localScale.x, 0);
		throwableObject.GetComponent<arrow>().speed = 10f;
        throwableWeapon.GetComponent<arrow>().projectileDmg = dmgValue;
        throwableWeapon.name = "EnemyThrowableWeapon";
	}

	//collision damage
	/*void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(2f, transform.position);
		}
	}*/

	IEnumerator HitTime()
	{
		isHitted = true;
		isInvincible = true;
		yield return new WaitForSeconds(0.1f);
		isHitted = false;
		isInvincible = false;
	}
	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(0.25f);
		enemyAnimator.SetBool("isAttacking", false);
		yield return new WaitForSeconds(attackCooldown);
		canAttack = true;
	}
	IEnumerator AttackStunState()
	{
		/*isAttacking = true;*/
		AttackFlicker();
		StartCoroutine(FlickerState());
		yield return new WaitForSeconds(attackStun);
		enemyAnimator.SetBool("preAttack", false);
		enemyAnimator.SetBool("isAttacking", true);
		DoDamage();

		/*isAttacking = false;*/

		StartCoroutine(AttackCooldown());
	}
	IEnumerator FlickerState()
	{
		while (isFlickerEnabled == true && life > 0)
		{
			enemySprite.color = Color.clear;
			yield return new WaitForSeconds(attackStun / 8);
			enemySprite.color = Color.white;
			yield return new WaitForSeconds(attackStun / 8);
			enemySprite.color = Color.clear;
			yield return new WaitForSeconds(attackStun / 8);
			enemySprite.color = Color.white;
			yield return new WaitForSeconds(attackStun / 8);
			enemySprite.color = Color.clear;
			yield return new WaitForSeconds(attackStun / 8);
			enemySprite.color = Color.white;
			yield return new WaitForSeconds(attackStun / 8);
			enemySprite.color = Color.clear;
			yield return new WaitForSeconds(attackStun / 8);
			enemySprite.color = Color.white;
			yield return new WaitForSeconds(attackStun / 8);
			isFlickerEnabled = false;
			enemySprite.color = OGcolor;
		}
	}
	IEnumerator DestroyEnemy()
	{
		CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
		capsule.size = new Vector2(1f, 0.25f);
		capsule.offset = new Vector2(0f, -0.8f);
		capsule.direction = CapsuleDirection2D.Horizontal;
		yield return new WaitForSeconds(0.25f);
		rb.velocity = new Vector2(0, rb.velocity.y);
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}
}
