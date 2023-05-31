using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Animator TurretAnim;
    public GameObject throwableObject;
    public GameObject spawnPoint;
    public Sprite sprite;
    [SerializeField] private bool canAttack = true;
    public int a;
    // Start is called before the first frame update
    void Start()
    {
        TurretAnim = GetComponent<Animator>();
        canAttack = true;
        InvokeRepeating(nameof(ShootFunc), 2.0f, 0.75f);
    }


    public void ShootFunc()
    {
        if (canAttack)
            StartCoroutine(PreShoot());
    }

    IEnumerator Shoot()
    {
        /*spawnPoint.transform.localPosition = new Vector2(Random.Range(0.57f, 1.15f), Random.Range(-0.67f, -0.33f));*/
        TurretAnim.SetBool("isAttacking", true);
        canAttack = false;
        GameObject throwableWeapon = Instantiate(throwableObject, (new Vector3(transform.position.x,transform.position.y+0.3f)) + (new Vector3(transform.localScale.x * 0.5f, -0.2f)), Quaternion.identity) as GameObject;
        throwableWeapon.GetComponent<SpriteRenderer>().sprite = sprite;
        throwableWeapon.GetComponent<SpriteRenderer>().flipX = true;
        throwableWeapon.GetComponent<Transform>().localScale= new Vector3(2.161f, 2.161f, 1);
        Vector2 direction = new Vector2(transform.localScale.x, 0);
        throwableWeapon.GetComponent<EnemyThrowableWeapon>().direction = direction;
        throwableWeapon.name = "EnemyThrowableWeapon";
        yield return new WaitForSeconds(1f);
        TurretAnim.SetBool("isAttacking", false);
        yield return new WaitForSecondsRealtime(3f);
        canAttack = true;
    }

    IEnumerator PreShoot()
    {
        TurretAnim.SetBool("preAttack", true);
        yield return new WaitForSeconds(0.5f);
        TurretAnim.SetBool("preAttack", false);
        StartCoroutine(Shoot());
    }
}
