using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    Transform targetToFollow;

    private float speed = 1f;
    public float jumpforce = 3;
    private float damage = 20f;
    private float timeToDisappear = 2f;

    private AudioSource damageSoundPlayer;
    private Animator anim;
    float health = 20f;
    private bool isDead;
    private void Start()
    {
        targetToFollow = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        damageSoundPlayer = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 direction = targetToFollow.position - transform.position;
        direction.Normalize();
        if(!isDead)
            transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(targetToFollow);
        CheckHealth();
    }

    private void CheckHealth()
    {
        if(health <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(DeathRoutine());
        }
    }

    public void TakeDamage(float dps)
    {
        Debug.Log("enemy: " + health);
        health -= dps * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDead)
        {
            collision.gameObject.GetComponent<HealthManager>().TakeDamage(damage);
            damageSoundPlayer.Play();
        }
    }

    IEnumerator DeathRoutine()
    {
        anim.SetBool("isDead", true);
        yield return new WaitForSeconds(timeToDisappear);
        Destroy(gameObject);
    }
}
