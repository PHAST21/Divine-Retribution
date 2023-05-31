using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dialogue : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public GameObject nextArrow;
    private Image nextArrowSprite;
    public string[] Lines;
    private CharacterController2D player;
    [Header("Text Attributes")]
    [SerializeField]private int index;
    [SerializeField]private int EndPos;
    public float txtSpeed;
    void Start()
    {
        /*txt.text = string.Empty;*/
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        nextArrowSprite = nextArrow.GetComponent<Image>();
        /*txt.text = "Press J or Left Click to move through Text";*/
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)||Input.GetMouseButtonDown(0))
        {
            if (txt.text == Lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                txt.text = Lines[index];
            }
        }
        if (txt.text == Lines[index])
        {
            nextArrow.SetActive(true);
        }
        else
        {
            nextArrow.SetActive(false);
        }
    }
    public void StartDialogue(int startPos)
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        }
        player.GetComponent<CharacterController2D>().inDialogue=true;
        index = startPos;
        InvokeRepeating("Blink", 0, 1f);
        StartCoroutine(TypeLine());
    }
    public void SetEndPoint(int endPos)
    {
        EndPos = endPos;
    }
    IEnumerator TypeLine()
    {
        foreach (char c in Lines[index].ToCharArray())
        {
            txt.text += c;
            yield return new WaitForSeconds(txtSpeed);
        }
    }
    void NextLine()
    {
        if (index < Lines.Length - 1&&index<EndPos)
        {
            index++;
            txt.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            player.GetComponent<CharacterController2D>().inDialogue = false;
            CancelInvoke();
            gameObject.SetActive(false);
        }
    }
    void Blink()
    {
        StartCoroutine(Blinking());
    }
    IEnumerator Blinking()
    {
        nextArrowSprite.color = Color.clear;
        yield return new WaitForSeconds(0.2f);
        nextArrowSprite.color = Color.white;
    }

}
