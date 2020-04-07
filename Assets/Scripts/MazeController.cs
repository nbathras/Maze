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
                // gameObject.transform.SetParent(transform);

                maze[i, j] = gameObject.GetComponent<MazeCell>();
                maze[i, j].SetWallColor(Color.green);
                maze[i, j].SetName(i, j);
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

            // Find the first open spot
            MazeCell current = FindOpenMazeCell(boolMaze);

            // base condition:
            // no open stops found algorithm finishes
            if (current == null)
            {
                break;
            }

            // Debug.Log("Test1: Current: " + x_c + ", " + y_c);
            current.SetWallColor(Color.red);

            yield return new WaitForSeconds(GameManager.instance.GetMapGenerationSpeed());

            if (!isFirstRun)
            {
                List<MazeCell> closedMazeCells = FindAdjacentClosedMazeCells(boolMaze, current);
                int closedCellIndex = Random.Range(0, closedMazeCells.Count);
                MazeCell next = closedMazeCells[closedCellIndex];
                MazeCell.ConnectMazeCells(current, next);
            }

            List<MazeCell> openMazeCells = FindAdjacentOpenMazeCells(boolMaze, current);
            while (openMazeCells.Count != 0)
            {
                yield return new WaitForSeconds(GameManager.instance.GetMapGenerationSpeed());

                int nextMazeCellIndex = Random.Range(0, openMazeCells.Count);

                MazeCell next = openMazeCells[nextMazeCellIndex];

                MazeCell.ConnectMazeCells(current, next);

                boolMaze[current.GetX(), current.GetY()] = true;
                current.SetWallColor(Color.gray);

                current = next;
                current.SetWallColor(Color.red);
                openMazeCells = FindAdjacentOpenMazeCells(boolMaze, current);
            }

            boolMaze[current.GetX(), current.GetY()] = true;
            current.DisableCenterWall();
            current.SetWallColor(Color.gray);
            isFirstRun = false;
        }

        GameManager.instance.CreatePlayer();
    }

    public MazeCell GetMazeCell(int x, int y) {
        return maze[x, y];
    }

    private MazeCell FindOpenMazeCell(bool[,] boolMaze)
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (!boolMaze[i, j])
                {
                    return maze[i, j];
                }
            }
        }

        return null;
    }

    private List<MazeCell> FindAdjacentOpenMazeCells(bool[,] boolMaze, MazeCell current)
    {
        List<MazeCell> openMazeCell = new List<MazeCell>();

        (int x, int y)[] positions = new (int x, int y)[4]
        {
            (0, 1),
            (1, 0),
            (0, -1),
            (-1, 0),
        };

        for (int i = 0; i < positions.Length; i++)
        {
            int x_p = current.GetX() + positions[i].x;
            int y_p = current.GetY() + positions[i].y;

            if ((x_p >= 0 && x_p < boolMaze.GetLength(0))
                &&
                (y_p >= 0 && y_p < boolMaze.GetLength(1))
                &&
                !boolMaze[x_p, y_p])
            {
                openMazeCell.Add(maze[x_p, y_p]);
            }
        }

        return openMazeCell;
    }

    private List<MazeCell> FindAdjacentClosedMazeCells(bool[,] boolMaze, MazeCell current)
    {
        List<MazeCell> closedMazeCell = new List<MazeCell>();

        (int x, int y)[] positions = new (int x, int y)[4]
        {
            (0, 1),
            (1, 0),
            (0, -1),
            (-1, 0),
        };

        for (int i = 0; i < positions.Length; i++)
        {
            int x_p = current.GetX() + positions[i].x;
            int y_p = current.GetY() + positions[i].y;

            if ((x_p >= 0 && x_p < boolMaze.GetLength(0))
                &&
                (y_p >= 0 && y_p < boolMaze.GetLength(1))
                &&
                boolMaze[x_p, y_p])
            {
                closedMazeCell.Add(maze[x_p, y_p]);
            }
        }

        return closedMazeCell;
    }

    private const float offset = .5f;
    public Vector3 MazeCordToWorldCord(int x, int y)
    {
        return new Vector3(
            -maze.GetLength(0) / 2f + offset + x,
            0,
            maze.GetLength(1) / 2f - offset - y
        );
    }

    public (int x, int y) WorldCordToMazeCord(Vector3 worldCord) {
        float w_x = worldCord.x;
        float w_z = worldCord.z;

        return
        (
            Mathf.RoundToInt(w_x - (-maze.GetLength(0) / 2f + offset)),
            Mathf.RoundToInt((maze.GetLength(1) / 2f - offset) - w_z)
        );
    }
}
