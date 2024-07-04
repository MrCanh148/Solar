using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;

    private float timer;
    private float refreshTime = 1f;
    private float currentFPS;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= refreshTime)
        {
            currentFPS = 1.0f / Time.deltaTime;
            fpsText.text = "FPS: " + Mathf.Ceil(currentFPS).ToString();
            timer = 0f;
        }
    }
}
