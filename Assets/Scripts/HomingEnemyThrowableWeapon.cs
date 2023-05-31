using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemyThrowableWeapon : MonoBehaviour
{
	[SerializeField]public Vector2 direction;
	private Vector3 vector3;
	public bool hasHit = false;
	public float speed = 10f;
	private CharacterController2D characterController2D;
	private BossEnemy bossEnemy;
	public float projectileDmg;
	private Rigidbody2D rb;
	public float rotateSpeed;
	private bool rightProjectile;
	[SerializeField] protected Transform target;
	 private CircleCollider2D col;

    // Start is called before the first frame update
    private void Awake()
    {
		characterController2D=FindObjectOfType<CharacterController2D>();
		bossEnemy = FindObjectOfType<BossEnemy>();
		vector3 = gameObject.transform.localScale;
	}
    void Start()
	{
		rb = GetComponent<Rigidbody2D>();
        if (bossEnemy.facingRight)
        {
			 rightProjectile=true;
        }
		col = GetComponent<CircleCollider2D>();
		col.enabled = true;
	}
	public void RightProjectile()
    {
		rightProjectile = true;
    }

	// Update is called once per frame
	void FixedUpdate()
	{
		target = characterController2D.getPosition();
		Vector2 direction = (Vector2)target.position - rb.position;
		direction.Normalize();
		if (!rightProjectile)
		{
			float rotateAmount = Vector3.Cross(direction, transform.right).z;
			rb.angularVelocity = -rotateSpeed * rotateAmount;
			rb.velocity = transform.right * speed;
		}
        else
        {
			float rotateAmount = Vector3.Cross(direction, transform.right).z;
			rb.angularVelocity = rotateSpeed * rotateAmount;
			rb.velocity = -transform.right * speed;
		}
        if (rightProjectile)
        {
            gameObject.transform.localScale = new Vector3(-vector3.x, vector3.y, vector3.z);
        }
    }

	void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.tag == "Player")
		{
            col.enabled = true;
            rb.isKinematic = false;
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
