using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /* GameObject instances */
    [SerializeField]
    private GameObject thirdPersonCamera;
    [SerializeField]
    private GameObject firstPersonCamera;
    [SerializeField]
    private GameObject topDownCamera;

    /* Player Attributes */
    [SerializeField]
    private float movementSpeed = 2;
    [SerializeField]
    private float speedH = 4;

    private float yaw;
    private Vector3 movement;
    private Rigidbody rb;
    private List<GameObject> cameras;
    private int camerasIndex;

    public float MaxVelocity {
        get {
            return movementSpeed;
        }
    }
    public float CurrentVelocity {
        get {
            return movement.magnitude * movementSpeed;
        }
    }

    /* Unity Methods */
    private void Start() {
        rb = GetComponent<Rigidbody>();
        yaw = 0.0f;

        cameras = new List<GameObject> {
            thirdPersonCamera,
            firstPersonCamera,
            topDownCamera
        };

        camerasIndex = 0;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            camerasIndex = (camerasIndex + 1) % cameras.Count;
            ActivateCamera(camerasIndex);
        }
    }

    private void FixedUpdate() {
        float forwardMovement = Input.GetAxis("Vertical");

        movement = new Vector3(0, 0, forwardMovement);

        rb.transform.Translate(movement * movementSpeed * Time.deltaTime);

        yaw += speedH * Input.GetAxis("Mouse X");

        transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
    }

    private void OnTriggerEnter(Collider other) {
        GameManager.instance.WinGame();
    }

    /* Activate Camera Methods */
    public void ActivateCamera(int index) {
        for (int i = 0; i < cameras.Count; i++) {
            cameras[i].SetActive(i == index);
        }
    }

    public void ActivateThirdPersonCamera() {
        thirdPersonCamera.SetActive(true);
        firstPersonCamera.SetActive(false);
        topDownCamera.SetActive(false);
    }

    public void ActivateFirstPersonCamera() {
        thirdPersonCamera.SetActive(false);
        firstPersonCamera.SetActive(true);
        topDownCamera.SetActive(false);
    }

    public void ActivateTopDownCamera() {
        thirdPersonCamera.SetActive(false);
        firstPersonCamera.SetActive(false);
        topDownCamera.SetActive(true);
    }
}
