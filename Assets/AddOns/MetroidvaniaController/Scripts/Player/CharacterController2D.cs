using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_WallCheck;								//Posicion que controla si el personaje toca una pared

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	public float coyoteTime = 0.1f;
	public float coyoteTimeCounter;
	private Rigidbody2D m_Rigidbody2D;
	[SerializeField]private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;
	private float limitFallSpeed = 25f; // Limit fall speed
	public GameObject Player;
	protected Transform currentPosition;
	public Vector3 respawnPosition;
	public GameManager GM;
	public GameObject Sprite;
	public bool healthUp = false;
	public GameObject healthUpText;
	public List<bool> abilities;
	public List<GameObject> AbilityTxt;

	public bool canDoubleJump = true; //If player can double jump
	[SerializeField] private float m_DashForce = 25f;
	public bool backgroundAssetCollider = false;
	public bool canDash = true;
	private bool isDashing = false; //If player is dashing
	public bool isParrying = false;//If player is parrying
	public bool isHealing = false; //If player is healing
	public bool isPaused = false; //If player pauses
	public bool inDialogue = false; //If Player is in Dialogue
	public bool PauseToggle = false; //If false, Turn on Pause, If True, don't turn on
	public bool isBeingHit = false; //If player is in Hit Stun
	[SerializeField]private bool m_IsWall = false; //If there is a wall in front of the player
	private bool isWallSliding = false; //If player is sliding in a wall
	[SerializeField]private int wallSlidingNum = 0; //For WallSticking before WallSlide
	private bool oldWallSlidding = false; //If player is sliding in a wall in the previous frame
	private float prevVelocityX = 0f;
	private bool canCheck = false; //For check if player is wallsliding
	public bool hasParried = false; //Only true when player has succesfully parried an enemy attack

	public float life; //Life of the player
	public float parryTime = 1f; //Amount of time a player is invincible while parrying
	public bool invincible = false; //If player can die
	public bool canMove = true; //If player can move
	public int maxHp;//Maximum Player Health, Can be manipulated through upgrades
	public Animator animator;
	public ParticleSystem particleJumpUp; //Trail particles
	public ParticleSystem particleJumpDown; //Explosion particles

	private float jumpWallStartX = 0;
	private float jumpWallDistX = 0; //Distance between player and wall
	private bool limitVelOnWallJump = false; //For limit wall jump distance with low fps

	[Header("Events")]
	[Space]

	public UnityEvent OnFallEvent;
	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		/*animator = GetComponent<Animator>();*/

		if (OnFallEvent == null)
			OnFallEvent = new UnityEvent();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
		GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        if (GM.MaxHealth == 0)
        {
			GM.MaxHealth = 10;
        }
        else
        {
			maxHp = GM.MaxHealth;
        }
	}

    private void Start()
    {
		transform.position = GM.CheckpointPos;
		life = maxHp;
		abilities = GM.abilities;

	}
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Insert))
        {
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

		}
        if (isBeingHit && isHealing)
        {
			isHealing = false;
			canMove = true;
        }
        if (!isPaused && Time.timeScale == 0)
        {
			Time.timeScale = 1;
        }
        if (healthUp)
        {
			StartCoroutine(HealthUpTxt());
            if (!m_FacingRight)
            {
                /*Debug.Log(healthUpText.GetComponent<RectTransform>().localScale);*/
                healthUpText.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
            }
            else
            {
				healthUpText.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
			}
        }
		if (!m_FacingRight)
		{
			for (int i = 0; i < AbilityTxt.Count; i++)
			{
				AbilityTxt[i].GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
			}

		}
        else
        {
			for (int i = 0; i < AbilityTxt.Count; i++)
			{
				AbilityTxt[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
			}
		}
        if (isPaused||inDialogue)
        {
			Cursor.visible = true;
        }
        else
        {
			Cursor.visible = false;
        }

		/*Debug.Log(PauseToggle);*/
    }

	public void AbilityUnlockFunc(int i)
    {
		StartCoroutine(AbilityUnlock(i));
    }
	public void PauseBehaviour()
    {
		isPaused = true;
		canMove = false;
		canDash = false;
		PauseToggle = true;
	}
	public IEnumerator PauseDisable()
    {
		Time.timeScale = 1;
		isPaused = false;
		canMove = true;
		canDash = true;
		PauseToggle = false;
		yield return new WaitForSeconds(0.2f);
		
    }
    private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
        /*if (Input.GetKeyUp("W") && m_Rigidbody2D.velocity.y > 0f)
        {
            coyoteTimeCounter = 0f;
        }*/

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject && !colliders[i].gameObject.CompareTag("BackgroundAssets")&& colliders[i].gameObject != colliders[i].gameObject.CompareTag("Enemy"))
				m_Grounded = true;
				if (!wasGrounded )
				{
					OnLandEvent.Invoke();
					if (!m_IsWall && !isDashing) 
						particleJumpDown.Play();
					canDoubleJump = true;
					if (m_Rigidbody2D.velocity.y < 0f)
						limitVelOnWallJump = false;
				}
			if (colliders[i].gameObject.CompareTag("BackgroundAssets")&&colliders[i].gameObject.CompareTag("Enemy"))
			{	
				m_Grounded = false;
				canDoubleJump = false;
				backgroundAssetCollider = true;
			}
		}
		m_IsWall = false;

		if (m_Grounded)
		{
			wallSlidingNum = 0;
			coyoteTimeCounter = coyoteTime;

		}
		if (!m_Grounded)
		{
			OnFallEvent.Invoke();
			Collider2D[] collidersWall = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_WhatIsGround);
			for (int i = 0; i < collidersWall.Length; i++)
			{
				if (collidersWall[i].gameObject != null && !collidersWall[i].gameObject.CompareTag("BackgroundAssets")&&!collidersWall[i].gameObject.CompareTag("Enemy"))
				{
					isDashing = false;
					m_IsWall = true;
				}
				if (collidersWall[i].gameObject.CompareTag("BackgroundAssets"))
				{		canDoubleJump = false;
						backgroundAssetCollider = true;
				}
			}
            
			coyoteTimeCounter -= Time.deltaTime;
			prevVelocityX = m_Rigidbody2D.velocity.x;
		}
		
		if (limitVelOnWallJump)
		{
			if (m_Rigidbody2D.velocity.y < -0.5f)
				limitVelOnWallJump = false;
			jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
			if (jumpWallDistX < -0.5f && jumpWallDistX > -1f) 
			{
				canMove = true;
			}
			else if (jumpWallDistX < -1f && jumpWallDistX >= -2f) 
			{
				canMove = true;
				m_Rigidbody2D.velocity = new Vector2(10f * transform.localScale.x, m_Rigidbody2D.velocity.y);
			}
			else if (jumpWallDistX < -2f) 
			{
				limitVelOnWallJump = false;
				m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
			}
			else if (jumpWallDistX > 0) 
			{
				limitVelOnWallJump = false;
				m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
			}
		}
		currentPosition = Player.transform;
	}

	public Transform getPosition()
    {
		return currentPosition;
    }
	public void Move(float move, bool jump, bool dash)
	{
		if (canMove && !isParrying&&!isHealing&&!isPaused&&!inDialogue) {
			if (dash && canDash && !isWallSliding)
			{
				//m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * m_DashForce, 0f));
				StartCoroutine(DashCooldown());
			}
			// If crouching, check to see if the character can stand up
			if (isDashing)
			{
				m_Rigidbody2D.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
			}
			//only control the player if grounded or airControl is turned on
			else if (m_Grounded || m_AirControl)
			{
				if (m_Rigidbody2D.velocity.y < -limitFallSpeed)
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -limitFallSpeed);
				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
				// And then smoothing it out and applying it to the character
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight && !isWallSliding)
				{
					// ... flip the player.
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && m_FacingRight && !isWallSliding)
				{
					// ... flip the player.
					Flip();
				}
			}
            if (m_Rigidbody2D.velocity.y > 0f)
            {
                coyoteTimeCounter = 0f;
            }
            // If the player should jump...
            if (coyoteTimeCounter>0f && jump)
			{
				// Add a vertical force to the player.
				coyoteTimeCounter = 0f;
				animator.SetBool("IsJumping", true);
				animator.SetBool("JumpUp", true);
				m_Grounded = false;
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
				canDoubleJump = true;
				particleJumpDown.Play();
				particleJumpUp.Play();
			}
			else if (!m_Grounded && jump && canDoubleJump && !isWallSliding)
			{
				canDoubleJump = false;
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce / 1.2f));
				animator.SetBool("IsDoubleJumping", true);
			}

			else if (m_IsWall && !m_Grounded)
			{
				if (!oldWallSlidding && m_Rigidbody2D.velocity.y < 0 || isDashing)
				{
					isWallSliding = true;
					m_WallCheck.localPosition = new Vector3(-m_WallCheck.localPosition.x, m_WallCheck.localPosition.y, 0);
					Flip();
					StartCoroutine(WaitToCheck(0.1f));
					canDoubleJump = true;
					animator.SetBool("IsWallSliding", true);
				}
				isDashing = false;

				if (isWallSliding)
				{
					if (move * transform.localScale.x > 0.1f&&wallSlidingNum>0)
					{
						StartCoroutine(WaitToEndSliding());
					}
					else if (wallSlidingNum > 0)
                    {
						oldWallSlidding = true;
                        m_Rigidbody2D.velocity = new Vector2(-transform.localScale.x * 2, -5);
                    }
                    else if (wallSlidingNum == 0&&m_IsWall)
                    {
                        StartCoroutine(WallSlidePause());
                    }
                }

				if (jump && isWallSliding)
				{
					animator.SetBool("IsJumping", true);
					animator.SetBool("JumpUp", true);
					m_Rigidbody2D.gravityScale = 5f;
					m_Rigidbody2D.velocity = new Vector2(0f, 0f);
					m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * m_JumpForce *1.2f, m_JumpForce));
					jumpWallStartX = transform.position.x;
					limitVelOnWallJump = true;
					canDoubleJump = true;
					isWallSliding = false;
					animator.SetBool("IsWallSliding", false);
					oldWallSlidding = false;
					m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
					canMove = false;
				}
				else if (dash && canDash)
				{
					isWallSliding = false;
					animator.SetBool("IsWallSliding", false);
					oldWallSlidding = false;
					m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
					canDoubleJump = true;
					StartCoroutine(DashCooldown());
				}
			}
			else if (isWallSliding && !m_IsWall && canCheck /*&&wallSlidingNum>0*/) 
			{
				isWallSliding = false;
				animator.SetBool("IsWallSliding", false);
				oldWallSlidding = false;
				m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
				canDoubleJump = true;
			}
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Enemy")
        {
            if (isDashing)
            {

            }
        }
    }
    private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void AppyDamageProjectile(float damage)
    {
		if(!invincible)
        {
			animator.SetBool("Hit", true);
			life -= damage;
            if (life <= 0)
            {
				StartCoroutine(WaitToDead());
            }
            else
            {
				/*StartCoroutine(Sprite.GetComponent<weirdassscript>().FlickerState(1f));*/

				StartCoroutine(Stun(0.25f));

				StartCoroutine(MakeInvincible(1f));
            }
        }
    }
	public void SetRespawnPoint(Vector3 newRespawnPosition)
	{
		respawnPosition = new Vector3(newRespawnPosition.x,newRespawnPosition.y+5);
		GM.CheckpointPos = respawnPosition;
		
	}
	public void ApplyDamage(float damage, Vector3 position)
	{
		if (!invincible)
		{
			animator.SetBool("Hit", true);
			life -= damage;
			Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f ;
			m_Rigidbody2D.velocity = Vector2.zero;
			m_Rigidbody2D.AddForce(damageDir * 10);
			if (life <= 0)
			{
				life = 0;
				StartCoroutine(WaitToDead());
			}
			else
			{
				/*StartCoroutine(Sprite.GetComponent<weirdassscript>().FlickerState(1f));*/

				StartCoroutine(Stun(0.25f));
				
				StartCoroutine(MakeInvincible(1f));
			}
		}
	}
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (isDashing)
            {
				StartCoroutine(DashInvul());
            }
        }
    }


    public void Heal(int healingamt)
    {
        if (life < maxHp && life!=0)
        {
			life += healingamt;
            if (life > maxHp)
            {
				life = maxHp;
            }
        }
    }

	public void Respawn()
    {
		isBeingHit = false;
		canMove = true;
		invincible = false;
		GetComponent<Attack>().enabled = true;
		GM.PlayerRespawn();
       /* SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        transform.position = GM.CheckpointPos;
		life = maxHp;*/
		
	}

	public IEnumerator HealthUpTxt()
    {
		healthUpText.SetActive(true);
		yield return new WaitForSeconds(2f);
		healthUpText.SetActive(false);
		healthUp = false;
    }
	public IEnumerator AbilityUnlock(int i)
    {
        AbilityTxt[i].SetActive(true);
        yield return new WaitForSeconds(2f);
		AbilityTxt[i].SetActive(false);
    }
	IEnumerator DashCooldown()
	{
		if (abilities[1])
		{
			animator.SetBool("IsDashing", true);
		}
		isDashing = true;
		canDash = false;
		yield return new WaitForSeconds(0.1f);
		isDashing = false;
		yield return new WaitForSeconds(0.3f);
		canDash = true;
	}
	IEnumerator DashInvul()
    {
		GetComponent<CapsuleCollider2D>().enabled = false;
		yield return new WaitForSeconds(0.1f);
		GetComponent<CapsuleCollider2D>().enabled = true;
	}

	IEnumerator Stun(float time) 
	{
		canMove = false;
		isBeingHit = true;
		yield return new WaitForSeconds(time);
		isBeingHit = false;
		canMove = true;
	}
	IEnumerator MakeInvincible(float time) 
	{
		invincible = true;
		yield return new WaitForSeconds(time);
		invincible = false;
	}
	IEnumerator WaitToMove(float time)
	{
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}

	IEnumerator WaitToCheck(float time)
	{
		canCheck = false;
		yield return new WaitForSeconds(time);
		canCheck = true;
	}

	IEnumerator WaitToEndSliding()
	{
		yield return new WaitForSeconds(0.1f);
		canDoubleJump = true;
		isWallSliding = false;
		animator.SetBool("IsWallSliding", false);
		oldWallSlidding = false;
		m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
	}

	IEnumerator WallSlidePause()
    {
		if (wallSlidingNum == 0)
		{
			m_Rigidbody2D.gravityScale = 0f;
			m_Rigidbody2D.velocity = Vector2.zero;
		}
        yield return new WaitForSeconds(0.3f);
		wallSlidingNum++;
        canMove = true;
        m_Rigidbody2D.gravityScale = 5f;
    }
	
	IEnumerator WaitToDead()
	{
		animator.SetBool("IsDead", true);
		canMove = false;
		invincible = true;
		GetComponent<Attack>().enabled = false;
		yield return new WaitForSeconds(0.4f);
		m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
		yield return new WaitForSeconds(1.1f);
		Respawn();
		/*SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);*/
	}
}
