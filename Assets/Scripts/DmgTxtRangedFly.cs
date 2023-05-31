using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgTxtRangedFly : DmgTxtBase
{
    public GameObject txt;
    public GameObject baseEnemy;
    protected Rangedflying enemy;
    [SerializeField] private Vector2 a, b, c, d;
    void Awake()
    {
        enemy = GetComponent<Rangedflying>();
        txt.SetActive(false);
        a = new Vector2(-2.53f, -3.81f);
        b = new Vector2(2.27f, -3.83f);
        c = new Vector2(-1.18f, -2.77f);
        d = new Vector2(4.02f, -2.27f);
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
