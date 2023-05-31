using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgTxtBase : MonoBehaviour
{
    [SerializeField] private float t = 0;
    private bool atkChk = false;
    public float lerpSpeed = 0.05f;
    protected void Lerpy(GameObject txt, Enemy enemy,Vector2 a,Vector2 b, Vector2 c,Vector2 d)
    {
        /*Debug.Log(enemy.MaxHP - enemy.life);*/
        t += lerpSpeed;
        Mathf.Clamp(t, 0f, 1f);
        /*Debug.Log(t);*/

        
        if (enemy.facingRight)
        {
            txt.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            TxtLerp(b, d,txt);

        }
        else
        {
            txt.transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
         
            TxtLerp(a, c,txt);
        }
    }
    protected void Lerpy(GameObject txt, FlyingEnemy enemy, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        /*Debug.Log(enemy.MaxHP - enemy.life);*/
        t += lerpSpeed;
        Mathf.Clamp(t, 0f, 1f);
        /*Debug.Log(t);*/

        
        if (enemy.facingRight)
        {
            txt.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
           
            TxtLerp(b, d, txt);

        }
        else
        {
            txt.transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
            
            TxtLerp(a, c, txt);
        }
    }
    protected void Lerpy(GameObject txt, RangedEnemy enemy, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        /*Debug.Log(enemy.MaxHP - enemy.life);*/
        t += lerpSpeed;
        Mathf.Clamp(t, 0f, 1f);
        /*Debug.Log(t);*/

       
        if (enemy.facingRight)
        {
            txt.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            TxtLerp(b, d, txt);

        }
        else
        {
            txt.transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
            
            TxtLerp(a, c, txt);
        }
    }
    protected void Lerpy(GameObject txt, Rangedflying enemy, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        /*Debug.Log(enemy.MaxHP - enemy.life);*/
        t += lerpSpeed;
        Mathf.Clamp(t, 0f, 1f);
        /*Debug.Log(t);*/

        
        if (enemy.facingRight)
        {
            txt.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            TxtLerp(b, d, txt);

        }
        else
        {
            txt.transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
            
            TxtLerp(a, c, txt);
        }
    }
    protected void Lerpy(GameObject txt, BossEnemy enemy, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        /*Debug.Log(enemy.MaxHP - enemy.life);*/
        t += lerpSpeed;
        Mathf.Clamp(t, 0f, 1f);
        /*Debug.Log(t);*/

       
        if (enemy.facingRight)
        {
            txt.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            TxtLerp(b, d, txt);

        }
        else
        {
            txt.transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
            
            TxtLerp(a, c, txt);
        }
    }
    protected void Lerpy(GameObject txt, FlyingBoss enemy, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        /*Debug.Log(enemy.MaxHP - enemy.life);*/
        t += lerpSpeed;
        Mathf.Clamp(t, 0f, 1f);
        /*Debug.Log(t);*/

        
        if (enemy.facingRight)
        {
            txt.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            TxtLerp(b, d, txt);

        }
        else
        {
            txt.transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
            
            TxtLerp(a, c, txt);
        }
    }
    protected void Lerpy(GameObject txt, ArrowAngel enemy, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        /*Debug.Log(enemy.MaxHP - enemy.life);*/
        t += lerpSpeed;
        Mathf.Clamp(t, 0f, 1f);
        /*Debug.Log(t);*/


        if (enemy.facingRight)
        {
            txt.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            TxtLerp(b, d, txt);

        }
        else
        {
            txt.transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);

            TxtLerp(a, c, txt);
        }
    }
    protected void Lerpy(GameObject txt, TrainingDummy enemy, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        /*Debug.Log(enemy.MaxHP - enemy.life);*/
        t += lerpSpeed;
        Mathf.Clamp(t, 0f, 1f);
        /*Debug.Log(t);*/


        if (enemy.facingRight)
        {
            txt.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            TxtLerp(b, d, txt);

        }
        else
        {
            txt.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            TxtLerp(a, c, txt);
        }
    }

    void TxtLerp(Vector2 v1, Vector2 v2,GameObject txt)
    {
        txt.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(v1, v2, t);
        if (t >= 1f && !atkChk)
            t = 0f;
    }


    protected IEnumerator DamageText(GameObject txt,Enemy enemy)
    {
        atkChk = true;
        t = 0;
        txt.SetActive(true);
        txt.GetComponent<TMP_Text>().text = (enemy.MaxHP - enemy.life).ToString();
        yield return new WaitForSecondsRealtime(1f);
        txt.SetActive(false);

    }
        protected IEnumerator DamageText(GameObject txt,FlyingEnemy enemy)
    {
        atkChk = true;
        t = 0;
        txt.SetActive(true);
        txt.GetComponent<TMP_Text>().text = (enemy.MaxHp - enemy.life).ToString();
        yield return new WaitForSecondsRealtime(1f);
        txt.SetActive(false);

    }
    protected IEnumerator DamageText(GameObject txt, RangedEnemy enemy)
    {
        atkChk = true;
        t = 0;
        txt.SetActive(true);
        txt.GetComponent<TMP_Text>().text = (enemy.MaxHP - enemy.life).ToString();
        yield return new WaitForSecondsRealtime(1f);
        txt.SetActive(false);

    }
    protected IEnumerator DamageText(GameObject txt, Rangedflying enemy)
    {
        atkChk = true;
        t = 0;
        txt.SetActive(true);
        txt.GetComponent<TMP_Text>().text = (enemy.MaxHp - enemy.life).ToString();
        yield return new WaitForSecondsRealtime(1f);
        txt.SetActive(false);

    }
    protected IEnumerator DamageText(GameObject txt, BossEnemy enemy)
    {
        atkChk = true;
        t = 0;
        txt.SetActive(true);
        txt.GetComponent<TMP_Text>().text = (enemy.MaxHP - enemy.life).ToString();
        yield return new WaitForSecondsRealtime(1f);
        txt.SetActive(false);

    }
    protected IEnumerator DamageText(GameObject txt, FlyingBoss enemy)
    {
        atkChk = true;
        t = 0;
        txt.SetActive(true);
        txt.GetComponent<TMP_Text>().text = (enemy.MaxHP - enemy.life).ToString();
        yield return new WaitForSecondsRealtime(1f);
        txt.SetActive(false);

    }
    protected IEnumerator DamageText(GameObject txt, ArrowAngel enemy)
    {
        atkChk = true;
        t = 0;
        txt.SetActive(true);
        txt.GetComponent<TMP_Text>().text = (enemy.MaxHP - enemy.life).ToString();
        yield return new WaitForSecondsRealtime(1f);
        txt.SetActive(false);

    }
    protected IEnumerator DamageText(GameObject txt, TrainingDummy enemy)
    {
        atkChk = true;
        t = 0;
        txt.SetActive(true);
        txt.GetComponent<TMP_Text>().text = (enemy.MaxHP - enemy.life).ToString();
        yield return new WaitForSecondsRealtime(1f);
        txt.SetActive(false);

    }
}
