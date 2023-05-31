using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : MonoBehaviour
{
	public Vector2 direction;
	private Vector3 vector3;
	public bool hasHit = false;
	public float speed = 10f;
	public bool playerShoot= false;

    // Start is called before the first frame update
    void Start()
    {
		vector3 = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if ( !hasHit)
		GetComponent<Rigidbody2D>().velocity = direction * speed;
		if (direction.x < 0)
		{
			gameObject.transform.localScale = new Vector3(-vector3.x, vector3.y, vector3.z);
		}
	}

    void OnCollisionEnter2D(Collision2D collision)
	{

			if (collision.gameObject.tag == "Enemy")
			{
				collision.gameObject.SendMessage("ApplyDamage", Mathf.Sign(direction.x) * 2f);
				Destroy(gameObject);
			}
			else if (collision.gameObject.tag != "Player")
			{
				Destroy(gameObject);
			}
		
	}
}
