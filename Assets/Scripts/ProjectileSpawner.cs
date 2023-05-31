using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject throwableObject;
    public GameObject spawnPoint;
    [SerializeField] private bool canAttack = true;
    public int a;
    // Start is called before the first frame update
    void Start()
    {
        canAttack=true;
        InvokeRepeating("ShootFunc", 2.0f, 0.75f);
    }

    
    public void ShootFunc()
    {
        if(canAttack)
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        spawnPoint.transform.localPosition = new Vector2(Random.Range(0.57f, 1.15f), Random.Range(-0.67f, -0.33f));
        canAttack = false;
        GameObject throwableWeapon = Instantiate(throwableObject, spawnPoint.transform.position + new Vector3(transform.localScale.x * 0.5f, -0.2f), Quaternion.Euler(0f, 0f, -45f)) as GameObject;
        Vector2 direction = new Vector2(-spawnPoint.transform.localScale.x, -spawnPoint.transform.localScale.y);
        throwableWeapon.GetComponent<PassThroughProjectile>().direction = direction;
        throwableWeapon.name = "EnemyPassThruProjectile";
        yield return new WaitForSecondsRealtime(5f);
        canAttack = true;
    }
}
