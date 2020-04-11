using UnityEngine;
using UnityEngine.UI;

public class FrameRateTracker : MonoBehaviour
{
    [SerializeField]
    private int targetFrameRate = 60;

    [SerializeField]
    private Text frameRateText;

    private const string frameRateString = "Framerate: ";

    private float frameCount  = 0.0f;
    private float nextUpdate = 0.0f;
    private float fps        = 0.0f;
    private float updateRate = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = targetFrameRate;

        frameRateText.gameObject.SetActive(true);

        nextUpdate = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        if (Time.time > nextUpdate)
        {
            nextUpdate += 1.0f / updateRate;
            fps = frameCount * updateRate;

            frameRateText.text = frameRateString + ((int)fps).ToString();

            frameCount = 0;
        }
    }
}
