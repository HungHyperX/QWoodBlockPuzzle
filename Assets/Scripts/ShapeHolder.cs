using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ShapeHolder : MonoBehaviour
{
    //public Image ghostImage;
    //public Image activeImage;
    //public Image normalImage;
    //private RectTransform _transform;
    //public SpriteRenderer Renderer;

    //[SerializeField]
    //private Shape _shape;
    public bool Selected { get; set; }
    public bool _touch { get; set; }
    public bool SquareOccupied { get; set; }

    public ShapeStorer shapeStorer;

    public Shape shapeForHold;

    public UnityEvent onPlaceHoldThenSave;

    public UnityEvent onRenewShapes;

    private ShapeData shapeForHoldData;

    public bool InHold { get; set; }
    private void OnDisable()
    {
        Event.CheckPlaced
            -= CheckInHold;
    }

    private void OnEnable()
    {
        Event.CheckPlaced += CheckInHold;
    }


    // Start is called before the first frame update
    void Start()
    {
        //_transform = this.GetComponent<RectTransform>();
        Selected = false;
        SquareOccupied = false;
        _touch = false;
        InHold = false;
        //shapeForHold.SetShapeInactive1();
    }

    private void OnTriggerEnter2D(Collider2D collision) // when enter trigger collider
    {
        
        _touch = true;
    }
    private void OnTriggerStay2D(Collider2D collision) // when stay on trigger collider
    {
        
        _touch = true;
        //Event.CheckPlaced();
    }

    private void OnTriggerExit2D(Collider2D collision) // when exit trigger collider
    {
        _touch = false;
    }

    private void CheckInHold()
    {
        if (shapeForHold.CheckAnyActive() == false && _touch)
        {
            InHold = true;
            Debug.Log("PlaceHold");
            shapeForHoldData = shapeStorer.GetCurrentSelectedShapeData();
            shapeForHold.RequestNewShape(shapeForHoldData);
            shapeStorer.GetCurrentSelectedShape().SetShapeInactive1();
            //Event.CheckPlaced();
            if (shapeStorer.shapeList[0].CheckAnyActive() == false && shapeStorer.shapeList[1].CheckAnyActive() == false && shapeStorer.shapeList[2].CheckAnyActive() == false)
            {
                onRenewShapes?.Invoke();
            }
            onPlaceHoldThenSave?.Invoke();
        }
        
    }
    public ShapeData GetCurrentShapeDataIndexForHold()
    {
        return shapeForHoldData;
    }
}
