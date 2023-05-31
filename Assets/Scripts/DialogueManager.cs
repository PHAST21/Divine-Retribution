using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    private dialogue diag;
    private int startPos = 0, EndPos = 3;
    public bool PlayDiagBool = false;
    void Start()
    {
        dialogueBox.SetActive(true);
        diag = dialogueBox.GetComponent<dialogue>();
        dialogueBox.SetActive(false);
    }
    private void Update()
    {
/*        if (PlayDiagBool)
        {
            dialogueBox.SetActive(true);
            diag.txt.text = string.Empty;
            StartDialogue();
            PlayDiagBool = false;
        }*/
    }
    public void SetStartPoint(int SPoint)
    {
        startPos = SPoint;
    }
    public void SetEndPoint(int EPoint)
    {
        EndPos = EPoint;
    }
    public void StartDiag()
    {
        dialogueBox.SetActive(true);
        diag.txt.text = string.Empty;
        Dialogue();
    }
    public void Dialogue()
    {
        diag.SetEndPoint(EndPos);
        diag.StartDialogue(startPos);
        
    }

}
