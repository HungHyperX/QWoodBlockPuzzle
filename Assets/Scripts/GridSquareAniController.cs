using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquareAniController : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    
}
