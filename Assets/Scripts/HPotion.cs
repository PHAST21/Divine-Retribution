using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPotion : MonoBehaviour
{
    public List<GameObject> Hp;
    private Attack atk;
    // Start is called before the first frame update
    void Start()
    {
        atk = GameObject.FindGameObjectWithTag("Player").GetComponent<Attack>();
    

    }

    // Update is called once per frame
    public void HealUse(int a)
    {
            Hp[a].SetActive(false);
        
    }
    public void HealReset()
    {
        for(int i = 0; i < atk.maxHeal; i++)
        {
            Hp[i].SetActive(true);
        }
    }
}
