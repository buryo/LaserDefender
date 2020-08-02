using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] int life = 200;

    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;

    [Header("Projectile")]
    [SerializeField] GameObject Laser = default;
    [SerializeField] int shotsFired = 0;
    [SerializeField] float projectTileSpeed = 20f;
    [SerializeField] float shootTimer = 0.3f;

    [Header("Player SFX")]
    [SerializeField] AudioClip deathSFX = default;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = .75f;

    private float shipPadding = 1f;
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    Coroutine firingCorouotine;

    public event EventHandler OnSpacePressed;


    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
        OnSpacePressed += CountShots;
    }

    private void CountShots(object sender, EventArgs e)
    {
        shotsFired++;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (firingCorouotine != null)
            {
                StopCoroutine(firingCorouotine);
            }
            firingCorouotine = StartCoroutine(FireContinuously());
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCorouotine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            OnSpacePressed?.Invoke(this, EventArgs.Empty);
            GameObject laser = Instantiate(Laser, new Vector3(transform.position.x, transform.position.y + .6f, 0), Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectTileSpeed);
            yield return new WaitForSeconds(shootTimer);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("you called me?");
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>() ?? null;
        if (damageDealer != null) ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        life -= damageDealer.GetDamage();
        damageDealer.Hit();

        if (life <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPosition = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPosition = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPosition, newYPosition);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + shipPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - shipPadding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + shipPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - 3f;
    }
}
