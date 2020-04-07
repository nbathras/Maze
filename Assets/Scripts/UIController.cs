using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject winText;

    private void Awake() {
        winText.SetActive(false);
    }

    public void DisplayWinText() {
        winText.SetActive(true);
    }
}
