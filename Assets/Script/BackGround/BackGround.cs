using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Renderer renderTerrain;
    [SerializeField] float speed;

    private void Update()
    {
        transform.position = player.position;
        renderTerrain.material.mainTextureOffset = new Vector2(player.position.x * 0.05f, player.position.y * 0.05f) * speed;
    }
}
