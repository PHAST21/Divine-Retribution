using UnityEngine;
using System.Collections;

public class BossEnemy : MonoBehaviour
{
	public float MaxHP = 80;
	public float life = 10;
	private bool isPlat;
	private bool isObstacle;
	private Transform fallCheck;
	private Transform wallCheck;
	public LayerMask turnLayerMask;
	private Rigidbody2D rb;
	public GameObject player;
	public Animator enemyAnimator;
	public SpriteRenderer enemySprite;
	public Transform attackCheck;
	public bool facingRight = true; //actually is for facingLeft but cba to rename it
	public float dmgValue = 1;
	public GameObject throwableObject;
	private int timesHit = 0;
	[SerializeField] private float hitTime = 0.1f;
	[SerializeField] private float bigHitTime = 3f;
	[SerializeField] private int i;
	[SerializeField] private bool stopForAttack=false;
	public bool debugforAtk = false;
	private float tempAtk;

	public GameManager gm;
	public Material mat;
	private Material OGmat;
	public float speed = 5f;
	public float attackCooldown = 3f;
	public float attackDistance = 2f;
	public float projectileattackDistance = 5f;
	public bool isInvincible = false;
	private bool isHitted = false;
	private bool canAttack = true;
	[SerializeField]private float dist;
	/*private Color OGcolor;
	private bool isFlickerEnabled = false;*/

	[SerializeField] private bool isChasing;
	[SerializeField] private float chaseDist = 6;
	[SerializeField] private float attackStun = 0.5f;
	public GameObject walltrigger;
	public int damageTakenUI = 0;
	void Awake()
	{
		fallCheck = transform.Find("FallCheck");
		wallCheck = transform.Find("WallCheck");
		rb = GetComponent<Rigidbody2D>();
		/*OGcolor = enemySprite.color;*/
		OGmat = enemySprite.material;
		life = MaxHP;
		tempAtk = dmgValue;
		gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        if (gm.boss1Defeated)
        {
			walltrigger.SetActive(false);
			Destroy(gameObject);
        }
		rb.velocity = Vector2.zero;
	}


    void FixedUpdate()
	{
		dist = Vector2.Distance(transform.position, player.transform.position); //calculates distance b/w player and enemy
		if (life <= 0)
		{
			transform.GetComponent<Animator>().SetBool("IsDead", true);
			StartCoroutine(DestroyEnemy());
			walltrigger.SetActive(false);
			
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

                if (isChasing && dist > 1.5f&&!stopForAttack) //chaser AI
                {
                    if (transform.position.x > player.transform.position.x)
                    {
                        transform.localScale = new Vector3(-2, 2, 2);
                        rb.velocity = new Vector2(-speed, rb.velocity.y);
                        facingRight = true;
                    }
                    if (transform.position.x < player.transform.position.x)
                    {
                        transform.localScale = new Vector3(2, 2, 2);
                        facingRight = false;
                        rb.velocity = new Vector2(speed, rb.velocity.y);
                    }
                }
                else if ((isChasing && dist < 1.5f)||stopForAttack)
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
		if (canAttack == true && life > 0)
		{
			if (Mathf.Abs(dist) < attackDistance)
			{
				if (!debugforAtk)
				{
					i = Random.Range(0, 3);
				}
                canAttack = false;
				switch (i)
				{
					case 0:
						enemyAnimator.SetBool("PokePreAttack", true);
						break;
					case 1:
						enemyAnimator.SetBool("SlashPreAttack", true);
						break;
					case 2:
						enemyAnimator.SetBool("ProjectilePreAttack", true);
						break;
				}
                
				StartCoroutine(AttackStunState());

			}
		}
	}
	void Flip()
	{
		// Switch the way the player is labelled as facing.
		/*facingRight = !facingRight;*/

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	void DoDamage()
	{
		switch (i) {
			case 0:
				Debug.Log("Poke");
				Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 2f);
				for (int i = 0; i < collidersEnemies.Length; i++)
				{
					if (collidersEnemies[i].gameObject.tag == "Player" && life > 0)
					{
						if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
						{
							/*dmgValue = -dmgValue;*/

						}
						/*enemyAnimator.SetBool("PokeAttack", true);
						StartCoroutine(AnimCooldown());*/
						if (collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().isParrying)
						{
							isHitted = true;
							rb.velocity = Vector3.zero;
							enemyAnimator.SetBool("PokePreAttack", false);
							enemyAnimator.SetBool("PokeAttack", false);
							enemyAnimator.SetBool("isParried", true);
							GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().hasParried = true;
							StartCoroutine(Parried());
						}
						else
						{
							enemyAnimator.SetBool("PokeAttack", true);
							collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().ApplyDamage(dmgValue, transform.position);
						}
					}
				}
				break;
			case 1:
				Debug.Log("Slash");
				Collider2D[] collidersEnemies1 = Physics2D.OverlapCircleAll(attackCheck.position, 2f);
				dmgValue = tempAtk + 2;
				for (int i = 0; i < collidersEnemies1.Length; i++)
				{
					if (collidersEnemies1[i].gameObject.tag == "Player" && life > 0)
					{
						if (collidersEnemies1[i].transform.position.x - transform.position.x < 0)
						{
							/*dmgValue = -dmgValue;*/
						}
						/*enemyAnimator.SetBool("SlashAttack", true);
						StartCoroutine(AnimCooldown());*/
						if (collidersEnemies1[i].gameObject.GetComponent<CharacterController2D>().isParrying)
						{
							isHitted = true;
							rb.velocity = Vector3.zero;
							enemyAnimator.SetBool("SlashPreAttack", false);
							enemyAnimator.SetBool("SlashAttack", false);
							enemyAnimator.SetBool("isParried", true);
							GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().hasParried = true;
							StartCoroutine(Parried());
						}
						else
						{
							enemyAnimator.SetBool("Cooldown", true);
							enemyAnimator.SetBool("SlashAttack", true);
							collidersEnemies1[i].gameObject.GetComponent<CharacterController2D>().ApplyDamage(dmgValue, transform.position);
						}
					}
				}
				break;

			case 2:
				Debug.Log("Shooting");
				enemyAnimator.SetBool("ProjectilePreAttack", false);
				StartCoroutine(MultiShoot());
				break;
		}
					
        /*if (i == 0)
        {
            Debug.Log("Poke");
            Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 2f);
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
                        enemyAnimator.SetBool("PokePreAttack", false);
                        enemyAnimator.SetBool("PokeAttack", false);
                        enemyAnimator.SetBool("isParried", true);
						GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().hasParried = true;
						StartCoroutine(Parried());
                    }
                    else
                    {
                        enemyAnimator.SetBool("PokeAttack", true);
                        collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().ApplyDamage(dmgValue, transform.position);
                    }
                }
            }
        }
		else if (i == 1) { 
			Debug.Log("Slash");
			Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 2f);
			dmgValue = tempAtk + 3;
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
					enemyAnimator.SetBool("SlashPreAttack", false);
					enemyAnimator.SetBool("SlashAttack", false);
					enemyAnimator.SetBool("isParried", true);
					GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().hasParried = true;
					StartCoroutine(Parried());
					}
					else
					{
					enemyAnimator.SetBool("SlashAttack", true);
					collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().ApplyDamage(dmgValue, transform.position);
					}
				}
			}
		}
        else
        {
			Debug.Log("Shooting");
			enemyAnimator.SetBool("ProjectilePreAttack", false);
			
			StartCoroutine(MultiShoot());
		}*/
	}
/*	void AttackFlicker()
	{
		isFlickerEnabled = true;
	}*/
	public void ApplyDamage(float damage)
	{
		if (!isInvincible)
		{
			damageTakenUI = (int)damage;
			timesHit++;
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			if (transform.position.x < player.transform.position.x)
			{
				direction = -direction;
			}
			transform.GetComponent<Animator>().SetBool("Hit", true);
			life -= damage;
			GetComponent<DmgTxtBoss>().dmgTextEnable();
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(direction * 1000f, 100f));
			if (timesHit > 3) {
				StartCoroutine(BigHitTime());
				timesHit = 0;
			}
			else {
				StartCoroutine(HitTime());
			}
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
		isInvincible = true;
		yield return new WaitForSeconds(hitTime);
		isHitted = false;
		isInvincible = false;
	}
	IEnumerator BigHitTime()
	{
		isHitted = true;
		isInvincible = true;
		enemySprite.material = mat;
		yield return new WaitForSeconds(bigHitTime);
		enemySprite.material = OGmat;
		isHitted = false;
		isInvincible = false;
	}
	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(0.25f);
		switch (i)
		{
			case 0:
				enemyAnimator.SetBool("PokeAttack", false);
				break;
			case 1:
				dmgValue = tempAtk;
				enemyAnimator.SetBool("SlashAttack", false);
				
				break;
			case 2:
				break;
		}
		yield return new WaitForSeconds(attackCooldown);
        /*enemyAnimator.SetBool("Cooldown", false);*/
        stopForAttack = false;
		canAttack = true;
	}
	IEnumerator Parried()
	{
		
		yield return new WaitForSeconds(1.25f);
		GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().hasParried = false;
		rb.velocity = Vector3.zero;
		enemyAnimator.SetBool("isParried", false);
		isHitted = false;
	}
	IEnumerator MultiShoot()
    {
		enemyAnimator.SetBool("ProjectileAttack", true);
		stopForAttack = true;
		for (int i = 0; i < 3; i++)
		{
			GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f, -0.2f), Quaternion.identity) as GameObject;
			Vector2 direction = new Vector2(transform.localScale.x, 0);	
			throwableObject.GetComponent<HomingEnemyThrowableWeapon>().speed = 10f;
			throwableWeapon.GetComponent<HomingEnemyThrowableWeapon>().projectileDmg = 2f;
			throwableWeapon.name = "EnemyThrowableWeapon";
			yield return new WaitForSeconds(0.5f);
		}
		stopForAttack = false;
		enemyAnimator.SetBool("ProjectileAttack", false);
	}

/*	IEnumerator ScatterShoot()
    {
		for(int i = 0; i < 4; i++)
        {
			GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f, -0.2f), Quaternion.identity) as GameObject;
        }
    }*/
	IEnumerator AttackStunState()
	{
		
		/*AttackFlicker();*/
		/*StartCoroutine(FlickerState());*/
		yield return new WaitForSeconds(attackStun);
		switch (i) {
			case 0:
				enemyAnimator.SetBool("PokePreAttack", false);
				enemyAnimator.SetBool("PokeAttack", true);
				break;
			case 1:
				stopForAttack = true;
				/*yield return new WaitForSeconds(0.5f);*/
				enemyAnimator.SetBool("SlashPreAttack", false);
				enemyAnimator.SetBool("SlashAttack", true);
				break;
			case 2:
				enemyAnimator.SetBool("ProjectilePreAttack", false);
				break;
		}
		DoDamage();

		

		StartCoroutine(AttackCooldown());
	}
/*	IEnumerator FlickerState()
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
	}*/
	IEnumerator DestroyEnemy()
	{
		CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
		capsule.size = new Vector2(1f, 0.25f);
		capsule.offset = new Vector2(0f, -0.8f);
		capsule.direction = CapsuleDirection2D.Horizontal;
		yield return new WaitForSeconds(0.25f);
		rb.velocity = new Vector2(0, rb.velocity.y);
		yield return new WaitForSeconds(2f);
		gm.boss1Defeated = true;
		Destroy(gameObject);
	}
}
