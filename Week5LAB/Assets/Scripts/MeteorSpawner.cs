using UnityEngine; 
using UnityEngine.Events;

/// <summary>
/// Spawns a meteor every 2 seconds and big one after 5 kills, hard coded because I am getting tired
/// <summary>

public class MeteorSpawner : MonoBehaviour
{
    public static UnityAction onBigMeteorSpawned = delegate { };

    public GameObject meteorPrefab;
    public GameObject bigMeteorPrefab;

    private int meteorCount = 0;

    private void OnEnable()
    {
        Meteor.onDestroy += MeteorDestroyed;
        Meteor.onPlayerHit += StopSpawning;
    }

    private void OnDisable()
    {
        Meteor.onDestroy -= MeteorDestroyed;
        Meteor.onPlayerHit -= StopSpawning;
    }

    private void Start()
    {
        // first meteor after 1 second then every two seconds
        InvokeRepeating(nameof(SpawnMeteor), 1f, 2f);
    }

    private void SpawnMeteor()
    {
        Instantiate(meteorPrefab, new Vector3(Random.Range(-8, 8), 7.5f,0), Quaternion.identity);
    }

    private void MeteorDestroyed(Meteor meteor)
    {
        // so that big meteor kills dont count toward the next one
        if (meteor.isBig)
        {
            return;
        }

        meteorCount++;

        if (meteorCount == 5)
        {
            meteorCount = 0;
            Instantiate(bigMeteorPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
            onBigMeteorSpawned();
        }
    }

    private void StopSpawning()
    {
        CancelInvoke();
    }
}