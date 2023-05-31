using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgTxtFlyBoss : DmgTxtBase
{
    public GameObject txt;
    protected FlyingBoss enemy;
    public GameObject boss;
    private Vector2 a, b, c, d;
    void Awake()
    {
        enemy = boss.GetComponent<FlyingBoss>();
        txt.SetActive(false);
        a = new Vector2(-2.85f, -0.87f);
        b = new Vector2(2.26f, -0.78f);
        c = new Vector2(-1.72f, 0.07f);
        d = new Vector2(3.47f, 0.47f);
    }

    // Update is called once per frame
    void Update()
    {
        Lerpy(txt, enemy, a, b, c, d);
    }
    public void dmgTextEnable()
    {
        StartCoroutine(DamageText(txt, enemy));
    }
    private void OnDrawGizmosSelected()
    {
        Vector2 LocalPointA = transform.TransformPoint(a);
        Vector2 LocalPointB = transform.TransformPoint(b);
        Vector2 LocalPointC = transform.TransformPoint(c);
        Vector2 LocalPointD = transform.TransformPoint(d);
        Gizmos.color = Color.yellow;
        /*Gizmos.matrix = transform.localToWorldMatrix;*/
        Gizmos.DrawLine(LocalPointA, LocalPointC);
        Gizmos.DrawLine(LocalPointB, LocalPointD);
    }

}
