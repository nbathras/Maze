using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 2;

    private List<MazeCell> path;
    private int pathIndex = 1;

    private void Awake() {
        StartCoroutine(TrackCoroutine());
    }

    private void Update() {
        /*
        if (GameManager.instance.setupComplete) {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition;

            if (path != null && pathIndex < path.Count - 1) {
                targetPosition = path[pathIndex].transform.position + new Vector3(0, .125f, 0); ;
            } else {
                targetPosition = GameManager.instance.playerGameObject.transform.position;
            }

            if (Vector3.Magnitude(currentPosition - targetPosition) < 0.3f) {
                if (path != null && pathIndex < path.Count) {
                    pathIndex += 1;
                }
            } else {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(currentPosition, targetPosition, step);
            }
        }
        */
    }

    private IEnumerator TrackCoroutine() {

        while(true) {
            yield return new WaitForSeconds(1f);

            if (GameManager.instance.setupComplete) {
                MazeController mazeController = GameManager.instance.mazeController;
                GameObject playerGameObject = GameManager.instance.playerGameObject;

                (int x, int y) playerCord = mazeController.WorldCordToMazeCord(playerGameObject.transform.position);
                (int x, int y) enemyCord = mazeController.WorldCordToMazeCord(gameObject.transform.position);

                path = AStar.RunAStar(
                    mazeController.GetMazeCell(enemyCord.x, enemyCord.y),
                    mazeController.GetMazeCell(playerCord.x, playerCord.y)
                );
                pathIndex = 1;

                string pathString = "";
                foreach (MazeCell mazeCell in path) {
                    pathString += mazeCell.name + " => ";
                }
                // Debug.Log(pathString);
            }
        }
    }
}
