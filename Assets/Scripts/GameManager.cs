using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* Singleton */
    public static GameManager instance;

    /* Prefabs */
    [SerializeField]
    private GameObject prefabPlayer;
    [SerializeField]
    private GameObject prefGoal;

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

    public void CreatePlayer() {
        Vector3 playerPosition = mazeController.MazeCordToWorldCord(Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)) + new Vector3(0,.125f,0);

        playerGameObject = Instantiate(prefabPlayer, playerPosition, new Quaternion());
        player = playerGameObject.GetComponent<Player>();

        setupCamera.SetActive(false);
        player.ActivateThirdPersonCamera();

        Vector3 goalPosition = mazeController.MazeCordToWorldCord(Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)) + new Vector3(0, .4f, 0);

        goalGameObject = Instantiate(prefGoal, goalPosition, new Quaternion());
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
