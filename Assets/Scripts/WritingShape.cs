using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WritingShape : MonoBehaviour
{
    public GameObject particle;
    public void DeactivateWriting()
    {
        gameObject.SetActive(false);
        particle.SetActive(false);
    } 


}
