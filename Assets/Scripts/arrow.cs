using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public GameObject target;
    public float speed = 10f;
    public Vector3 movePosition;
    private Rigidbody2D rb;
	public float projectileDmg;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 Movedir = (target.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(Movedir.x, Movedir.y);
    }

	void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.tag == "Player")
		{
			/*col.enabled = true;*/
			/*rb.isKinematic = false;*/
			target.GetComponent<CharacterController2D>().AppyDamageProjectile(projectileDmg) /*transform.position*/;
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


}