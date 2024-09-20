//using DG.Tweening;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using static UnityEngine.RuleTile.TilingRuleOutput;

//public class Bomb : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
//{
//    public GameObject squareShapeImage;
//    public Vector3 shapeSelectedScale;
//    // Vector2 offset = new Vector2(0f, 700f);

//    [HideInInspector]
//    public ShapeData CurrentShapeData;

//    public int totalSquareNumber { get; set; }

//    private float current_z = 0;
//    private List<GameObject> _currentShape = new List<GameObject>();
//    private Vector3 _shapeStartScale;
//    private RectTransform _transform;
//    public bool _isDragging { get; set; }
//    private Canvas _canvas;
//    //private Vector3 _startPosition;
//    public Vector3 _startPosition { get; set; }
//    private bool _shapeActive = true;
//    private bool _isShapeDisable = true;

//    private Animator animator;

//    public float duration = 2f;
//    public float targetRotation = 90f;

//    private Quaternion startRotation;
//    private float elapsedTime = 0f;

//    GameObject shapeHolder;

//    public float rotateSpeed = 10;

//    public bool isClicked { get; set; }

//    public Vector3 lastPos { get; set; }

//    private GameObject shapeStorer;

//    private AudioManager audioManager;
//    [SerializeField]
//    private GameObject rotateLabel;
//    private bool _isPickUp;
//    public void Awake()
//    {
//        _shapeStartScale = this.GetComponent<RectTransform>().localScale;
//        _transform = this.GetComponent<RectTransform>();
//        _canvas = GetComponentInParent<Canvas>();
//        _isDragging = false;
//        _startPosition = _transform.localPosition;
//        _shapeActive = true;
//        _isShapeDisable = true;
//        isClicked = false;
//        animator = GetComponent<Animator>();


//        //rotateLabel = GetComponentInChildren<GameObject>();
//        shapeStorer = GameObject.Find("ShapeStorer");


//        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
//        if (audioManager == null)
//        {
//            Debug.Log("AUDIOMANAGER NOT FOUND");
//        }

//    }

   
//    // Start is called before the first frame update
//    void Start()
//    {
//        startRotation = _transform.transform.rotation;
        
//    }

//    //private void FixedUpdate()
//    //{
//    //    if (Event.rotateSwitch && CheckAnyActive() && !_isDragging)
//    //    {
//    //        rotateLabel.SetActive(true);
//    //        rotateLabel.GetComponent<RectTransform>().SetAsLastSibling();
//    //    }
//    //    else
//    //    {
//    //        rotateLabel.SetActive(false);
//    //    }
//    //}

//    public void OnPointerClick(PointerEventData eventData)
//    {

//    }

//    public void OnPointerUp(PointerEventData eventData)
//    {
//        isClicked = true;
//        _transform.anchorMin = new Vector2(0.5f, 0.5f);
//        _transform.anchorMax = new Vector2(0.5f, 0.5f);
//        _transform.pivot = new Vector2(0.5f, 0.5f);

        
//        //Event.CheckPlaced();
//        _isDragging = false;
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        _transform.SetAsLastSibling();
        
//        _isDragging = true;
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        isClicked = true;
//        _isDragging = true;
//        _transform.anchorMin = new Vector2(0.5f, 0.5f);
//        _transform.anchorMax = new Vector2(0.5f, 0.5f);
//        _transform.pivot = new Vector2(0.5f, 0.5f);
//        _transform.SetAsLastSibling();
//        Vector2 pos;
//        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);
//        _transform.DOLocalMove(pos, 0.1f).SetEase(Ease.OutExpo);
//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        isClicked = true;
//        _isDragging = false;

//        _transform.anchorMin = new Vector2(0.5f, 0.5f);
//        _transform.anchorMax = new Vector2(0.5f, 0.5f);
//        _transform.pivot = new Vector2(0.5f, 0.5f);

       
//        //Event.CheckPlaced();
//    }

//    public void OnPointerDown(PointerEventData eventData)
//    {
//        //gameObject.GetComponent<BoxCollider2D>().enabled = false;
//        _isDragging = true;

        

//        _transform.anchorMin = new Vector2(0.5f, 0.5f);
//        _transform.anchorMax = new Vector2(0.5f, 0.5f);
//        _transform.pivot = new Vector2(0.5f, 0.5f);
//        _transform.SetAsLastSibling();

//        Vector2 pos;
//        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);
//        _transform.DOLocalMove(pos, 0.1f).SetEase(Ease.OutExpo);

//        if (Event.rotatable)
//        {
//            if (!isClicked)
//            {
//                audioManager.PlaySFX(audioManager.blockRotate);
                
//                isClicked = true;
//            }
//        }
//    }

  

   
//}
