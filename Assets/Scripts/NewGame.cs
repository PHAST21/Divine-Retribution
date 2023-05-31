using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    public GameObject Info;
    private void Start()
    {
        Info.SetActive(false); 
    }
    public void OpenInfo()
    {
        Info.SetActive(true);
    }
    public void CloseInfo()
    {
        Info.SetActive(false);
    }
}
