using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionScript : MonoBehaviour
{
    private bool isActive = true;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isActive = !isActive;
            gameObject.GetComponent<Text>().enabled = isActive;
        }
    }
}
