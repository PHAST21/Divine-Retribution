using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HPtxt : MonoBehaviour
{
    public GameObject txt;
    public CharacterController2D characterController;
    public GameObject player;
     // Start is called before the first frame update
    void Awake()
    {
        characterController = player.GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        txt.GetComponent<TMP_Text>().text = characterController.life.ToString(); 
    }
}
