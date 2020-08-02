using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy stats")]
    [SerializeField] float health = 100;

    [Header("Enemy shooting")]
    [SerializeField] GameObject laser = default;
    [SerializeField] float shotCounter;
    [SerializeField] float projectTileSpeed = 1f;
    [SerializeField] float minTimeBetween = .8f;
    [SerializeField] float maxTimeBetween = 4f;
    Coroutine countAndShoot = default;

    [Header("Enemy items")]
    [SerializeField] GameObject explosion = default;
    [SerializeField] AudioClip deathSFX = default;
    [SerializeField] AudioClip shootSFX = default;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = .75f;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = .25f;

    void Start()
    {
        countAndShoot = StartCoroutine(CountDownAndShoot());

    }

    IEnumerator CountDownAndShoot()
    {
        while (true)
        {
            shotCounter = UnityEngine.Random.Range(minTimeBetween, maxTimeBetween);
            yield return new WaitForSeconds(shotCounter);
            Fire();
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(this.laser, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectTileSpeed);
        AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameObject explosion = Instantiate(this.explosion, transform.position, Quaternion.identity) as GameObject;
        Destroy(gameObject);
        Destroy(explosion, 1f);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
    }
}