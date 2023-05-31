using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgTxt : DmgTxtBase
{
    public GameObject txt;
    protected Enemy enemy;
    [SerializeField]private Vector2 a, b, c, d;
    void Awake()
    {
        enemy = GetComponent<Enemy>();
        txt.SetActive(false);
         /*a = new Vector2(-2.46f, -1.86f);
         b = new Vector2(2.09f, -1.65f);
         c = new Vector2(-0.67f, -0.12f);
         d = new Vector2(3.8f, -0.42f);*/
    }

    // Update is called once per frame
    void Update()
    {
       Lerpy(txt, enemy,a,b,c,d);
    }
   public void dmgTextEnable()
    {
        StartCoroutine(DamageText(txt,enemy));
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
