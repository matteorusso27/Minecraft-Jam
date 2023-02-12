/*
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int sizeX;
    public int sizeZ;

    public float terDetail;
    public float terHeight;
    int seed;

    public GameObject[] blocks;
    
    void Start()
    {
        seed = Random.Range(1, 2);
        GenerateTerrain();
    }

    private void GenerateTerrain()
    {
        for(int x = 0; x < sizeX; x++)
            for(int z = 0 ; z < sizeZ; z++)
            {
                int y = (int)(Mathf.PerlinNoise((x / 2 + seed) / terDetail, (z / 2 + seed) / terDetail) * terHeight);
                GameObject grass = Instantiate(blocks[0], new Vector3(x, y, z), Quaternion.identity);
                grass.transform.SetParent(GameObject.FindGameObjectWithTag("Environment").transform);
            }
    }
}
*/
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject grassPrefab;
    public GameObject woodPrefab;
    public GameObject cobblestonePrefab;
    public int worldWidth = 10;
    public int worldLength = 10;
    public int worldHeight = 5;

    private void Start()
    {
        for (int y = 0; y < worldHeight; y++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                for (int z = 0; z < worldLength; z++)
                {
                    int randomNumber = Random.Range(0, 100);
                    if (randomNumber < 70)
                    {
                        Instantiate(grassPrefab, new Vector3(x, y, z), Quaternion.identity);
                    }
                    else if (randomNumber >= 70 && randomNumber < 90)
                    {
                        Instantiate(woodPrefab, new Vector3(x, y, z), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(cobblestonePrefab, new Vector3(x, y, z), Quaternion.identity);
                    }
                }
            }
        }
    }
}
