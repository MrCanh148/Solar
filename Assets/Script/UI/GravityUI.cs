using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GravityUI : MonoBehaviour
{
    [SerializeField] private Status status;
    [SerializeField] private TextMeshProUGUI AcceleText;
    [SerializeField] private TextMeshProUGUI DeceleText;
    [SerializeField] private TextMeshProUGUI MinMergeForceText;
    [SerializeField] private Slider AcceleSlide;
    [SerializeField] private Slider DeceleSlide;
    [SerializeField] private Slider MinMergeForceSlide;

    private void Start()
    {
        AcceleSlide.value = status.acceleration / 10f;
        DeceleSlide.value = status.deceleration / 10f;
        MinMergeForceSlide.value = status.minimumMergeForce / 10f;

        AcceleSlide.onValueChanged.AddListener(OnAcceleSliderValueChanged);
        DeceleSlide.onValueChanged.AddListener(OnDeceleSliderValueChanged);
        MinMergeForceSlide.onValueChanged.AddListener(OnMinMergeForceSliderValueChanged);
    }

    private void Update()
    {
        AcceleText.text = "acceleration " + "[" + status.acceleration.ToString("F2") + "]";
        DeceleText.text = "deceleration " + "[" + status.deceleration.ToString("F2") + "]";
        MinMergeForceText.text = "min merge force " + "[" + status.minimumMergeForce.ToString("F1") + "]";
    }

    private void OnAcceleSliderValueChanged(float value)
    {
        status.acceleration = value * 10f;
    }

    private void OnDeceleSliderValueChanged(float value)
    {
        status.deceleration = value * 10f;
    }

    private void OnMinMergeForceSliderValueChanged(float value)
    {
        status.minimumMergeForce = value * 10f;
    }
}
