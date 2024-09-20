using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class ShapeStorer : MonoBehaviour
{
    public List<ShapeData> shapeData;
    public List<Shape> shapeList;

    //private List<int> shapeIndexList { get; set; }

    public List<int> shapeIndexList { get; set; }
    public Grids grids;

    [SerializeField]
    private UnityEvent onDisableShape;
    private void Awake()
    {
        shapeIndexList = PlayerPrefsHelper.LoadIntList("ShapeIndices");

        if (Event.emptyGrids)
        {
            int flag = 0;
            foreach (var shape in shapeList)
            {

                int shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
                shape.CreateShape(shapeData[shapeIndex]);
            }
        }
        else
        {

            // Nếu có dữ liệu đã lưu, sử dụng nó để khởi tạo các hình dạng
            if (shapeIndexList.Count > 0)
            {
                Debug.Log("x");
                for (int i = 0; i < shapeList.Count; i++)
                {
                    Debug.Log("x" + i);
                    // Đảm bảo rằng không vượt quá số lượng shapeList hoặc shapeIndices
                    if (i < shapeIndexList.Count)
                    {
                        shapeList[i].CreateShape(shapeData[shapeIndexList[i]]);


                        Debug.Log("y" + i);
                    }
                    else
                        Debug.Log("u" + i);

                }
            }
            else
            {
                // Nếu không có dữ liệu đã lưu, tạo hình dạng ngẫu nhiên
                Debug.Log("z");
                foreach (var shape in shapeList)
                {
                    int shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
                    shape.CreateShape(shapeData[shapeIndex]);
                }
            }



        }

        PlayerPrefsHelper.SaveIntList("ShapeIndices", shapeIndexList);
        //onDisableShape?.Invoke();
        //Event.rotateSwitch = false;

    }

    void Start()
    {
        //for (int index = 0; index < shapeList.Count; index++)
        //{
        //    var isShapeActive = shapeList[index].CheckAnyActive();

        //    Shape currentShape = shapeList[index]; // Lưu giá trị vào biến tạm thời


        //    if (!grids.CheckCanPlacedMoreOnGrid(ref currentShape, isShapeActive))
        //    {
        //        shapeList[index]?.DisableShapeSquares();
        //    }
        //}
        //shapeList[3].SetShapeInactive1();
        DeactivateAllRotateLabel();
    }

    private void OnDisable()
    {
        Event.RequestNewShapes -= RequestNewShapes;



    }

    private void OnEnable()
    {
        Event.RequestNewShapes += RequestNewShapes;

    }

    public void DeactivateAllRotateLabel()
    {
        shapeList[0].RotateLabelDeactive();
        shapeList[1].RotateLabelDeactive();
        shapeList[2].RotateLabelDeactive();
        shapeList[3].RotateLabelDeactive();
    }

    public Shape GetCurrentSelectedShape()
    {
        foreach (var shape in shapeList)
        {
            if (shape.IsOnStartPosition() == false && shape.CheckAnyActive())
            {
                return shape;
            }
        }

        //Debug.LogError("No Shape Selected!");
        return null;
    }

    public int GetCurrentIndexShape()
    {
        int index = 0;
        foreach (var shape in shapeList)
        {
            if (shape.IsOnStartPosition() == false && shape.CheckAnyActive())
            {
                return index;
            }
            index++;
        }

        //Debug.LogError("No Shape Selected!");
        return -1;
    }

    public ShapeData GetCurrentSelectedShapeData()
    {
        foreach (var shape in shapeList)
        {
            if (shape.IsOnStartPosition() == false && shape.CheckAnyActive())
            {
                return shape.CurrentShapeData;
            }
        }

        //Debug.LogError("No Shape Selected!");
        return null;
    }

    public int GetCurrentSelectedShapeDataIndex()
    {
        int index = 0;
        foreach (var shape in shapeList)
        {
            for (int i = 0; i < shapeData.Count; i++)
            {
                if (shapeData[i] == shape.CurrentShapeData)
                {
                    index = i;
                    break;
                }
            }
        }

        //Debug.LogError("No Shape Selected!");
        return index;
    }

    //private void RequestNewShapes()
    //{
    //    foreach (var shape in shapeList)
    //    {
    //        shape.isClicked = false;
    //        var shapeIndex = UnityEngine.Random.Range(0,shapeData.Count);   
    //        shape.RequestNewShape(shapeData[shapeIndex]);
    //    }
    //}

    private void RequestNewShapes()
    {

        DeactivateAllRotateLabel();
        // Tạo một từ điển để theo dõi số lần mỗi shapeIndex đã được chọn
        Dictionary<int, int> shapeIndexCounts = new Dictionary<int, int>();
        List<int> selectedShapeIndices = new List<int>(); // Tạo danh sách để lưu shapeIndex
        int flag = 0;
        foreach (var shape in shapeList)
        {

            int shapeIndex;

            // Tìm shapeIndex có thể sử dụng được (xuất hiện dưới 2 lần)
            //if (flag < 3)
            do
            {
                shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
            }
            while (shapeIndexCounts.ContainsKey(shapeIndex) && shapeIndexCounts[shapeIndex] >= 2);
            if (flag == 3)
            {
                shapeIndex = GetCurrentSelectedShapeDataIndex();
                selectedShapeIndices.Add(shapeIndex);
            }

            // Cập nhật số lần xuất hiện của shapeIndex
            if (shapeIndexCounts.ContainsKey(shapeIndex))
            {
                shapeIndexCounts[shapeIndex]++;
            }
            else
            {
                shapeIndexCounts[shapeIndex] = 1;
            }

            
            
            //selectedShapeIndices.Add(shapeIndex);
            //shape.RequestNewShape(shapeData[shapeIndex]);
            if (flag < 3)
            {
                selectedShapeIndices.Add(shapeIndex);
                // Gán shapeData cho shape và tạo hình dạng mới
                shape.RequestNewShape(shapeData[shapeIndex]);
            }
            else if (flag == 3 && shape.CheckAnyActive()) { shape.RequestNewShape(shapeData[shapeIndex]); }

            flag++;
        }
        //if (shapeList[3].CheckAnyActive())
        //    selectedShapeIndices.Add(GetShapeDataIndex(GetCurrentSelectedShapeData()));
        //shapeList[3].SetShapeInactive1();
        shapeIndexList = selectedShapeIndices;
        // Lưu danh sách shapeIndex bằng PlayerPrefsHelper
        PlayerPrefsHelper.SaveIntList("ShapeIndices", shapeIndexList);
        //shapeList[2].SetShapeInactive1();
        
    }
}
