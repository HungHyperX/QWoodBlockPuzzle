using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberCombo : MonoBehaviour
{
    public List<GameObject> comboNumbers;
    public Grids grids;
    //float timePassed = 0;
    private int numComboIndex;

    // Start is called before the first frame update
    void OnEnable()
    {
        ShowNumbers();
    }

    public void ShowNumbers()
    {
        numComboIndex = grids.lineFullNum - 3;
        if (numComboIndex >= 0)
        {
            comboNumbers[numComboIndex].SetActive(true);
            StartCoroutine(Count(comboNumbers[numComboIndex]));
        }
    }

    //public void DeactivateWriting()
    //{
    //    gameObject.SetActive(false);
    //}
    private IEnumerator Count(GameObject comboNumber)
    {
        
        yield return new WaitForSeconds(2f);
        comboNumber.SetActive(false);
    }
   
}
