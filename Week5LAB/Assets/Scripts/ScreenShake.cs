using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Shakes the camera whenever a meteor breaks.
/// </summary>
// the Impulse Source is what actually makes the shake, this just tells it when
[RequireComponent(typeof(CinemachineImpulseSource))]
public class ScreenShake : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        Meteor.onDestroy += Shake;
    }

    private void OnDisable()
    {
        Meteor.onDestroy -= Shake;
    }

    private void Shake(Meteor meteor)
    {
        impulseSource.GenerateImpulse();
    }
}