using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;

public class WeaponBehavior : MonoBehaviour
{
    public GameObject prefab;
    [HideInInspector]
    public Camera playerCam;
    public float fireSpeed = 0.5f;
    public float bulletSpeed = 10;
    public KeyCode fireKey = KeyCode.Mouse0;
    [HideInInspector]
    public bool canFire = true;
    [HideInInspector]
    public static bool isFiring;
    private PlayerManager playerManager;
    private Rigidbody playerRigid;
    public AudioClip shotSound;
    private AudioSource source;

    // Start is called before the first frame update
    void Awake()
    {
        playerCam = GetComponentInParent<Camera>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerRigid = playerManager.gameObject.GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!PauseMenu.GamePaused)
        {
            if (Input.GetKey(fireKey) && playerManager.haveAmmo)
            {
                StartCoroutine(FireProjectileCo());
            }
        }
    }

    private IEnumerator FireProjectileCo()
    {
        if (canFire)
        {
            canFire = false;

            isFiring = true;

            InstantiateProjectile();

            source.clip = shotSound;

            source.Play();

            playerManager.ammo -= 1;

            yield return new WaitForSeconds(fireSpeed);

            canFire = true;
        }

        isFiring = false;
    }

    void InstantiateProjectile()
    {
        GameObject projectile = Instantiate(prefab) as GameObject;

        projectile.transform.position = transform.position + playerCam.transform.forward;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.velocity = playerCam.transform.forward * bulletSpeed;

        rb.velocity += playerRigid.velocity;

    }
}
