
using UnityEngine;
using static Utils;

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
                int randomNumber = Random.Range(1, 4);
                GameObject toInstantiate = null;
                if(randomNumber == 1)
                {
                    toInstantiate = blocks[0];
                }
                else if(randomNumber == 2)
                {
                    toInstantiate = blocks[1];
                }
                else if (randomNumber == 3)
                {
                    toInstantiate = blocks[2];
                }

                GameObject grass = Instantiate(toInstantiate, new Vector3(x, y, z), Quaternion.identity);
                grass.transform.SetParent(FindGameObjectWithTag(Tags.Environment).transform);
            }
    }
}

/*
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject grassPrefab;
    public GameObject woodPrefab;
    public GameObject cobblestonePrefab;
    public GameObject limitBlockPrefab;

    public int pyramidWidth = 20;
    public int pyramidHeight = 20;

    private Transform environmentTransform;

    private void Start()
    {
        environmentTransform = GameObject.FindWithTag("Environment").transform;

        GenerateBaseLevel();
        GeneratePyramid();
    }

    private void GenerateBaseLevel()
    {
        for (int x = 0; x < pyramidWidth; x++)
        {
            for (int z = 0; z < pyramidWidth; z++)
            {
                Vector3 blockPosition = new Vector3(x, 0, z);
                Instantiate(limitBlockPrefab, blockPosition, Quaternion.identity, environmentTransform);
            }
        }
    }

    private void GeneratePyramid()
    {
        for (int y = 1; y <= pyramidHeight; y++)
        {
            int layerWidth = pyramidWidth - (y * 2);
            int layerStart = y;

            for (int x = layerStart; x < layerStart + layerWidth; x++)
            {
                for (int z = layerStart; z < layerStart + layerWidth; z++)
                {
                    Vector3 blockPosition = new Vector3(x, y, z);
                    GameObject blockPrefab = GetRandomBlock();
                    Instantiate(blockPrefab, blockPosition, Quaternion.identity, environmentTransform);
                }
            }
        }
    }

    private GameObject GetRandomBlock()
    {
        int randomIndex = Random.Range(0, 3);

        switch (randomIndex)
        {
            case 0:
                return grassPrefab;
            case 1:
                return cobblestonePrefab;
            case 2:
                return woodPrefab;
            default:
                return grassPrefab;
        }
    }
}
*/
/*
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject dirtBlockPrefab;
    public GameObject grassBlockPrefab;
    public GameObject cobblestoneBlockPrefab;
    public GameObject woodBlockPrefab;

    private Transform environment;

    private Transform pyramidObject;
    [SerializeField] int limitBlockDimension = 100;

    // Initialize pyramid size
    [SerializeField] int layerSizePyramid = 25;

    [SerializeField] public Vector3 pyramidPosition = new Vector3(31.5f,1,36);
    
    private void Start()
    {
        environment = GameObject.FindWithTag("Environment").transform;
        pyramidObject = GameObject.Find("Pyramid").transform;
        // Generate base layer of limit block
        /*
        for (int i = 0; i < limitBlockDimension; i++)
        {
            for (int j = 0; j < limitBlockDimension; j++)
            {
                Vector3 blockPosition = new Vector3(i, -1, j);
                //Instantiate(limitBlockPrefab, blockPosition, Quaternion.identity, environment);
                Instantiate(dirtBlockPrefab, new Vector3(blockPosition.x,blockPosition.y+1, blockPosition.z), Quaternion.identity, environment);
            }
        }

        */
/*
        // Generate pyramids on top of the base layer of limit block
        GeneratePyramid(pyramidPosition, pyramidObject); 
    }

    private void GeneratePyramid(Vector3 center,Transform pyramid)
    {
        // Generate pyramid layer by layer
        for (int layer = 0; layer < layerSizePyramid; layer++)
        {
            // Calculate the size of the current layer
            int layerSize = layerSizePyramid - layer;

            // Generate blocks in the current layer
            for (int i = 0; i < layerSize; i++)
            {
                for (int j = 0; j < layerSize; j++)
                {
                    Vector3 blockPosition = new Vector3(center.x + i - layerSize / 2 + 0.5f, layer + center.y, center.z + j - layerSize / 2 + 0.5f);

                    // Place grass block at the outside of the pyramid
                    if (i == 0 || i == layerSize - 1 || j == 0 || j == layerSize - 1)
                    {
                        Instantiate(grassBlockPrefab, blockPosition, Quaternion.identity, pyramid);
                    }
                    // Place cobblestone or wood block inside the pyramid with a given chance
                    else
                    {
                        int blockType = Random.Range(0, 2);
                        if (blockType == 0)
                        {
                            Instantiate(cobblestoneBlockPrefab, blockPosition, Quaternion.identity, pyramid).transform.SetParent(pyramid);
                        }
                        else
                        {
                            Instantiate(woodBlockPrefab, blockPosition, Quaternion.identity, pyramid).transform.SetParent(pyramid);
                        }
                    }
                }
            }
        }
    }
}
*/