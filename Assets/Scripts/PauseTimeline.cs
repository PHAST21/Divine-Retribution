using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PauseTimeline : MonoBehaviour
{
    public PlayableDirector playableDirector;
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }
    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().inDialogue)
        {
            PauseDirector();
        }
        else
        {
            ResumeDirector();
        }
    }
    public void PauseDirector()
    {
        playableDirector.Pause();
    }
    public void ResumeDirector()
    {
        playableDirector.Resume();
    }
}
