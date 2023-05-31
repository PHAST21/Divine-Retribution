using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgTxtBoss : DmgTxtBase
{
    public GameObject txt;
    protected BossEnemy enemy;
    public GameObject boss;
    [SerializeField] private Vector2 a, b, c, d;
    void Awake()
    {
        enemy = boss.GetComponent<BossEnemy>();
        txt.SetActive(false);
        a = new Vector2(-2.26f, -2.5f);
        b = new Vector2(2.48f, -1.42f);
        c = new Vector2(-0.73f, -0.88f);
        d = new Vector2(3.85f, -0.47f);
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
