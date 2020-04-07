using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private List<MazeCell> connections;

    private (int x, int y) cord;

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

    [SerializeField]
    private GameObject centerWall;
    private Renderer centerWallRenderer;

    /* Unity Methods */
    private void Awake()
    {
        northWallRenderer  = northWall.GetComponent<Renderer>();
        southWallRenderer  = southWall.GetComponent<Renderer>();
        eastWallRenderer   = eastWall.GetComponent<Renderer>();
        westWallRenderer   = westWall.GetComponent<Renderer>();
        centerWallRenderer = centerWall.GetComponent<Renderer>();

        connections = new List<MazeCell>();
    }

    /* Getters */
    public int GetX() {
        return cord.x;
    }
    public int GetY() {
        return cord.y;
    }
    public List<MazeCell> getConnections() {
        return connections;
    }

    /* Setters */
    public void SetName(int i_x, int i_y) {
        gameObject.name = "MazeCell (" + i_x + "," + i_y + ")";
        cord.x = i_x;
        cord.y = i_y;
    }

    /* Disable Wall Methods */
    public void DisableCenterWall() {
        centerWall.SetActive(false);
    }

    /* Set Color Method */
    public void SetWallColor(Color color)
    {
        northWallRenderer.material.color  = color;
        southWallRenderer.material.color  = color;
        eastWallRenderer.material.color   = color;
        westWallRenderer.material.color   = color;
        centerWallRenderer.material.color = color;
    }

    /* Overrides */
    public override bool Equals(object other) {
        MazeCell otherMazeCell = (MazeCell)other;
        if (this.cord.x == otherMazeCell.cord.x && this.cord.y == otherMazeCell.cord.y) {
            return true;
        } else {
            return false;
        }
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    /* Static Methods */
    public static void ConnectMazeCells(MazeCell current, MazeCell next) {
        (int x_t, int y_t) = (current.GetX() - next.GetX(), current.GetY() - next.GetY());

        current.DisableCenterWall();
        if (x_t == 0 && y_t == 1) {
            current.northWall.SetActive(false);
            next.southWall.SetActive(false);
            AddConnection();
        }
        if (x_t == 0 && y_t == -1) {
            current.southWall.SetActive(false);
            next.northWall.SetActive(false);
            AddConnection();
        }
        if (x_t == 1 && y_t == 0) {
            current.eastWall.SetActive(false);
            next.westWall.SetActive(false);
            AddConnection();
        }
        if (x_t == -1 && y_t == 0) {
            current.westWall.SetActive(false);
            next.eastWall.SetActive(false);
            AddConnection();
        }

        void AddConnection() {
            current.connections.Add(next);
            next.connections.Add(current);
        }
    }
}
