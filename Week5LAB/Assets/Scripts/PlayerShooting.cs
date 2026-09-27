using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Shoots laser when fire is pressed then waits before it can shoot again
/// </summary>
public class PlayerShooting : MonoBehaviour
{
   public GameObject laserPrefab;
   private bool canShoot = true;

   private void OnEnable()
    {
        PlayerInputHandler.onFire += Shoot;
    }

    private void OnDisable()
    {
        PlayerInputHandler.onFire -= Shoot;
    }

    private void Shoot()
    {
        if (!canShoot)
        {
            return;
        }

        Instantiate(laserPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        canShoot = false;
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }
}
