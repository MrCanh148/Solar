using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Camera _camera;
    [SerializeField] List<CameraWall> cameraWalls;

    private void Start()
    {
        _camera = Camera.main;

    }
    private void Update()
    {
        float boxY = _camera.orthographicSize * GameManager.instance.status.coefficientActiveGameObject;
        float boxX = boxY * (float)_camera.pixelWidth / _camera.pixelHeight;
        float distance = boxX > boxY ? boxX : boxY;
        if (cameraWalls.Count >= 4)
        {
            cameraWalls[0].transform.position = new Vector2(_camera.transform.position.x, _camera.transform.position.y + distance);
            cameraWalls[0].transform.localScale = new Vector2(distance * 2, 2f);   // Top

            cameraWalls[1].transform.position = new Vector2(_camera.transform.position.x, _camera.transform.position.y - distance);
            cameraWalls[1].transform.localScale = new Vector2(distance * 2, 2f);   // Bot

            cameraWalls[2].transform.position = new Vector2(_camera.transform.position.x + distance, _camera.transform.position.y);
            cameraWalls[2].transform.localScale = new Vector2(2f, distance * 2);   // Left

            cameraWalls[3].transform.position = new Vector2(_camera.transform.position.x - distance, _camera.transform.position.y);
            cameraWalls[3].transform.localScale = new Vector2(2f, distance * 2);   // Right
        }
    }
}
