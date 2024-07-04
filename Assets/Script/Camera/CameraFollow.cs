using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Character plant;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothSpeed = 0.125f;
    private Vector3 targetPosition;
    [SerializeField] private Camera m_Camera;
    [SerializeField] private float IntScale = 0.2f;
    private float StartSize;
    float currentSize;

    private void Start()
    {
        StartSize = m_Camera.orthographicSize;
        currentSize = StartSize + (int)plant.characterType * IntScale;
        if (m_Camera != null)
        {
            m_Camera.orthographicSize = currentSize;
        }
    }

    void FixedUpdate()
    {
        if (plant.canControl)
        {
            Vector3 desiredPosition = plant.tf.position + offset;
            targetPosition = desiredPosition;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;

        OnChangeSize(StartSize + (int)plant.characterType * IntScale);
    }

    public void OnChangeSize(float newSize)
    {
        if (currentSize != newSize)
        {
            if (m_Camera != null)
            {
                DOTween.To(() => m_Camera.orthographicSize, x => m_Camera.orthographicSize = x, newSize, 2f);
            }

        }

        currentSize = newSize;
    }
}
