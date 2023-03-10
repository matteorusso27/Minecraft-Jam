using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightSpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    public float rotationSpeed = 5f;
    private float dropHeight = 7f;
    private float timeIntervalToSpawn = 20f;
    public bool night;

    private List<GameObject> spawnedSpiders;
    private void Start()
    {
        spawnedSpiders = new List<GameObject>();
        StartCoroutine(SpawnEnemyCoRoutine());
    }
    void Update()
    {
        ChangeTime();
        night = IsNight();
        KillSpiders();
    }

    private void ChangeTime()
    {
        transform.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
    }
    IEnumerator SpawnEnemyCoRoutine()
    {
        while (true)
        {
            if (IsNight())
            {
                spawnedSpiders.Add(Instantiate(enemy, new Vector3(Random.Range(3,25), dropHeight, Random.Range(3, 25)), Quaternion.identity));
            }
            yield return new WaitForSeconds(timeIntervalToSpawn);
        }
    }

    bool IsNight()
    {
        // If the angle is greater than the threshold angle, it is nighttime
        if (transform.rotation.eulerAngles.x >= 0 && transform.rotation.eulerAngles.x <= 90)
        {
            return false; // It is night
        }
        return true; // It is not night
    }

    private void KillSpiders()
    {
        if(spawnedSpiders.Count > 0 && !IsNight())
        {
            foreach (GameObject spider in spawnedSpiders)
            {
                Destroy(spider);
            }

            spawnedSpiders = new List<GameObject>();
        }
        
    }
}
