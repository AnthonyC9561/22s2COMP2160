using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton 
    static private GameManager instance;
    static public GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("No gameManager instance in the scene.");
            }
            return instance;
        }
    }

    [SerializeField] private float sceneChangeDelay = 2f; //delay until scene loading
    //stores last point to spawn player
    private Vector3 lastSpawnPoint;//used for saving checkpoints
    private int currentLevel = 0; //for SceneManager index
    private PlayerMovement player;
    private LogFile log; //for analytics

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        log = gameObject.GetComponent<LogFile>();
        player = FindObjectOfType<PlayerMovement>();
        StartCoroutine(LogStarter());
    }
    private IEnumerator LogStarter()
    {//small delay on game start to run Log.WriteLine()
        yield return null;
        LogStartLevel();
    }
    public void NextLevel()
    {//when player entering and using the teleporter
        LogTeleported();
        StartCoroutine(NewScene());
    }

    private IEnumerator NewScene()
    {//scenes restart from first one after all scenes in the build accessed.
        yield return new WaitForSeconds(sceneChangeDelay);

        if (currentLevel == SceneManager.sceneCountInBuildSettings - 1)
        {
            currentLevel %= SceneManager.sceneCountInBuildSettings - 1;
        }
        else
        {
            currentLevel++;
        }
        SceneManager.LoadScene(currentLevel);

        yield return null; //adds delay to ensure object is obtained after scene reload.
        player = FindObjectOfType<PlayerMovement>();
        lastSpawnPoint = player.transform.position;
        LogStartLevel();
    }

    public void Die()
    {//when player dies
        LogDie();
        StartCoroutine(ResetScene());
    }

    private IEnumerator ResetScene()
    {//need to check interaction with checkpoint
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(sceneChangeDelay);
        SceneManager.LoadScene(currentLevel);

        yield return null; //a frame delay to allow player object to be found and translated to last checkpoint
        player = FindObjectOfType<PlayerMovement>();
        player.transform.position = lastSpawnPoint;
    }

    public void ReachedCheckpoint(Vector3 checkpoint)
    {
        if (checkpoint != lastSpawnPoint)
        {
            LogCheckPoint();
            lastSpawnPoint = checkpoint;
        }
    }

    private void LogStartLevel()
    {
        if (log != null)
        {
            log.WriteLine("Player started a level", Time.time,
                SceneManager.GetSceneByBuildIndex(currentLevel).name, player.transform.position);
        }
    }

    private void LogDie()
    {
        if (log != null)
        {
            log.WriteLine("Player Died", Time.time,
                SceneManager.GetSceneByBuildIndex(currentLevel).name, player.transform.position);
        }
    }

    private void LogCheckPoint()
    {
        if (log != null)
        {
            log.WriteLine("New checkpoint reached", Time.time,
                SceneManager.GetSceneByBuildIndex(currentLevel).name, player.transform.position);
        }
    }

    private void LogTeleported()
    {
        if (log != null)
        {
            log.WriteLine("Teleporter used", Time.time,
                SceneManager.GetSceneByBuildIndex(currentLevel).name, player.transform.position);
        }
    }

}
