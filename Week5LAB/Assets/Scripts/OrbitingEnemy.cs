using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy that circles the player and always points at them. The point the enemy circles follows the player with a delay.
/// </summary>
public class OrbitingEnemy : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Orbit")] 
    // degrees per second, can be negative to orbit the other way
    [SerializeField] private float angularSpeed = 15f;

    // how fast the orbit point follows the player, lower means further behind the player
    [SerializeField] private float followSpeed = 2f;

    [Header("Distance Scaling")]
    // the further the enemy is the faster it moves
    [SerializeField] private float nearDistance = 4f;
    [SerializeField] private float farDistance = 6f;
    [SerializeField] private float farSpeedMultiplier = 2f;

    [Header("Separation")]
    [SerializeField] private float separationRadius = 1f;
    [SerializeField] private float separationStrength = 0.5f;

    // every enemy adds itself here so they can check the others without searching the scene
    private static readonly List<OrbitingEnemy> activeEnemies = new List<OrbitingEnemy>();

    private Vector3 orbitCenter;
    private float orbitAngle;
    private float orbitRadius;

    private void OnEnable()
    {
        activeEnemies.Add(this);
    }

    private void OnDisable()
    {
        activeEnemies.Remove(this);
    }

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        orbitCenter = player.position;

        // where the enemy is placed in the scene sets its orbit radius and angle
        Vector2 offset = transform.position - orbitCenter;
        orbitRadius = offset.magnitude;
        orbitAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    }

    private void Update()
    {
        // stop moving once the ship is destroyed, otherwise this errors every frame
        if (player == null)
        {
            return;
        }

        Vector2 toPlayer = player.position - transform.position; 

        orbitCenter = Vector3.MoveTowards(orbitCenter, player.position, followSpeed * Time.deltaTime);
        orbitAngle += angularSpeed * GetSpeedScale(toPlayer.sqrMagnitude) * Time.deltaTime;

        // cos and sin give a point on a circle of radius 1, multiplying by orbitRadius pushes it out to the real orbit
        float radians = orbitAngle * Mathf.Deg2Rad;
        Vector3 ringPoint = orbitCenter + new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * orbitRadius;

        transform.position = ringPoint + GetSeparation() * separationStrength;

        FacePlayer(player.position - transform.position);
    }

    // both squared so they can compare against sqrMagnitude
    private float GetSpeedScale(float sqrDistance)
    {
        float t = Mathf.InverseLerp(nearDistance * nearDistance, farDistance * farDistance, sqrDistance);
        return Mathf.Lerp(1f, farSpeedMultiplier, t);
    }

    // for position offset, pushes enemies apart if theey are close and settle back after
    private Vector3 GetSeparation()
    {
        Vector3 push = Vector3.zero;
        float sqrRadius = separationRadius * separationRadius;

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            OrbitingEnemy other = activeEnemies[i];

            if (other == this) 
            {
                continue;
            }

            Vector3 away = transform.position - other.transform.position;
            float sqrDistance = away.sqrMagnitude;

            if (sqrDistance > sqrRadius)
            {
                continue;       
            }

            // closer the enemy the bigger the push
            push += away.normalized * (1f - Mathf.Sqrt(sqrDistance) / separationRadius);
        }

        return push;
    }

    // atan2 gives the angle of the vector, rad2deg converts it and then euler applies it around z for 2D
    private void FacePlayer(Vector2 toPlayer)
    {
        float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}