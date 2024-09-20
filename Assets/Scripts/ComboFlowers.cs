using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboFlowers : MonoBehaviour
{
    public List<GameObject> comboFlowers;
    public Grids grids;
    private int flowersComboIndex;

    // Start is called before the first frame update
    void OnEnable()
    {
        ShowFlowers();
    }

    public void ShowFlowers()
    {
        flowersComboIndex = grids.lineFullNum - 3;
        if (flowersComboIndex >= 0)
        {
            comboFlowers[flowersComboIndex].SetActive(true);
            StartCoroutine(Count(comboFlowers[flowersComboIndex]));
        }
        //Invoke("Count", 1f);
    }
    IEnumerator Count(GameObject comboNumber)
    {
        yield return new WaitForSeconds(2f);
        comboNumber.SetActive(false);
    }
    //void Count()
    //{

    //    comboFlower.SetActive(false);

    //}

}
