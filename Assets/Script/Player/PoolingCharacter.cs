using System.Collections.Generic;
using UnityEngine;

public class PoolingCharacter : FastSingleton<PoolingCharacter>
{
    public Character asteroidPrefab; // Prefab của character
    public int initialPoolSize = 10; // Kích thước ban đầu của pool

    [SerializeField] private List<Character> characterPool; // Danh sách các character trong pool

    private void Start()
    {
        characterPool = new List<Character>();
        FillPool();
    }

    private void FillPool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            Character character = Instantiate(asteroidPrefab);
            character.gameObject.SetActive(false);
            characterPool.Add(character);
        }
    }

    public Character GetCharacterFromPool()
    {
        foreach (Character character in characterPool)
        {
            if (!character.gameObject.activeSelf)
            {
                character.gameObject.SetActive(true);
                return character;
            }
        }

        // Nếu không có character nào sẵn sàng, tạo một mới
        Character newCharacter = Instantiate(asteroidPrefab);
        characterPool.Add(newCharacter);
        return newCharacter;
    }

    public void ReturnCharacterToPool(Character character)
    {
        character.gameObject.SetActive(false);
        character.tf.position = Vector3.zero;
        character.tf.rotation = Quaternion.identity;
    }
}