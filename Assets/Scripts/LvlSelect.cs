using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlSelect : MonoBehaviour
{
    public GameObject LvlSelectMenu;

    private void Start()
    {
        LvlSelectMenu.SetActive(false);
    }
    public void OpenLevelSelect()
    {
        LvlSelectMenu.SetActive(true);
    }

  
}
