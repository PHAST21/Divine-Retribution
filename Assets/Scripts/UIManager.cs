using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image fadeImg;
    public GameObject menuImg;
    private CharacterController2D player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        FadeOut(1.5f);
    }
    public void FadeOut(float t)
    {
        fadeImg.CrossFadeAlpha(0, t, false);
    }

    public void FadeIn(float t)
    {
        fadeImg.CrossFadeAlpha(1,t, false);
    }
    void Update()
    {
        if (player.life <= 0)
        {
            FadeIn(1.5f);
        }
        if(!player.isPaused)
        {
            menuImg.SetActive(false);
            player.PauseDisable();
        }
    }

    public void EnablePause()
    {
         menuImg.SetActive(true);
    }
}
