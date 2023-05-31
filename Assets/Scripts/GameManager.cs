using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

//To be attached to an object inside the first scene
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameManager instance=null;
    [SerializeField]private CharacterController2D player;
    public bool boss1Defeated = false;
    public List<bool> abilities;
    public List<bool> Hupgrades;
    [SerializeField] public int MaxHealth=0;
    /*[SerializeField] private bool InMenu=true;*/
    public bool healChange = false;
    public float HealthbarScale;

    [SerializeField] private Vector3 InitialCheckpoint; 
    public Vector3 CheckpointPos;

    private void Awake()
    {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        if (GameObject.FindGameObjectsWithTag("GM").Length > 1)
        {
            for (int i = 1; i < GameObject.FindGameObjectsWithTag("GM").Length; i++)
            {
                Destroy(GameObject.FindGameObjectsWithTag("GM")[i]);
            }
        }

    }
    private void Start()
    {
        /*player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();*/
        /*player.SetRespawnPoint(player.transform.position);*/
        /*MaxHealth = player.maxHp;*/
        ResetRespawns();
        
    }
    private void Update()
    {
        if (player == null && SceneManager.GetActiveScene().buildIndex != 0)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        }
        /*else if (GameObject.FindGameObjectWithTag("NotMenu").activeSelf)
        {
            InMenu = false;
        }

        else if(GameObject.FindGameObjectWithTag("Menu").activeSelf)
        {
            InMenu = true;
        }*/
        
        if (SceneManager.GetActiveScene().buildIndex==0)
        {
            Cursor.visible = true;
        }
    }
    private void FixedUpdate()
    {
        if (/*!InMenu*//* && */player != null && healChange)
        {
            MaxHealth = player.maxHp;
        }
    }
    public void ResetRespawns()
    {
        CheckpointPos = new Vector3(0,0,0);
    }
    public void PlayerRespawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
    public void NewGame()
    {
        MaxHealth = 10;
        ResetRespawns();
        boss1Defeated = false;
       for(int i=0; i > abilities.Count; i++)
        {
            abilities[i] = false;
        }
        for(int i =0; i < Hupgrades.Count; i++)
        {
            Hupgrades[i] = false;
        }
    }
    public void LoadNewScene(string s)
    {
        SceneManager.LoadSceneAsync(s);
        ResetRespawns();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
    }

    public void LoadNewScenewithPos(string s, Vector3 position)
    {
        SceneManager.LoadSceneAsync(s);
        CheckpointPos = position;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
    }
}
