using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* Singleton */
    public static GameManager instance;

    /* Prefabs */
    [SerializeField]
    private GameObject prefabPlayer;
    [SerializeField]
    private GameObject prefabGoal;
    [SerializeField]
    private GameObject prefabEnemy;

    /* Game Fields */
    [SerializeField]
    private int mazeWidth = 1;
    [SerializeField]
    private int mazeHeight = 1;
    [SerializeField]
    private float mapGenerationSpeed = .25f;

    /* GameObject instances */
    [SerializeField]
    private GameObject mazeControllerGameObject;
    public GameObject playerGameObject;
    private GameObject goalGameObject;
    private GameObject enemyGameObject;
    [SerializeField]
    private GameObject setupCamera;
    public bool setupComplete = false;

    /* Component instances */
    [SerializeField]
    private UIController uIController;
    public MazeController mazeController;
    private Player player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        mazeControllerGameObject.transform.localScale = new Vector3(
            mazeWidth / 10f,
            1,
            mazeHeight / 10f
        );

        mazeController = mazeControllerGameObject.GetComponent<MazeController>();
        mazeController.GenerateMaze(mazeWidth, mazeHeight);
    }

    public void CreatePlayer() {
        Vector3 playerPosition = mazeController.MazeCordToWorldCord(Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)) + new Vector3(0,.125f,0);

        playerGameObject = Instantiate(prefabPlayer, playerPosition, new Quaternion());
        player = playerGameObject.GetComponent<Player>();

        setupCamera.SetActive(false);
        player.ActivateThirdPersonCamera();

        Vector3 goalPosition = mazeController.MazeCordToWorldCord(Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)) + new Vector3(0, .4f, 0);
        goalGameObject = Instantiate(prefabGoal, goalPosition, new Quaternion());

        Vector3 enemyPosition = mazeController.MazeCordToWorldCord(Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)) + new Vector3(0, .125f, 0);
        enemyGameObject = Instantiate(prefabEnemy, enemyPosition, new Quaternion());

        setupComplete = true;
    }

    /* Getter */
    public float GetMapGenerationSpeed()
    {
        return mapGenerationSpeed;
    }

    /* Other */
    public void WinGame() {
        uIController.DisplayWinText();
    }
}
