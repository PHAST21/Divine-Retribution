using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public float dmgValue = 3;
	[SerializeField]private float OGDmg;
	public GameObject throwableObject;
	public Transform attackCheck;
	private Rigidbody2D m_Rigidbody2D;
	public Animator animator;
	public bool canAttack = true;
	public bool canFireball = true;
	public bool canAttack2 = false;
	public bool canAttack2Alt = false;
	public int AtkInput = 0;
	public bool isTimeToCheck = false;
	private CharacterController2D characterController;
	public GameObject cam;
	[SerializeField]private HPotion Hpotion;
	public int healingAmt = 2;
	public int maxHeal = 5;
	[SerializeField]private int currentHeals=5;
	[SerializeField]private float deltaTime = 0;
	public Material healaura;
	private Material OGmat;
	public SpriteRenderer sprite;
	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
    {
		characterController = GetComponent<CharacterController2D>();
		Hpotion = GameObject.FindGameObjectWithTag("UIPotion").GetComponent<HPotion>();
		currentHeals = maxHeal;
		OGmat = sprite.material;
		OGDmg = dmgValue;
    }

	// Update is called once per frame
	void Update()
	{
		if (deltaTime < 0.9f)
		{
			deltaTime += Time.deltaTime;
		}
		else
        {
			AtkInput = 0;
		}
/*        if (deltaTime > 0.4f)
        {
            canAttack2 = false;
        }
*/		if (AtkInput == 0&&!characterController.hasParried)
		{
			dmgValue = OGDmg;	
		}
		if (Input.GetKeyDown(KeyCode.J)&&!characterController.hasParried)
        {
            /*dmgValue = OGDmg;*/
            AtkInput++;
			AtkInput = Mathf.Clamp(AtkInput, 0, 2);
			Attacking();
		}
        if (characterController.hasParried && Input.GetKeyDown(KeyCode.J))
        {
            AtkInput = 5;
            Attacking();

        }
		if (Input.GetKeyDown(KeyCode.K)&& characterController.abilities[0] && canFireball && !characterController.isPaused&& !characterController.inDialogue)
		{
			GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f, -0.2f), Quaternion.identity) as GameObject;
			Vector2 direction = new Vector2(transform.localScale.x, 0);
			throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction;
			throwableWeapon.name = "ThrowableWeapon";
			canFireball = false;
			StartCoroutine(FireballCooldown());
		}

		if (Input.GetKeyDown(KeyCode.I) && characterController.m_Grounded && !characterController.isParrying && !characterController.isPaused && !characterController.inDialogue)
		{
			canAttack = false;
			characterController.isParrying = true;
			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			StartCoroutine(MakeInvincibleParry(characterController.parryTime));
			animator.SetBool("IsParrying", true);
		}
        if (Input.GetKeyDown(KeyCode.U)&&currentHeals>0&&characterController.m_Grounded &&!characterController.isHealing&&!characterController.isPaused&& !characterController.inDialogue)
        {

			
			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			StartCoroutine(HealCooldown());
			animator.SetBool("IsHealing", true);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
			canAttack = false;
			characterController.PauseBehaviour();
			GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().EnablePause();
			Time.timeScale = 0;

		}
        if (Input.GetKeyDown(KeyCode.Escape) && characterController.isPaused)
        {
			StartCoroutine(characterController.PauseDisable());
        }
        if (characterController.inDialogue)
        {
			m_Rigidbody2D.velocity = new Vector2(0, -10f);
        }

	}

	public void Attacking()
    {
		if (!characterController.isPaused && !characterController.inDialogue)
		{
			switch (AtkInput)
			{  
				case 1:
					if (canAttack)
					{
                        dmgValue = OGDmg;
                        canAttack = false;
						deltaTime = 0f;
						animator.SetBool("IsAttacking", true);
						StartCoroutine(AttackCooldown());
					}
					break;
				case 2:
					if (canAttack2)
					{
						dmgValue=OGDmg+1;
						/*Debug.Log("Atk2");*/
						canAttack2 = false;
						animator.SetBool("IsAttacking2", true);
						StartCoroutine(Attack2Cooldown());
					}
					break;
				case 5:
					dmgValue = OGDmg+2;
					/*Debug.Log("ParryAtk");*/
					canAttack = false;
					
					animator.SetBool("ParryAtk", true);
					StartCoroutine(Attack2Cooldown());
					break;
			}
					/*if (AtkInput == 1 && canAttack && !characterController.isPaused && !characterController.inDialogue)
					{
						dmgValue = OGDmg;
						canAttack = false;
						deltaTime = 0f;
						animator.SetBool("IsAttacking", true);
						StartCoroutine(AttackCooldown());
					}
					if (AtkInput == 2 && canAttack2 && !characterController.isPaused && !characterController.inDialogue)
					{
						dmgValue++;
						Debug.Log("Atk2");
						canAttack2 = false;
						animator.SetBool("IsAttacking2", true);
						StartCoroutine(Attack2Cooldown());
					}*/

					/*		if (deltaTime > 0.5f && AtkInput == 2 && canAttack2Alt)
							{
								Debug.Log("Atk2Alt");
								canAttack2Alt = false;
								animator.SetBool("IsParrying", true);
								StartCoroutine(Attack2Cooldown());
							}*/
			}
	}
	public void DialogueBehaviour()
	{
		characterController.canMove = false;
		characterController.canDash = false;
		canAttack = false;
		canFireball = false;
		m_Rigidbody2D.velocity = Vector2.zero;

	}
	public void DialogueDisable()
	{
		characterController.canMove = true;
		characterController.canDash = true;
		canAttack = true;
		canFireball = true;
		m_Rigidbody2D.velocity = Vector2.zero;
	}
	public void RefillFlask()
    {
		currentHeals = maxHeal;
		Hpotion.HealReset();
    }
	IEnumerator MakeInvincibleParry(float time)
	{
		characterController.invincible = true;
		yield return new WaitForSeconds(time);
		canAttack = true;
		characterController.invincible = false;
		characterController.isParrying=false;
		StartCoroutine(ParryCooldown());
	}
	
	IEnumerator ParryCooldown()
    {
		yield return new WaitForSeconds(1f);
    }
	IEnumerator AttackCooldown()
	{
		canAttack2 = false;
		StartCoroutine(Attack2Stun());
		yield return new WaitForSeconds(0.8f);
		canAttack = true;
		
	}
	IEnumerator Attack2Stun()
    {
		yield return new WaitForSeconds(0.1f);
		canAttack2 = true;
		canAttack2Alt = true;
	}
	IEnumerator Attack2Cooldown()
    {
/*		yield return new WaitForSeconds(0.1f);
		characterController.hasParried = false;*/
		yield return new WaitForSeconds(0.9f);
		AtkInput = 0;
		canAttack = true;
    }
	IEnumerator HealCooldown()
    {
		currentHeals--;
		canAttack = false;
		sprite.material = healaura;
		characterController.isHealing = true;
		Hpotion.HealUse(currentHeals);
        yield return new WaitForSeconds(1f);
		if (characterController.isBeingHit == false)
		{
			characterController.Heal(healingAmt);
			characterController.isHealing = false;
			canAttack = true;
			sprite.material = OGmat;
		}
		else
		{
			characterController.isHealing = false;
			canAttack = true;
			sprite.material = OGmat;
		}
    }

	IEnumerator FireballCooldown()
    {
		yield return new WaitForSeconds(0.5f);
		canFireball = true;
    }

    public void DoDashDamage()
    {
        dmgValue = Mathf.Abs(dmgValue);
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 2f);
        for (int i = 0; i < collidersEnemies.Length; i++)
        {
			Debug.Log(i);
            if (collidersEnemies[i].gameObject.tag == "Enemy")
            {
                if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
                {
                    /*dmgValue = -dmgValue;*/
                }
                collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
                /*cam.GetComponent<CameraFollow>().ShakeCamera();*/
            }
        }
    }
}
