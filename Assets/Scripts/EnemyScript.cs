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

    float health = 20f;

    private void Start()
    {
        targetToFollow = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 direction = targetToFollow.position - transform.position;
        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(targetToFollow);
        CheckHealth();
    }

    private void CheckHealth()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float dps)
    {
        Debug.Log("enemy: " + health);
        health -= dps * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HealthManager>().TakeDamage(damage);
        }
    }
}
