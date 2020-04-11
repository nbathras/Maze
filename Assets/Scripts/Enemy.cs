using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int detectionDistance = 5;

    [SerializeField]
    private float locomationAnimationSmoothTime = .1f;

    [SerializeField]
    private float movementSpeed = 2;

    private Animator animator;

    private List<MazeCell> path;
    private int pathIndex = 1;

    private void Awake() {
        StartCoroutine(TrackCoroutine());

        animator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        if (GameManager.instance.setupComplete && path != null) {
            // The player character is to far away to bet detected
            if (path.Count > detectionDistance)
            {
                return;
            }

            Vector3 currentPosition = transform.position;
            Vector3 targetPosition;

            // naviage to the next mazecell which does not contain the player
            if (pathIndex < path.Count - 1) {
                targetPosition = path[pathIndex].transform.position + new Vector3(0, .125f, 0);
            // navigate to the player in the mazecell
            } else {
                targetPosition = GameManager.instance.playerGameObject.transform.position + new Vector3(0, .125f, 0);
            }

            // Look at the target
            transform.LookAt(targetPosition);

            // If close enough stop moving and stop playign the runnig animation
            if (Vector3.Magnitude(currentPosition - targetPosition) < 0.3f) {
                if (pathIndex < path.Count) {
                    pathIndex += 1;
                }
                animator.SetFloat("speedPercent", 0f, locomationAnimationSmoothTime, Time.deltaTime);
            // If not close enough keep moving towards target well playing the running animation
            } else {
                animator.SetFloat("speedPercent", 1f, locomationAnimationSmoothTime, Time.deltaTime);
                float step = movementSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(currentPosition, targetPosition, step);
            }
        }
    }

    public void CalculatePath() {
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

            /*
            string pathString = "";
            foreach (MazeCell mazeCell in path) {
                pathString += mazeCell.name + " => ";
            }
            Debug.Log(pathString);
            */
        }
    }

    private IEnumerator TrackCoroutine() {

        while(true) {
            yield return new WaitForSeconds(.1f);

            CalculatePath();
        }
    }
}
