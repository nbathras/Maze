using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* Singleton */
    public static GameManager instance;

    /* Prefabs */
    [SerializeField]
    private GameObject mazeControllerGameObject;

    /* Game Fields */
    [SerializeField]
    private int mazeWidth = 1;
    [SerializeField]
    private int mazeHeight = 1;
    [SerializeField]
    private int mapScale = 10;
    [SerializeField]
    private float mapGenerationSpeed = .25f;
    
    /* Controller instances */
    private MazeController mazeController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        mazeControllerGameObject.transform.localScale = new Vector3(
            mazeWidth / GetMapScale(),
            1,
            mazeHeight / GetMapScale()
        );

        mazeController = mazeControllerGameObject.GetComponent<MazeController>();
        mazeController.GenerateMaze(mazeWidth, mazeHeight);
    }

    /* Getter */
    public float GetMapScale()
    {
        return mapScale;
    }

    public float GetMapGenerationSpeed()
    {
        return mapGenerationSpeed;
    }
}
