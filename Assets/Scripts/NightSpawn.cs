using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightSpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    public float rotationSpeed = 5f;
    private float dropHeight = 7f;
    [SerializeField] private bool isNight; 
    void Update()
    {
        ChangeTime();
        if (Mathf.Abs(transform.rotation.x) < Mathf.PI) isNight = false;
        else
        {
            isNight = true;
        }
        Quaternion originalRotation = transform.localRotation;
        if (transform.localEulerAngles.x > 360)
        {
            transform.localRotation = new Quaternion(0, originalRotation.y, originalRotation.z, originalRotation.w);
        }
        StartCoroutine(SpawnEnemyCoRoutine());
    }

    private void ChangeTime()
    {
        transform.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
    }
    IEnumerator SpawnEnemyCoRoutine()
    {
        while (true)
        {
            if (isNight)
            {
                Instantiate(enemy, new Vector3(Random.Range(3,25), dropHeight, Random.Range(3, 25)), Quaternion.identity);
            }
            yield return new WaitForSeconds(10f);
        }
    }
}
