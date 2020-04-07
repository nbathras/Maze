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
    private GameObject playerGameObject;
    private GameObject goalGameObject;
    private GameObject enemyGameObject;
    [SerializeField]
    private GameObject setupCamera;

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

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            RunAStar();
        }
    }

    private void RunAStar() {
        if (playerGameObject != null && enemyGameObject != null) {
            (int x, int y) playerCord = mazeController.WorldCordToMazeCord(playerGameObject.transform.position);
            (int x, int y) enemyCord = mazeController.WorldCordToMazeCord(enemyGameObject.transform.position);

            List<MazeCell> path = AStar.RunAStar(
                mazeController.GetMazeCell(enemyCord.x, enemyCord.y),
                mazeController.GetMazeCell(playerCord.x, playerCord.y)
            );

            string pathString = "";
            foreach (MazeCell mazeCell in path) {
                pathString += mazeCell.name + " => ";
            }
            Debug.Log(pathString);

            enemyGameObject.transform.Translate(path[1].transform.position - enemyGameObject.transform.position);
        }
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
