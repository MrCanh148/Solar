using UnityEngine;

public class CameraWall : MonoBehaviour
{
    private Character character;
    private AsteroidGroup asteroidGroup;
    private ShootTarget target;
    private GroupPlanet groupPlanet;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        character = Cache.GetCharacterCollider(collision);
        if (character != null)
        {
            if (!character.isSetup)
            {
                if (!character.isPlayer)
                {
                    if (character.characterType > CharacterType.Asteroid)
                    {
                        SpawnPlanets.instance.ActiveCharacter2(character);
                    }
                    else
                    {
                        SpawnPlanets.instance.DeActiveCharacter(character);
                    }

                }
            }
        }

        asteroidGroup = collision.GetComponent<AsteroidGroup>();
        if (asteroidGroup != null)
        {
            asteroidGroup.OnInit();
            asteroidGroup.transform.localPosition = SpawnPlanets.instance.ReSpawnerAsterrooidGroup();
        }

        target = Cache.GetShootTargetCollider(collision);
        if (target != null)
        {
            Destroy(target.gameObject);
        }

        groupPlanet = collision.GetComponent<GroupPlanet>();
        if (groupPlanet != null)
        {
            if (groupPlanet.masterStar.host == null)
            {
                GroupPlanet group = SpawnPlanets.instance.GetGroupPlanet();
                {
                    if (group != null)
                    {
                        group.OnInit();
                        group.transform.localPosition = SpawnPlanets.instance.SpawnerCharacter();
                        groupPlanet.gameObject.SetActive(false);
                        group.gameObject.SetActive(true);

                    }
                    else
                    {
                        groupPlanet.OnInit();
                        groupPlanet.transform.localPosition = SpawnPlanets.instance.SpawnerCharacter();
                        groupPlanet.gameObject.SetActive(true);

                    }
                }

            }
        }
    }
}
