using UnityEngine;
using System.Collections;

public class RangedEnemy : MonoBehaviour
{
	public float MaxHP = 10;
	public float life = 10;
	private bool isPlat;
	private bool isObstacle;
	private Transform fallCheck;
	private Transform wallCheck;
	public LayerMask turnLayerMask;
	private Rigidbody2D rb;
	public GameObject player;
	public Animator enemyAnimator;
	public Transform attackCheck;
	public bool facingRight = true;
	public float dmgValue = 1;
	public GameObject throwableObject;
	public SpriteRenderer enemySprite;

	public Sprite sprite;
	public float speed = 5f;
	public float attackCooldown = 3f;
	public float attackDistance = 2f;
	public bool isInvincible = false;
	private bool isHitted = false;
	private bool canAttack = true;
	[SerializeField]private float dist;
	private bool isFlickerEnabled = false;
	private Color OGcolor;
	[SerializeField] private bool isChasing;
	[SerializeField] private float chaseDist = 6;
	/*[SerializeField] private bool isAttacking = false;*/
	[SerializeField] private float attackStun = 0.5f;
	void Awake()
	{
		life = MaxHP;
		fallCheck = transform.Find("FallCheck");
		wallCheck = transform.Find("WallCheck");
		rb = GetComponent<Rigidbody2D>();
		OGcolor = enemySprite.color;
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
		//Debug.Log(Mathf.Abs(this.transform.position.x - Player.transform.position.x));
		isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Default"));
		isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);

		if (!isHitted && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f) //movement function starts
		{
			if (isPlat /*&& !isObstacle*/ && !isHitted)
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

				if (isChasing&&dist>1f) //chaser AI
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
				else if (isChasing && dist < 1f)
				{
					rb.velocity = new Vector2(0, 0);
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
		if (canAttack == true && life>0)
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
	void AttackFlicker()
    {
		isFlickerEnabled = true;
    }

	public void ApplyDamage(float damage)
	{
		if (!isInvincible)
		{
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			if (transform.position.x < player.transform.position.x)
			{
				direction = -direction;
			}
			transform.GetComponent<Animator>().SetBool("Hit", true);
			life -= damage;
			GetComponent<DmgTxtRanged>().DmgTextEnable();
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(direction * 1000f, 100f));
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
	void DoDamage()
    {
		rb.velocity = Vector3.zero;
		GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f, -0.2f), Quaternion.identity) as GameObject;
		throwableWeapon.GetComponent<SpriteRenderer>().sprite = sprite;
		throwableWeapon.GetComponent<SpriteRenderer>().flipX = false;
		Vector2 direction = new Vector2(transform.localScale.x, 0);
		throwableWeapon.GetComponent<EnemyThrowableWeapon>().direction = direction;
		throwableWeapon.name = "EnemyThrowableWeapon";
	}
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
