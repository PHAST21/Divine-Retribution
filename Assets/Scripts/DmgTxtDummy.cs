using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgTxtDummy : DmgTxtBase
{
    public GameObject txt;
    protected TrainingDummy enemy;
    [SerializeField]private Vector2 a, b, c, d;
    void Awake()
    {
        enemy = GetComponent<TrainingDummy>();
        txt.SetActive(false);
        /*       a = new Vector2(1.85f, -3.28f);
               b = new Vector2(2.09f, -1.65f);
               c = new Vector2(-0.11f, 1.17f);
               d = new Vector2(5f, 0.85f);*/
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
