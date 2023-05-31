using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgTxtRanged : DmgTxtBase
{
    public GameObject txt;
    protected RangedEnemy enemy;
    public GameObject baseEnemy;
    [SerializeField] private Vector2 a, b, c, d;
    void Awake()
    {
        enemy = baseEnemy.GetComponent<RangedEnemy>();
        txt.SetActive(false);
        a = new Vector2(-2.26f, -2.5f);
        b = new Vector2(2.24f, -2.39f);
        c = new Vector2(-0.73f, -1.83f);
        d = new Vector2(4.37f, -1.34f);
    }

    // Update is called once per frame
    void Update()
    {
        Lerpy(txt, enemy, a, b, c, d);
    }
    public void DmgTextEnable()
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
