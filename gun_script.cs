using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NUnit.Framework.Constraints;
// using UnityEditor.TerrainTools;
using TMPro;
public class gun_script : MonoBehaviour
{

    [SerializeField] Sprite gunSprite;
    [SerializeField] Sprite  gunUI;
    [SerializeField] int maxBullets;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireRate;
    [SerializeField] int projectilePerShot;
    [SerializeField] float bulletSpread;
    [SerializeField] Transform firepoint;
    [SerializeField] Image UIImage;
    [SerializeField] TextMeshProUGUI text;
    //Transform firePoint/**/;
    Quaternion rotationPoint;
    gun my_gun = null;
    gun_pickup pickup = null;
    bool has_gun = false;   
    int currentBullets;
    private float nextFireTime = 0f;



    float timer = 0f;
    float waitTime = 1000f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

/*        rotationPoint = Quaternion.Euler(0, 0, 90f);
        currentBullets = maxBullets;
        GetComponent<SpriteRenderer>().sprite = gunSprite;

        UIImage.sprite = gunUI;
        UIImage.color= new Color(255,255,255,255);*/
    }

    // Update is called once per frame
    void Update()
    {

        if (has_gun ==false && Input.GetKeyDown(KeyCode.E) && pickup != null)
        {
            has_gun = true;
            my_gun = pickup.arma;
            // put gun items 
            currentBullets = my_gun.maxBullets;
            maxBullets = my_gun.maxBullets;
            GetComponent<SpriteRenderer>().sprite = my_gun.gunSprite;
            bulletPrefab = my_gun.bulletPrefab;
            fireRate= my_gun.fireRate;
            projectilePerShot = my_gun.projectilePerShot;
            bulletSpread = my_gun.bulletSpread;

            text.text = $"{currentBullets}/{maxBullets} Rounds";
            UIImage.sprite = my_gun.gunUI;
            UIImage.color = new Color(255, 255, 255, 255);
            pickup.PickUp();
        }

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && has_gun ==true)
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            if(has_gun == true){
                // discard gun and disable 
                currentBullets = 0;
                maxBullets = 0;
                GetComponent<SpriteRenderer>().sprite = null ;
                UIImage.sprite = null;
                UIImage.color = new Color(255, 255, 255, 0);
                has_gun = false;
                pickup = null;
                my_gun = null;
                text.text = $"0/0 Rounds";


            }


        }



    }



    void Fire()
    {
        if (currentBullets <= 0)
        {
            currentBullets = 0;
            maxBullets = 0;
            GetComponent<SpriteRenderer>().sprite = null;
            UIImage.sprite = null;
            UIImage.color = new Color(255, 255, 255, 0);
            has_gun = false;
            pickup = null;
            my_gun = null;
            text.text = $"0/0 Rounds";

            return;
        }

        Debug.Log($"Firing {projectilePerShot} projectiles with {currentBullets} ammo left!");

        nextFireTime = Time.time + fireRate;

        float angleStep = projectilePerShot > 1 ? bulletSpread / (projectilePerShot - 1) : 0f;
        float startAngle = -bulletSpread / 2f;

        for (int i = 0; i < projectilePerShot; i++)
        {
            float currentAngle = startAngle + i * angleStep;
            Quaternion rotation = firepoint.rotation * Quaternion.Euler(0, 0, currentAngle);
            GameObject projectile = Instantiate(bulletPrefab, firepoint.position, rotation);   
  
        }

        currentBullets-=projectilePerShot;
        text.text = $"{currentBullets}/{maxBullets} Rounds";
        if (currentBullets <= 0)
        {
            currentBullets = 0;
            maxBullets = 0;
            GetComponent<SpriteRenderer>().sprite = null;
            UIImage.sprite = null;
            UIImage.color = new Color(255, 255, 255, 0);
            has_gun = false;
            pickup = null;
            my_gun = null;
            text.text = $"0/0 Rounds";

            return;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickup"))
        {
            pickup = other.GetComponent<gun_pickup>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pickup"))
        {
            if (pickup != null && other.gameObject == pickup.gameObject)
            {
                pickup = null;
            }
        }
    }


}
