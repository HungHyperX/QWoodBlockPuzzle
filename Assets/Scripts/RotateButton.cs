using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideButtonAfterTenClicks : MonoBehaviour
{
    public Button button; // Tham chiếu tới Button component
    [SerializeField]
    private Text clickCountText;
    private Animator animator;
    public List<Shape> shapeList; // Danh sách các đối tượng shape

    void Start()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        animator = GetComponentInChildren<Animator>();
        // Gán hàm xử lý sự kiện nhấn nút
        button.onClick.AddListener(OnButtonClick);

        UpdateClickCountText();
        if (Event.clickCount < 0)
        {
            button.interactable = false;
            Event.rotateOut = true;
        }
    }

    void OnButtonClick()
    {
        animator.SetTrigger("Rotate");
        if (!Event.rotateSwitch)
        {
            Event.rotatable = true;
            Event.rotateAni = true;

            Event.clickCount--;
            Event.rotateSwitch = true;
        }

        // Gọi hàm kiểm tra sau khi nhấn nút
        CheckAndRotateLabels();

        if (Event.clickCount < 0)
        {
            button.interactable = false;
            Event.rotateOut = true;
        }

        UpdateClickCountText();
    }

    // Hàm kiểm tra và xoay nhãn
    private void CheckAndRotateLabels()
    {
        bool shouldDeactivate = !Event.rotateSwitch;

        foreach (var shape in shapeList)
        {
            // Kiểm tra xem có shape nào đang kéo không
            if (shape._isDragging)
            {
                shouldDeactivate = true;
                break;
            }
        }

        // Xoay nhãn nếu điều kiện cho phép
        if (!shouldDeactivate)
        {
            foreach (var shape in shapeList)
            {
                if (shape.CheckAnyActive() && !shape._isDragging)
                {
                    shape.RotateLabelActive();
                }
            }
        }
        else
        {
            DeactivateAllRotateLabel();
        }
    }

    // Hàm cập nhật số lần nhấn hiển thị trên UI
    void UpdateClickCountText()
    {
        if (clickCountText != null)
        {
            clickCountText.text = "" + (Event.clickCount + 1);
        }
    }

    // Hàm vô hiệu hóa tất cả nhãn xoay
    private void DeactivateAllRotateLabel()
    {
        foreach (var shape in shapeList)
        {
            shape.RotateLabelDeactive();
        }
    }
}
