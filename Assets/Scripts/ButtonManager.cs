using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void CloseInfo(GameObject a)
    {
        a.SetActive(false);
    }
    public void OpenInfo(GameObject a)
    {
        a.SetActive(true);
    }
    public void LoadNewScene(string s)
    {
        SceneManager.LoadSceneAsync(s);
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>().ResetRespawns();
    }
    public void LoadNewScenewithPos()
    {
        SceneManager.LoadSceneAsync("Lvl 1(DesignTest)");
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>().ResetRespawns();
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>().CheckpointPos = new Vector3(32f,112f,12.03f);
    }
    public void NewGame()
    {
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>().NewGame();
        SceneManager.LoadSceneAsync(1);
    }
}
