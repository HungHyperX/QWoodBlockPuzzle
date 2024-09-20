using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboWriting : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject writing;
    public GameObject whiteFlowers;
    public Grids grids;
    public GameObject particle;
    
    private void OnEnable()
    {
        Event.ShowComboWriting += ShowComboWriting;
    }

    private void OnDisable()
    {
        Event.ShowComboWriting -= ShowComboWriting;
    }

    private void ShowComboWriting()
    {

       
        writing.SetActive(true);
        particle.SetActive(true);
        int FullComboNum = grids.lineFullNum;
        if (FullComboNum > 4)
        {
            whiteFlowers.SetActive(false);
        }
            //Invoke("Count", 1.0f);
        Debug.Log("Appear");
    }


    //void Count()
    //{
    //    writing.SetActive(false);
    //    whiteFlowers.SetActive(false);
    //}
}
