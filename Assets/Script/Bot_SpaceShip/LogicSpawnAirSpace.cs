using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LogicSpawnAirSpace : MonoBehaviour
{
    [SerializeField] private GameObject[] shipSpacePrefab;
    [SerializeField] private float spawnInterval = 4f;
    [SerializeField] private float TimeEvolutionGO = 5f;
    private int maxShips0, maxShips1, maxShips2;

    private float spawnTimer;
    private Transform PlaceSpawn;
    private Character character;
    private List<GameObject> spawnedShips = new List<GameObject>();
    private Queue<GameObject> spawnQueue = new Queue<GameObject>();
    private Coroutine evolutionCoroutine;

    private void Start()
    {
        PlaceSpawn = GetComponent<Transform>();
        character = GetComponent<Character>();
        if (PlaceSpawn != null)
        {
            spawnedShips.Add(PlaceSpawn.gameObject);
        }
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        // Xóa các tàu đã bị hủy khỏi danh sách
        spawnedShips.RemoveAll(ship => ship == null);

        // Cập nhật số lượng tàu tối đa dựa trên số lượng kill
        MaxShipBaseKill();

        if (character.characterType == CharacterType.LifePlanet)
        {
            if (evolutionCoroutine == null)
            {
                evolutionCoroutine = StartCoroutine(TimeEvolution(TimeEvolutionGO));
            }
        }
        else
        {
            character.EvolutionDone = false;
            if (evolutionCoroutine != null)
            {
                StopCoroutine(evolutionCoroutine);
                evolutionCoroutine = null;
            }
        }

        if (spawnTimer <= 0f)
        {
            if (character.EvolutionDone)
            {
                AddShipsToQueue();
                SpawnNextShipInQueue();
            }
            spawnTimer = spawnInterval;
        }
    }

    private void MaxShipBaseKill()
    {
        if (character.Kill >= 36)
        {
            maxShips0 = 5;
            maxShips1 = 3;
            maxShips2 = 2;
        }
        else if (character.Kill >= 24)
        {
            maxShips0 = 4;
            maxShips1 = 2;
            maxShips2 = 1;
        }
        else if (character.Kill >= 12)
        {
            maxShips0 = 3;
            maxShips1 = 1;
            maxShips2 = 1;
        }
        else if (character.Kill >= 6)
        {
            maxShips0 = 2;
            maxShips1 = 1;
            maxShips2 = 0;
        }
        else
        {
            maxShips0 = 2;
            maxShips1 = 0;
            maxShips2 = 0;
        }
    }

    private void AddShipsToQueue()
    {
        // Đếm số lượng hiện tại của từng loại tàu
        int currentShips0 = CountShipsOfType(0);
        int currentShips1 = CountShipsOfType(1);
        int currentShips2 = CountShipsOfType(2);

        // Add tàu vào queue nếu số lượng hiện tại nhỏ hơn maxShips tương ứng
        if (currentShips0 < maxShips0 && !spawnQueue.Contains(shipSpacePrefab[0]))
        {
            spawnQueue.Enqueue(shipSpacePrefab[0]);
        }

        if (currentShips1 < maxShips1 && !spawnQueue.Contains(shipSpacePrefab[1]))
        {
            spawnQueue.Enqueue(shipSpacePrefab[1]);
        }

        if (currentShips2 < maxShips2 && !spawnQueue.Contains(shipSpacePrefab[2]))
        {
            spawnQueue.Enqueue(shipSpacePrefab[2]);
        }
    }

    private void SpawnNextShipInQueue()
    {
        if (spawnQueue.Count > 0)
        {
            GameObject shipPrefab = spawnQueue.Dequeue();
            SpawnShip(shipPrefab);
        }
    }

    private int CountShipsOfType(int typeIndex)
    {
        int count = 0;
        foreach (GameObject ship in spawnedShips)
        {
            if (ship != null && ship.name.Contains(shipSpacePrefab[typeIndex].name))
            {
                count++;
            }
        }
        return count;
    }

    private void SpawnShip(GameObject shipPrefab)
    {
        GameObject newShip = Instantiate(shipPrefab, PlaceSpawn.position, PlaceSpawn.rotation);
        spawnedShips.Add(newShip);

        // Thiết lập centerPoint cho RandomMovement của tàu mới
        RandomMovement randomMovement = newShip.GetComponent<RandomMovement>();
        if (randomMovement != null)
        {
            randomMovement.SetCenterPoint(PlaceSpawn);
        }

        // Thiết lập spawnedShips cho ShootTarget của tàu mới
        ShootTarget shootTarget = newShip.GetComponent<ShootTarget>();
        if (shootTarget != null)
        {
            shootTarget.SetIgnoredTargets(spawnedShips, character);
        }
    }

    private IEnumerator TimeEvolution(float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            if (character.characterType != CharacterType.LifePlanet)
            {
                character.EvolutionDone = false;
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        character.EvolutionDone = true;
    }
}
