using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutStart : MonoBehaviour
{
    private DialogueManager DManager;
    public int StartPos, EndPos;
    // Start is called before the first frame update
    void Start()
    {
        DManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<DialogueManager>();
        StartCoroutine(StartWait());
    }

    // Update is called once per frame
    IEnumerator StartWait()
    {
        yield return new WaitForSeconds(0.8f);
        DManager.SetStartPoint(StartPos);
        DManager.SetEndPoint(EndPos);
        DManager.StartDiag();
    }
}
