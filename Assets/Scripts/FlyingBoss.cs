using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FlyingBoss : MonoBehaviour
{

	public float life = 10;
	public float MaxHP = 80;
	private bool isPlat;
	private bool isObstacle;
	private Transform fallCheck;
	private Transform wallCheck;
	public LayerMask turnLayerMask;
	private Rigidbody2D rb;
	protected GameObject player;
	public Animator enemyAnimator;
	public SpriteRenderer enemySprite;
	public Transform attackCheck;
	public bool facingRight = true;
	public float dmgValue = 1;
	public float projectileDmg = 1f;
	public GameObject throwableObject;
	private int timesHit = 0;
	[SerializeField] private float hitTime = 0.1f;
	[SerializeField] private float bigHitTime = 3f;
	[SerializeField] private float hitResistance = 3;
	public bool bossFightStarted = false;
	public GameObject explosion;

   /* [SerializeField] private bool playerCol;*/
    public Material mat;
	private Material OGmat;
	public float speed = 5f;
	public float attackCooldown = 3f;
	public float attackDistance = 2f;
	public bool isInvincible = false;
	private bool isHitted = false;
	private bool canAttack = true;
	private float dist;
	private Color OGcolor;
	private bool isFlickerEnabled = false;
	public Transform ProjectileSpawn;
    /*[SerializeField] private bool isAttacking = false;*/
    [SerializeField] private bool isChasing;
	/*[SerializeField] private float chaseDist = 6;*/
	[SerializeField] private float attackStun = 0.5f;
	[SerializeField] private bool atWaypoint = false;
	[SerializeField] private bool moveWaypoint = true;
	[SerializeField] private int waypointNum = 1;

	public System.Collections.Generic.List<Transform> waypoints; //List of Waypoints for Boss 
	void Awake()
	{
		fallCheck = transform.Find("FallCheck");
		wallCheck = transform.Find("WallCheck");
		rb = GetComponent<Rigidbody2D>();
		OGcolor = enemySprite.color;
		OGmat = enemySprite.material;
		life = MaxHP;
	}
	void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player");
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
		/*isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Default"));
		isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);*/

		if (!isHitted && life > 0 &&bossFightStarted)
		{
			if (atWaypoint)
			{
				
				StartCoroutine(StayAtPos());
			}
            if (moveWaypoint)
            {
				atWaypoint = false;
				switch (waypointNum){
                    case 1: 
						MoveTo(1);
						break;
					case 2: 
						MoveTo(2);
						break;
					case 3:
						MoveTo(3);
						break;
					case 4:
						MoveTo(4);
						break;
					case 5:
						MoveTo(5);
						break;
					case 6:
						MoveTo(0);
						break;
				}

			}
			if (transform.position.x > player.transform.position.x)
			{
				transform.localScale = new Vector3(-2, 2, 2);
				facingRight = true;
			}
			else
			{
				transform.localScale = new Vector3(2, 2, 2);
				facingRight = false;
			}
		}


		/*if (!isHitted && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f && !isAttacking) //movement function starts
		{
			if (isPlat && !isObstacle && !isHitted)
			{
				if (isChasing) //chaser AI
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
		if (canAttack == true && life > 0 &&bossFightStarted)
		{
			/*if (Mathf.Abs(dist) < attackDistance)*/
			/*{*/
				canAttack = false;
				enemyAnimator.SetBool("preAttack", true);
				StartCoroutine(AttackStunState());

			/*}*/
		}
	}

	private void MoveTo(int num)
    {
		transform.position = Vector3.MoveTowards(transform.position, waypoints[num].position, speed * Time.deltaTime);
		Debug.Log(num);
		StartCoroutine(Moving());
        if (transform.position == waypoints[num].position)
        {
			atWaypoint=true;
			
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
	void DoDamage()
	{
		if ((int)Random.Range(0, 2) == 1)
		{
			explosion.SetActive(true);
			Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 2f);
			for (int i = 0; i < collidersEnemies.Length; i++)
			{
				if (collidersEnemies[i].gameObject.tag == "Player" && life > 0)
				{
					if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
					{
						/*dmgValue = -dmgValue;*/
					}
					collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().ApplyDamage(dmgValue, transform.position);
				}
			}
		}
		else
		{
			StartCoroutine(MultiShoot());
		}
	}
	void AttackFlicker()
	{
		isFlickerEnabled = true;
	}
	public void ApplyDamage(float damage)
	{
		if (!isInvincible)
		{
			timesHit++;
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			transform.GetComponent<Animator>().SetBool("Hit", true);
			life -= damage;
			GetComponent<DmgTxtFlyBoss>().dmgTextEnable();
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(direction * 500f, 100f));
			if (timesHit > hitResistance)
			{
				StartCoroutine(BigHitTime());
				timesHit = 0;
			}
			else
			{
				StartCoroutine(HitTime());
			}
		}
	}
	//collision damage
	/*void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(1f, transform.position);
			StartCoroutine(CollisionDisable());
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
	IEnumerator CollisionDisable()
	{
		GetComponent<CapsuleCollider2D>().enabled = false; ;
		yield return new WaitForSeconds(0.25f);
		GetComponent<CapsuleCollider2D>().enabled = true;
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
		enemyAnimator.SetBool("isAttacking", false);
		explosion.SetActive(false);
		yield return new WaitForSeconds(attackCooldown);
		canAttack = true;
	}
	IEnumerator MultiShoot()
	{

		for (int i = 0; i < 6; i++)
		{
			GameObject throwableWeapon = Instantiate(throwableObject, ProjectileSpawn.position, Quaternion.identity) as GameObject;
			Vector2 direction = new Vector2(transform.localScale.x, 0);
			throwableObject.GetComponent<FlyingBProjectile>().speed = 10f;
			throwableWeapon.GetComponent<FlyingBProjectile>().projectileDmg = projectileDmg;
			throwableWeapon.name = "EnemyThrowableWeapon";
			yield return new WaitForSeconds(0.5f);
		}
	}
	IEnumerator Moving()
    {
		yield return new WaitUntil(()=> atWaypoint);
		canAttack = false;
		moveWaypoint = false;
		if (waypointNum < 6)
			waypointNum++;
		else
			waypointNum = 1;
    }
	IEnumerator StayAtPos()
    {
		moveWaypoint = false;
		
		yield return new WaitForSeconds(10f);
		moveWaypoint = true;
		atWaypoint = false;
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
			enemySprite.color = Color.black;
			yield return new WaitForSeconds(attackStun / 4);
			enemySprite.color = Color.white;
			yield return new WaitForSeconds(attackStun / 4);
			enemySprite.color = Color.black;
			yield return new WaitForSeconds(attackStun / 4);
			enemySprite.color = Color.white;
			yield return new WaitForSeconds(attackStun / 4);
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
		yield return new WaitForSeconds(3f);
		SceneManager.LoadSceneAsync(0);
		Destroy(gameObject);
	}
}
