using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager DManager;
    public int StartPos, EndPos;
    private void Start()
    {
        DManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<DialogueManager>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DManager.SetStartPoint(StartPos);
            DManager.SetEndPoint(EndPos);
            DManager.StartDiag();
            Destroy(gameObject);
        }
    }

}
