using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeController : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabMazeCell;

    private MazeCell[,] maze;

    public void GenerateMaze(int mazeWidth, int mazeHeight)
    {
        // Create all MazeCells objects
        maze = new MazeCell[mazeWidth, mazeHeight];
        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeHeight; j++)
            {
                Vector3 mazeCellPosition = MazeCordToWorldCord(i, j);
                GameObject gameObject = Instantiate(
                    prefabMazeCell,
                    mazeCellPosition,
                    new Quaternion()
                );
                gameObject.name = "MazeCell (" + i + "," + j + ")";

                gameObject.transform.SetParent(transform);

                maze[i, j] = gameObject.GetComponent<MazeCell>();
            }
        }

        StartCoroutine(GenerateMazeCoroutine());
    }

    private IEnumerator GenerateMazeCoroutine()
    {
        bool[,] boolMaze = new bool[maze.GetLength(0), maze.GetLength(1)];
        bool isFirstRun = true;
        while (true)
        {
            yield return new WaitForSeconds(GameManager.instance.GetMapGenerationSpeed());

            // Find the first open spot
            (int x_c, int y_c) = FindOpenMazeCell(boolMaze);

            // base condition:
            // no open stops found algorithm finishes
            if (x_c == -1 && y_c == -1)
            {
                break;
            }

            // Debug.Log("Test1: Current: " + x_c + ", " + y_c);
            maze[x_c, y_c].SetWallColor(Color.red);

            if (!isFirstRun)
            {
                List<(int x, int y)> closedMazeCells = FindAdjacentClosedMazeCells(boolMaze, x_c, y_c);
                int closedCellIndex = Random.Range(0, closedMazeCells.Count);
                (int x_o, int y_o) = closedMazeCells[closedCellIndex];
                OpenWalls(x_c, y_c, x_o, y_o);
            }

            List<(int x, int y)> openMazeCells = FindAdjacentOpenMazeCells(boolMaze, x_c, y_c);
            while (openMazeCells.Count != 0)
            {
                yield return new WaitForSeconds(GameManager.instance.GetMapGenerationSpeed());

                int nextMazeCellIndex = Random.Range(0, openMazeCells.Count);

                (int x_n, int y_n) = openMazeCells[nextMazeCellIndex];

                OpenWalls(x_c, y_c, x_n, y_n);

                boolMaze[x_c, y_c] = true;
                maze[x_c, y_c].SetWallColor(Color.gray);

                x_c = x_n;
                y_c = y_n;
                maze[x_c, y_c].SetWallColor(Color.red);
                openMazeCells = FindAdjacentOpenMazeCells(boolMaze, x_c, y_c);
            }

            boolMaze[x_c, y_c] = true;
            maze[x_c, y_c].SetWallColor(Color.gray);
            isFirstRun = false;
        }
    }

    private const float xOffset = -4.5f;
    private const float yOffset = 0f;
    private const float zOffset = 4.5f;
    private Vector3 MazeCordToWorldCord(int x, int y)
    {
        return new Vector3(
            (xOffset * (maze.GetLength(0) / GameManager.instance.GetMapScale()) + x),
            (yOffset),
            (zOffset * (maze.GetLength(1) / GameManager.instance.GetMapScale()) - y)
        );
    }

    private void OpenWalls(int x_c, int y_c, int x_n, int y_n)
    {
        MazeCell currentMazeCell = maze[x_c, y_c];
        MazeCell nextMazeCell = maze[x_n, y_n];

        (int x_t, int y_t) = (x_c - x_n, y_c - y_n);

        if (x_t == 0 && y_t == 1)
        {
            currentMazeCell.DisableNorthWall();
            nextMazeCell.DisableSouthWall();
        }
        if (x_t == 0 && y_t == -1)
        {
            currentMazeCell.DisableSouthWall();
            nextMazeCell.DisableNorthWall();
        }
        if (x_t == 1 && y_t == 0)
        {
            currentMazeCell.DisableEastWall();
            nextMazeCell.DisableWestWall();
        }
        if (x_t == -1 && y_t == 0)
        {
            currentMazeCell.DisableWestWall();
            nextMazeCell.DisableEastWall();
        }
    }

    private (int x, int y) FindOpenMazeCell(bool[,] boolMaze)
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (!boolMaze[i, j])
                {
                    return (i, j);
                }
            }
        }

        return (-1, -1);
    }

    private List<(int x, int y)> FindAdjacentOpenMazeCells(bool[,] boolMaze, int x, int y)
    {
        List<(int x, int y)> openMazeCell = new List<(int x, int y)>();

        (int x, int y)[] positions = new (int x, int y)[4]
        {
            (0, 1),
            (1, 0),
            (0, -1),
            (-1, 0),
        };

        for (int i = 0; i < positions.Length; i++)
        {
            int x_p = x + positions[i].x;
            int y_p = y + positions[i].y;

            if ((x_p >= 0 && x_p < boolMaze.GetLength(0))
                &&
                (y_p >= 0 && y_p < boolMaze.GetLength(1))
                &&
                !boolMaze[x_p, y_p])
            {
                openMazeCell.Add((x_p, y_p));
            }
        }

        return openMazeCell;
    }

    private List<(int x, int y)> FindAdjacentClosedMazeCells(bool[,] boolMaze, int x, int y)
    {
        List<(int x, int y)> openMazeCell = new List<(int x, int y)>();

        (int x, int y)[] positions = new (int x, int y)[4]
        {
            (0, 1),
            (1, 0),
            (0, -1),
            (-1, 0),
        };

        for (int i = 0; i < positions.Length; i++)
        {
            int x_p = x + positions[i].x;
            int y_p = y + positions[i].y;

            if ((x_p >= 0 && x_p < boolMaze.GetLength(0))
                &&
                (y_p >= 0 && y_p < boolMaze.GetLength(1))
                &&
                boolMaze[x_p, y_p])
            {
                openMazeCell.Add((x_p, y_p));
            }
        }

        return openMazeCell;
    }
}
