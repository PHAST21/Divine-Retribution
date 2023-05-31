using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
	public bool isParried = false;
	protected void DoDamage(Transform attackCheck, float life, bool isHitted, Rigidbody2D rb, Animator enemyAnimator, float dmgValue)
	{
		if (!isParried)
		{
			Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
			for (int i = 0; i < collidersEnemies.Length; i++)
			{
				if (collidersEnemies[i].gameObject.tag == "Player" && life > 0)
				{
					if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
					{
						/*dmgValue = -dmgValue;*/
					}
					if (collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().isParrying)
					{
						isHitted = true;

						enemyAnimator.SetBool("preAttack", false);
						enemyAnimator.SetBool("isParried", true);
                        enemyAnimator.SetBool("IsAttacking", false);
                        StartCoroutine(Parried(isHitted, rb, enemyAnimator));
						GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().hasParried = true;
					}
					else
					{
						enemyAnimator.SetBool("IsAttacking", true);
						collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().ApplyDamage(dmgValue, transform.position);
					}
				}
			}
		}
        else
        {
			enemyAnimator.SetBool("isParried", true);
		}
	}

	IEnumerator Parried(bool isHitted, Rigidbody2D rb, Animator enemyAnimator)
	{
		isParried = true;
		yield return new WaitForSeconds(0.75f);
		GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().hasParried = false;
		rb.velocity = Vector3.zero;
		enemyAnimator.SetBool("isParried", false);
		isParried = false;
		isHitted = false;
	}
}
