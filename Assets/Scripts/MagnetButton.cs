using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering.UI;

public class MagnetButton : MonoBehaviour
{
    private Button magnetButton; // Reference to the button in your UI
    public GameObject magnetMove;
    public Grids grids;
    private void Start()
    {
        magnetButton = this.GetComponent<Button>();
        // Add a listener to the button's onClick event
        magnetButton.onClick.AddListener(OnButtonClick);
        if (Event.MagnetAble == 0)
        {
            magnetButton.interactable = false;
        }
    }

    private void OnButtonClick()
    {

        magnetMove.SetActive(true);
        // Disable the button so it cannot be clicked again
        Event.MagnetAble = 0;
        magnetButton.interactable = false;

        StartCoroutine(MagnetWait(1.8f));
        // Start a coroutine to disable magnetMove after 4 seconds
        //StartCoroutine(DisableMagnetMoveAfterDelay(5.3f));
    }

    private IEnumerator DisableMagnetMoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        magnetMove.SetActive(false);
    }

    private IEnumerator MagnetWait(float delay)
    {
        yield return new WaitForSeconds(delay);
        grids.MagnetDownGrids();
        
    }
}
