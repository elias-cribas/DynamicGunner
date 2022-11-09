using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    [Header("References")]
    public Camera fpscam;
    public Transform attackPoint;

    [Header("Bullet")]
    public GameObject bullet;

    public float shootForce, upwardForce;

    [Header("Graphics")]
    public GameObject muzzleflash;
    public TextMeshProUGUI ammunitionDisplay;

    [Header("Gun Stats")]
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    [Header("Bools")]
    bool shooting, readyToShoot, reloading;

    [Header("Bug fixing")]
    public bool allowInvoke = true;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();


    }
    private void AmmmoDisplay()
    {
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    }

    private void MyInput()
    {
        //Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //Automatically reload
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        //Shooting
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Find the exact hit position using a raycast
        Ray ray = fpscam.ViewportPointToRay(new Vector3(.5f, .5f,0));
        RaycastHit hit;

        // check if the ray hits something
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        // Calculate the direction from the attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        //Instatiate bullet/projectiles
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        //Rotate bullet shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpscam.transform.up * upwardForce, ForceMode.Impulse);

        //Instatiate muzzleflash
        if (muzzleflash != null)
            Instantiate(muzzleflash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function(if not already involked)
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0) 
            Invoke("Shoot", timeBetweenShooting);

    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
   
    private void ReloadFinished() 
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

}
