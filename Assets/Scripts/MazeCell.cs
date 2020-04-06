using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject northWall;
    private Renderer northWallRenderer;
    [SerializeField]
    private GameObject southWall;
    private Renderer southWallRenderer;
    [SerializeField]
    private GameObject eastWall;
    private Renderer eastWallRenderer;
    [SerializeField]
    private GameObject westWall;
    private Renderer westWallRenderer;

    /* Unity Methods */
    private void Awake()
    {
        northWallRenderer = northWall.GetComponent<Renderer>();
        southWallRenderer = southWall.GetComponent<Renderer>();
        eastWallRenderer  = eastWall.GetComponent<Renderer>();
        westWallRenderer  = westWall.GetComponent<Renderer>();
    }

    /* Disable Wall Methods */
    public void DisableNorthWall()
    {
        northWall.SetActive(false);
    }

    public void DisableSouthWall()
    {
        southWall.SetActive(false);
    }

    public void DisableEastWall()
    {
        eastWall.SetActive(false);
    }

    public void DisableWestWall()
    {
        westWall.SetActive(false);
    }

    /* Set Color Method */
    public void SetWallColor(Color color)
    {
        northWallRenderer.material.color = color;
        southWallRenderer.material.color = color;
        eastWallRenderer.material.color  = color;
        westWallRenderer.material.color  = color;
    }
}
