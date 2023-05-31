using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewLoadScene : MonoBehaviour
{

    public GameObject trig;
    private GameManager GM;
    [SerializeField] private string i ="w";
    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GM.ResetRespawns();
        GM.LoadNewScene(i);
    }
}
