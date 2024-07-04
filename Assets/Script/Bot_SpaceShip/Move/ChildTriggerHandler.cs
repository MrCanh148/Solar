using UnityEngine;

public class ChildTriggerHandler : MonoBehaviour
{
    public ShootTarget parentHandler;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (parentHandler != null)
        {
            parentHandler.OnChildTriggerEnter(collision);
        }
    }
}
