using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeSquare : MonoBehaviour
{
    [SerializeField]
    private Image NormalImage;
    public Image OccupiedImage;
    public Image HooverImage;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        NormalImage.gameObject.SetActive(true);
        OccupiedImage.gameObject.SetActive(false);
        HooverImage.gameObject.SetActive(false);
        _animator = GetComponent<Animator>();
        if (this.transform.parent.tag != "ShapePlay")
        {
            _animator.enabled = false;
        }
    }

    public void DeactivateSquare()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.SetActive(false);
    }

    public void ActivateSquare()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;  
        gameObject.SetActive(true);
    }

    public void DisableShapeSquare()
    {
        OccupiedImage.gameObject.SetActive(true);
        NormalImage.gameObject.SetActive(false);
    }

    public void EnableShapeSquare()
    {
        OccupiedImage.gameObject.SetActive(false);
        NormalImage.gameObject.SetActive(true);
    }

    public void DisableHoover()
    {
        HooverImage.gameObject.SetActive(false);
    }

    public void EnableHoover()
    {
        HooverImage.gameObject.SetActive(true);
    }
}
