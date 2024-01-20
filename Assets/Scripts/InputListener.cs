using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    [SerializeField] private GameObject Instructions;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Instructions.SetActive(!Instructions.activeInHierarchy);
        }
    }
}
