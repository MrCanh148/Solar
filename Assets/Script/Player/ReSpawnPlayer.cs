using System.Collections;
using UnityEngine;

public class ReSpawnPlayer : MonoBehaviour
{
    public static ReSpawnPlayer Instance;

    [SerializeField] private float distanceTele;

    private Character character;
    private Player player;
    private Vector2 currentPos;
    private bool resetVelocity = false;

    private void Start()
    {

        Instance = this;
        character = GetComponent<Character>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (resetVelocity)
        {
            player.ResetVelocity();
            resetVelocity = false;
        }
     
    }

    private IEnumerator TeleNewPos()
    {
        yield return new WaitForSeconds(2f);
        RespawnPlace();
        character.spriteRenderer.enabled = true;
        character.canControl = true;
        resetVelocity = true;
        LogicUIPlayer.Instance.BgFadeIn(1f);
    }

    private void RespawnPlace()
    {
        Vector2 newPos = new Vector2(0, 0);

        newPos.x = (Random.value > 0.5f ? distanceTele : -distanceTele) + currentPos.x;
        newPos.y = (Random.value > 0.5f ? distanceTele : -distanceTele) + currentPos.y;

        transform.position = newPos;
    }

    public void ResPlayer()
    {
        player.ResetVelocity();
        LogicUIPlayer.Instance.BgFadeOut(0.5f);
        character.AllWhenDie();
        character.spriteRenderer.enabled = false;
        character.canControl = false;
        currentPos = transform.position;
        StartCoroutine(TeleNewPos());
    }
}
