using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    private CharacterController2D Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    public void ButtonPress()
    {
        StartCoroutine(Player.PauseDisable());
    }
}
