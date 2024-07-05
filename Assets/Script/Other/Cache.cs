using System.Collections.Generic;
using UnityEngine;

public class Cache : MonoBehaviour
{
    private static readonly Dictionary<Collider2D, Character> colliderCharacters = new();
    private static readonly Dictionary<Collider2D, ShootTarget> colliderShootTargets = new();


    public static Character GetCharacterCollider(Collider2D collider)
    {
        if (!colliderCharacters.ContainsKey(collider))
        {
            colliderCharacters.Add(collider, collider.GetComponent<Character>());
        }
        return colliderCharacters[collider];
    }

    public static ShootTarget GetShootTargetCollider(Collider2D collider)
    {
        if (!colliderShootTargets.ContainsKey(collider))
        {
            colliderShootTargets.Add(collider, collider.GetComponent<ShootTarget>());
        }
        return colliderShootTargets[collider];
    }
}
