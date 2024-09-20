using DG.Tweening;
using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public Image ghostImage;
    public Image activeImage;
    public Image normalImage;
    public Sprite GridBase;
    public Image hintImage;
    public Image tipImage;
    public Image bombedImage;
    public ParticleSystem particleSystemPrefab;
    public bool Selected { get; set; }
    public int SquareIndex { get; set; }
    public bool SquareOccupied { get; set; }

    public bool Bombed { get; set; }

    private Animator childAnimator;
    

    private void Awake()
    {
        // Truy cập trực tiếp vào đối tượng con bằng tên
        Transform childTransform = transform.Find("Active");


        if (childTransform != null)
        {
            // Lấy thành phần Animator từ đối tượng con
            childAnimator = childTransform.GetComponent<Animator>();

            if (childAnimator != null)
            {
                // Kiểm tra nếu Animator có một AnimatorController được gán
                if (childAnimator.runtimeAnimatorController != null)
                {
                    //Debug.Log("AnimatorController is assigned.");
                    // Sử dụng Coroutine để trì hoãn thiết lập giá trị Animator

                }
                else
                {
                    Debug.LogError("AnimatorController is not assigned.");
                }
            }
            else
            {
                Debug.LogError("Animator component not found in child object.");
            }
        }
        else
        {
            Debug.LogError("Child object 'Active' not found.");
        }
        Selected = false;
        SquareOccupied = false;
        Bombed = false;
    }

    void Start()
    {
        //Selected = false;
        //SquareOccupied = false;

    }

    public bool CheckUsed()
    {
        return ghostImage.gameObject.activeSelf;
    }

    public void PlaceShapeOnBoard()
    {
        ActivateSquare();
    }

    public void ActivateSquareWhenContinue()
    {
        //ghostImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        SquareOccupied = true;
    }

    public void ActivateSquare()
    {
        ghostImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        SquareOccupied = true;
    }

    public void Activate()
    {
        
        activeImage.gameObject.SetActive(true);
        
    }

    public void DeactivateSquare()
    {
        ghostImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(false);
        Selected = false;
        SquareOccupied = false;
    }

    public void Deactivate()
    {
        hintImage.gameObject.SetActive(false);
        if (childAnimator != null)
        {
            childAnimator.SetTrigger("isActive");
            
        }
        Invoke("DeactivateGridSquare", 0.07f);
        //activeImage.gameObject.SetActive(false);
    }

    private void DeactivateGridSquare()
    {
        activeImage.gameObject.SetActive(false);
        //Vector3 worldPos = CanvasToWorldPosition((Vector2)this.transform.position);
        InstantiateParticleSystemAtPosition(this.transform.position);
        
    }

    void InstantiateParticleSystemAtPosition(Vector3 position)
    {
        // Tạo một GameObject mới để chứa Particle System
        GameObject particleObject = new GameObject("ParticleEffect");
        particleObject.transform.position = position;

        // Thêm Particle System vào GameObject
        ParticleSystem ps = Instantiate(particleSystemPrefab, particleObject.transform);
        if (ps != null)
        {
            var main = ps.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            ps.Play();
            StartCoroutine(DestroyParticleAfterPlay(particleObject, ps));
        }
     
    }

    IEnumerator DestroyParticleAfterPlay(GameObject particleObject, ParticleSystem ps)
    {
        while (ps != null && !ps.isStopped)
        {
            yield return null;
        }

        // Destroy the particle object
        if (particleObject != null)
        {
            Destroy(particleObject);
        }
    }

    public void ClearOccupied()
    {
        Selected = false;
        SquareOccupied = false;
    }

    public void ShowTip()
    {
        tipImage.gameObject.SetActive(true);

    }

    public void HideTip()
    {
        tipImage.gameObject.SetActive(false);
        
    }

    public void ShowHint()
    {
        hintImage.gameObject.SetActive(true);

    }

    public void HideHint()
    {
        hintImage.gameObject.SetActive(false);
    }

    public void SetImage()
    {
        normalImage.GetComponent<Image>().sprite = GridBase;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SquareOccupied == false && Grids.compareGhostAndSquare)
        {
            Selected = true;
            ghostImage.gameObject.SetActive(true);
        }
        else if ( collision.GetComponent<ShapeSquare>() != null)
        {
            Grids.compareGhostAndSquare = false;
            //collision.GetComponent<ShapeSquare>().EnableHoover();
        }
        if (Bombed == true)
        {
            bombedImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Selected = true;
        
        if (SquareOccupied == false && Grids.compareGhostAndSquare)
        {
            ghostImage.gameObject.SetActive(true);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            Grids.compareGhostAndSquare = false;
            //collision.GetComponent<ShapeSquare>().EnableHoover();
        }
        if (Bombed == true)
        {
            bombedImage.gameObject.SetActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (SquareOccupied == false)
        {
            Selected = false;
            ghostImage.gameObject.SetActive(false);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            Grids.compareGhostAndSquare = false;
            //collision.GetComponent<ShapeSquare>().DisableHoover();
        }
        if (Bombed == true)
        {
            bombedImage.gameObject.SetActive(false);
        }

    }
}
