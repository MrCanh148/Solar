using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    private float radius, rotationSpeed;
    [SerializeField] private LineRenderer lineToCenter;
    [SerializeField] private float timeLimit = 2f;

    private List<Transform> centers = new List<Transform>();
    private Character planet1;
    private Vector3 orbitPosition;
    public float numberOrbit = 0; // center sẽ tăng biến này
    private float TimeConnect; 
    private bool isConnecting; 
    private bool isIncrease = false; // Biến Check cho việc tăng numberorbit Center

    private void Start()
    {
        lineToCenter.positionCount = 2;
        lineToCenter.enabled = false;
        planet1 = GetComponent<Character>();
        rotationSpeed = Random.Range(19f, 25f);
        rotationSpeed *= Random.value > 0.5f ? 1 : -1;
    }

    private void FixedUpdate()
    {
        if (isConnecting)
            TimeConnect += Time.deltaTime;

        if (TimeConnect >= timeLimit)
        {
            orbitPosition = MoveToOrbitPosition();

            if (centers.Count > 0)
            {
                StartCoroutine(MoveToPosition(transform, orbitPosition, 0.5f));
                lineToCenter.SetPosition(0, centers[centers.Count - 1].position);
                lineToCenter.SetPosition(1, transform.position);
                lineToCenter.enabled = true;
            }
        }
    }

    private IEnumerator MoveToPosition(Transform transform, Vector3 targetPosition, float timeToMove)
    {
        Vector3 currentPos = transform.position;
        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, targetPosition, t);
            yield return null;
        }
        orbitPosition = CalculateOrbitPosition();
    }

    private Vector3 MoveToOrbitPosition()
    {
        if (centers.Count == 0)
        {
            return transform.position;
        }

        Vector3 centerPosition = centers[centers.Count - 1].position;
        Vector3 directionToCenter = centerPosition - transform.position;
        Vector3 orbitDirection = directionToCenter.normalized * radius;
        Vector3 newPosition = centerPosition - orbitDirection;

        return newPosition;
    }


    private Vector3 CalculateOrbitPosition()
    {
        if (centers.Count == 0)
        {
            return transform.position;
        }

        Vector3 centerPosition = centers[centers.Count - 1].position;
        float angleInRadians = (Time.time * rotationSpeed) * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleInRadians) * radius;
        float y = Mathf.Sin(angleInRadians) * radius;

        Vector3 newPosition = new Vector3(x, y, 0) + centerPosition;

        return newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character planet2 = collision.GetComponent<Character>();
        Orbit orbitor = collision.GetComponent<Orbit>();

        if (planet2 == null || orbitor == null)
            return;

        if (planet2.generalityType == planet1.generalityType + 1)
        {
            isConnecting = true;
            centers.Add(planet2.transform);

            if (!isIncrease)
            {
                orbitor.numberOrbit += 0.75f;
                isIncrease = true;
                radius = orbitor.numberOrbit;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Character planet2 = collision.GetComponent<Character>();
        Orbit orbitor = collision.GetComponent<Orbit>();

        if (planet2 == null || orbitor == null)
            return;

        if (planet2.generalityType == planet1.generalityType + 1)
        {
            centers.Remove(planet2.transform);
            radius = orbitor.numberOrbit;
            isConnecting = false;
            TimeConnect = 0;
            isIncrease = false;
            orbitor.numberOrbit -= 0.75f;
        }
    }
}