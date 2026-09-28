using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Switches to the zoomed out camera when the big meteor is spawned and turns off when it is destroyed
/// </summary>

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera playerCamera;
    public CinemachineCamera bigMeteorCamera;

    private void OnEnable()
    {
        MeteorSpawner.onBigMeteorSpawned += ZoomOut;
        Meteor.onDestroy += MeteorDestroyed;
    }

    private void OnDisable()
    {
        MeteorSpawner.onBigMeteorSpawned -= ZoomOut;
        Meteor.onDestroy -= MeteorDestroyed;
    }

    private void Start()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        playerCamera.Follow = player;
        bigMeteorCamera.Follow = player;
    }

    private void ZoomOut()
    {
        playerCamera.gameObject.SetActive(false);
        bigMeteorCamera.gameObject.SetActive(true);
    }

    private void MeteorDestroyed(Meteor meteor)
    {
        // only zooms back in for big one
        if (!meteor.isBig)
        {
            return;
        }

        bigMeteorCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }
}