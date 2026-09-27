using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Meteors break at a certain amount of hits set in the inspector so big meteor can also use this script
/// <summary>

public class Meteor : MonoBehaviour
{
    // the spawner, gamemanager, and camera all listen to these so meteor doesnt have to find them
    public static UnityAction<Meteor> onDestroy = delegate { };
    public static UnityAction onPlayerHit = delegate { };

    public int hitsToDestroy = 1;
    public bool isBig;
    private int hitCount;

    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if (whatIHit.CompareTag("Player"))
        {
            onPlayerHit();
            Destroy(whatIHit.gameObject);

            // Small ones break but big one keeps going
            if (!isBig)
            {
                Destroy(gameObject);
            }
        }

        else if (whatIHit.CompareTag("Laser"))
        {
            Destroy(whatIHit.gameObject);
            hitCount++;

            if (hitCount == hitsToDestroy)
            {
                onDestroy(this);
                Destroy(gameObject);
            }
        }
    }
}