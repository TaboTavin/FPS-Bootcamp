using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public Transform[] firePoints;
    private float nextFireTime = 0f;

    public enum WeaponType { Pistol, MachineGun, Shotgun }
    public WeaponType currentWeapon = WeaponType.Pistol;

    // PreFab de Balas
    public GameObject pistolBulletPreFab;
    public GameObject machineGunBulletPreFab;
    public GameObject shotgunBulletPrefab;

    // Recoil
    public float recoilAmount = 1.5f;
    public float recoilDuration = 0.1f;

    private GameObject GetCurrentBulletPrefab()
    {
        switch(currentWeapon)
        {
            case WeaponType.Pistol:
                return pistolBulletPreFab;

            case WeaponType.MachineGun:
                return machineGunBulletPreFab;

            case WeaponType.Shotgun:
                return shotgunBulletPrefab;

            default:
                return null;
        }
    }


    private float GetFireRate()
    {
        switch(currentWeapon)
        {
            case WeaponType.Pistol:
                return 1.8f;

            case WeaponType.MachineGun:
                return 4.5f;

            case WeaponType.Shotgun:
                return 0.7f;

            default:
                return 0.2f;
        }
    }

    private int GetNumberOfPellets()
    {
        switch(currentWeapon)
        {
            case WeaponType.Pistol:
                return 1;

            case WeaponType.MachineGun:
                return 1;

            case WeaponType.Shotgun:
                return 5;

            default:
                return 1;
        }
    }

    private float GetBulletSpeed()
    {
        switch(currentWeapon)
        {
            case WeaponType.Pistol:
                return 50f;

            case WeaponType.MachineGun:
                return 85f;

            case WeaponType.Shotgun:
                return 30f;

            default:
                return 30f;
        }
    }

    private Vector3 ApplyGravity(Vector3 velocity, float time)
    {
        float gravity = Physics.gravity.y;
        velocity.y += gravity * time;

        return velocity;
    }

    private void Update()
    {
        #region Cambio de Armas por Numeros
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = WeaponType.Pistol;
        }

        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = WeaponType.MachineGun;
        }

        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeapon = WeaponType.Shotgun;
        }
        #endregion

        #region Cambio de Armas por Scroll de Mouse
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");

        if(mouseScroll > 0)
        {
            SwitchToNextWeapon();
        }

        else if(mouseScroll < 0)
        {
            SwitchToPreviousWeapon();
        }
        #endregion

        // Disparo
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / GetFireRate();

            ApplyRecoil();
        }
    }

    private void Shoot()
    {
        int numPellets = GetNumberOfPellets();

        for(int i = 0; i < numPellets; i++)
        {
            GameObject bulletPrefab = GetCurrentBulletPrefab();

            if (numPellets == 1)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();

                BulletPhysics(bulletRB);
            }

            else
            {
                int randomFirePointIndex = Random.Range(0, firePoints.Length);
                Transform selectedFirePoint = firePoints[randomFirePointIndex];

                GameObject bullet = Instantiate(bulletPrefab, selectedFirePoint.position, selectedFirePoint.rotation);
                Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();

                BulletPhysics(bulletRB);
            }
        }

        // Sonido de disparo, effectos visuales.
    }
    private void BulletPhysics(Rigidbody rb)
    {
        if (rb != null)
        {
            float bulletSpeed = GetBulletSpeed();
            rb.velocity = ApplyGravity(firePoint.forward * bulletSpeed, Time.deltaTime);
        }

    }

    private void SwitchToNextWeapon()
    {
        int nextWeaponIndex = ((int)currentWeapon + 1) % System.Enum.GetValues(typeof(WeaponType)).Length;
        currentWeapon = (WeaponType)nextWeaponIndex;
    }

    private void SwitchToPreviousWeapon()
    {
        int previousWeaponIndex = ((int)currentWeapon - 1 + System.Enum.GetValues(typeof(WeaponType)).Length) % System.Enum.GetValues(typeof(WeaponType)).Length;
        currentWeapon = (WeaponType)previousWeaponIndex;
    }

    private void ApplyRecoil()
    {
        Camera mainCamera = Camera.main;
        Vector3 recoilVector = new Vector3(Random.Range(-recoilAmount, recoilAmount), recoilAmount, 0f);
        StartCoroutine(RecoilEffect(mainCamera.transform, recoilVector));
    }

    private IEnumerator RecoilEffect(Transform camaraTransform, Vector3 recoilVector)
    {
        float elapsedTime = 0f;
        Vector3 originalPosition = camaraTransform.localPosition;

        while (elapsedTime < recoilDuration)
        {
            float verticalRecoil = Mathf.Lerp(recoilVector.y, 0f, elapsedTime / recoilDuration);
            Vector3 newLocalPosition = originalPosition + new Vector3(0f, verticalRecoil, 0f);

            camaraTransform.localPosition = newLocalPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        camaraTransform.localPosition = originalPosition;
    }
}
