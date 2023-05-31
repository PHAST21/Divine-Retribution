using UnityEngine;
using System.Collections;

public class TrainingDummy : MeleeEnemy
{

	public float life = 100;
	public float MaxHP = 100;
	private bool isPlat;
	private Transform fallCheck;
	private Transform wallCheck;
	public LayerMask turnLayerMask;
	private Rigidbody2D rb;
	public GameObject player;
	public Animator enemyAnimator;
	public SpriteRenderer enemySprite;
	public Transform attackCheck;
	public bool facingRight = true;
	public float dmgValue = 1;
	public GameObject DmgTxt;


	public float speed = 0f;
	public float attackCooldown = 3f;
	public float attackDistance = 2f;
	public bool isInvincible = false;
	private bool isHitted = false;
	[SerializeField] private bool canAttack = false;
	[SerializeField] private float dist;
/*	private Color OGcolor;
	private bool isFlickerEnabled = false;*/
	[SerializeField] private bool stopForAttack = false;
	[SerializeField] private bool isChasing;
	[SerializeField] private float chaseDist = 6;
	[SerializeField] private float attackStun = 0.5f;
	[SerializeField] private bool isAlreadyHit = false;
	void Awake()
	{
		fallCheck = transform.Find("FallCheck");
		wallCheck = transform.Find("WallCheck");
		rb = GetComponent<Rigidbody2D>();
		/*OGcolor = enemySprite.color;*/
		life = MaxHP;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		dist = Vector2.Distance(transform.position, player.transform.position); //calculates distance b/w player and enemy
		if (life <= 0)
		{
			transform.GetComponent<Animator>().SetBool("IsDead", true);
			StartCoroutine(DestroyEnemy());
		}
		//Debug.Log(Mathf.Abs(this.transform.position.x - Player.transform.position.x));
		isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Default"));
/*		isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);*/

		if (!isHitted && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f) //movement function starts
		{
			if (isPlat /*&& !isObstacle */&& !isHitted)
			{
				if (dist < chaseDist)
				{
					isChasing = true;
					enemyAnimator.SetBool("isChasing", true);
				}
				else if (dist > chaseDist)
				{
					rb.velocity = Vector3.zero;
					isChasing = false;
					enemyAnimator.SetBool("isChasing", false);
				}

				if (isChasing && dist > 1f && !stopForAttack) //chaser AI
				{

					if (transform.position.x > player.transform.position.x)
					{
                        /*transform.localScale = new Vector3(-1, 1, 1);*/
                        rb.velocity = new Vector2(-speed, rb.velocity.y);
						facingRight = true;
					}
					if (transform.position.x < player.transform.position.x)
					{
                        /*transform.localScale = new Vector3(1, 1, 1);*/
                        facingRight = false;
						rb.velocity = new Vector2(speed, rb.velocity.y);
					}
				}
				else if ((isChasing && dist < 1f) || stopForAttack)
				{
					rb.velocity = Vector2.zero;
				}

				/*if (facingRight)
				{
					rb.velocity = new Vector2(-speed, rb.velocity.y);
				}
				else
				{
					rb.velocity = new Vector2(speed, rb.velocity.y);
				}*/

			}
			else
			{
				Flip();
			}

		}
		if (canAttack == true && life > 0)
		{
			if (Mathf.Abs(dist) < attackDistance)
			{
				stopForAttack = true;
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
	/*void DoDamage()
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
				if (collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().isParrying)
				{
					isHitted = true;
					rb.velocity = Vector3.zero;
					enemyAnimator.SetBool("preAttack", false);
					*//*enemyAnimator.SetBool("IsAttacking", false);*//*
					enemyAnimator.SetBool("isParried", true);
                    StartCoroutine(Parried());
				}
				else
				{
					enemyAnimator.SetBool("IsAttacking", true);
					collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().ApplyDamage(dmgValue, transform.position);
				}
			}
		}
	}*/
	/*void AttackFlicker()
    {
		isFlickerEnabled = true;
    }*/
	public void ApplyDamage(float damage)
	{
		if (!isInvincible)
		{
			/*dmgtaken += damage;*/
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
            if (!isAlreadyHit)
            {
				if (facingRight)
					GetComponent<Animator>().SetBool("Hit", true);
				else
					GetComponent<Animator>().SetBool("HitLeft", true);
				isAlreadyHit = true;
			}
			life -= damage;
			GetComponent<DmgTxtDummy>().dmgTextEnable();
			rb.velocity = Vector2.zero;
            /*rb.AddForce(new Vector2(direction * 1000f, 100f));*/
            StartCoroutine(HitTime());
		}
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
		yield return new WaitForSeconds(0.4f);
		isInvincible = true;
		yield return new WaitForSeconds(0.6f);
		isAlreadyHit = false;
		life = MaxHP;
		isHitted = false;
		isInvincible = false;
	}
	/*	IEnumerator Parried()
		{
			yield return new WaitForSeconds(0.75f);
			rb.velocity = Vector3.zero;
			enemyAnimator.SetBool("isParried", false);
			isHitted = false;
		}*/
	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(0.25f);
		enemyAnimator.SetBool("IsAttacking", false);
		yield return new WaitForSeconds(attackCooldown);
		canAttack = true;

	}
	IEnumerator AttackStunState()
	{

		/*isFlickerEnabled = true;*/
		/*StartCoroutine(FlickerState());*/
		yield return new WaitForSeconds(attackStun);
		enemyAnimator.SetBool("preAttack", false);
		enemyAnimator.SetBool("IsAttacking", true);
		DoDamage(attackCheck, life, isHitted, rb, enemyAnimator, dmgValue);
		stopForAttack = false;


		StartCoroutine(AttackCooldown());
	}
	/*IEnumerator FlickerState()
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
			*//*for(int i = 0; i > 8; i++)
            {
				Debug.Log(i);
                if (enemySprite.color == OGcolor||enemySprite.color==Color.white)
                {
					enemySprite.color = Color.clear;
					yield return new WaitForSeconds(attackStun / 8);
                }
				else if (enemySprite.color == Color.clear)
                {
					enemySprite.color = Color.white;
					yield return new WaitForSeconds(attackStun / 8);

				}
            }*//*
			isFlickerEnabled = false;
			enemySprite.color = OGcolor;
		}
	}*/
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
