using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public GameObject squareShapeImage;
    public Vector3 shapeSelectedScale;
    public Vector2 offset = new Vector2(0f, 1200f);

    [HideInInspector]
    public ShapeData CurrentShapeData;

    public int totalSquareNumber { get; set; }

    private float current_z = 0;
    private List<GameObject> _currentShape = new List<GameObject>();
    private Vector3 _shapeStartScale;
    private RectTransform _transform;
    public bool _isDragging { get; set; }
    private Canvas _canvas;
    //private Vector3 _startPosition;
    public Vector3 _startPosition { get; set; }
    private bool _shapeActive = true;
    private bool _isShapeDisable = true;

    private Animator animator;

    public float duration = 2f;
    public float targetRotation = 90f;

    private Quaternion startRotation;
    private float elapsedTime = 0f;

    GameObject shapeHolder;

    public float rotateSpeed = 10;

    public bool isClicked { get; set; }

    public Vector3 lastPos {  get; set; }

    private GameObject shapeStorer;

    private AudioManager audioManager;
    [SerializeField]
    private GameObject rotateLabel;
    [SerializeField]
    private UnityEvent onHintDragging;
    private bool _isPickUp;
    public void Awake()
    {
        _shapeStartScale = this.GetComponent<RectTransform>().localScale;
        _transform = this.GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _isDragging = false;
        _startPosition = _transform.localPosition;
        _shapeActive = true;
        _isShapeDisable = true;
        isClicked = false;
        animator = GetComponent<Animator>();
       
       
        //rotateLabel = GetComponentInChildren<GameObject>();
        //shapeStorer = GameObject.Find("ShapeStorer");

        
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.Log("AUDIOMANAGER NOT FOUND");
        }

    }

    private void OnDisable()
    {
        Event.BackToStartPosition -= BackToStartPosition;
        Event.SetShapeInactive -= SetShapeInactive;
        
    }

    private void OnEnable()
    {
        Event.BackToStartPosition += BackToStartPosition;
        Event.SetShapeInactive += SetShapeInactive;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        startRotation = _transform.transform.rotation;
        RotateLabelDeactive();
        
    }

    //private void FixedUpdate()
    //{
    //    if (Event.rotateSwitch && CheckAnyActive() && !_isDragging)
    //    {
    //        rotateLabel.SetActive(true);
    //        rotateLabel.GetComponent<RectTransform>().SetAsLastSibling();
    //    }
    //    else
    //    {
    //        rotateLabel.SetActive(false);
    //    }
    //}

    public void RotateLabelActive()
    {
        rotateLabel.SetActive(true);
        rotateLabel.GetComponent<RectTransform>().SetAsLastSibling();
        rotateLabel.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void RotateLabelDeactive()
    {
        rotateLabel.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
        rotateLabel.SetActive(false);
        
    }

    public bool IsOnStartPosition() // detect shape is moved or not
    {
        return _transform.localPosition == _startPosition;
    }

    public bool CheckAnyActive()
    {
        foreach (var square in _currentShape)
        {
            if (square.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    public void DeactivateShape()
    {
        if (_shapeActive)
        {
            foreach (var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().DeactivateSquare(); // square? : check if square is active or not
            }
        }
        _shapeActive = false;
    }
    public void EnableShape()
    {
        int count = 0;
        if (!_isShapeDisable)
        {
            foreach (var square in _currentShape)
            {
                Debug.Log("Number Of Enabled Shapes: " + count);
                square.gameObject.GetComponent<ShapeSquare>().EnableShapeSquare();
            }
        }
        _isShapeDisable = true; 
    }
    public void DisableShapeSquares()
    {
        int count = 0;
        foreach (var square in _currentShape)
        {
            count++;
            Debug.Log("Number Of Disabled Shapes: " + count);
            square.gameObject.GetComponent<ShapeSquare>().DisableShapeSquare();
        }
    }
    public void DisableShape()
    {
        
        if (_isShapeDisable)
        {
            DisableShapeSquares();
        }
        _isShapeDisable = false;
    }

    private void SetShapeInactive()
    {
        if (IsOnStartPosition() == false && CheckAnyActive())
        {
            foreach (var square in _currentShape)
            {
                square.gameObject.SetActive(false);
            }

        }
    }

    public void SetShapeInactive1()
    {
            foreach (var square in _currentShape)
            {
                square.gameObject.SetActive(false);
            }
    }
    public void ActivateShape()
    {
        if (!_shapeActive)
        {
            foreach (var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().ActivateSquare();
            }
        }

        _shapeActive = true;
    }

    public void RequestNewShape(ShapeData shapeData)
    {
        
        lastPos = _transform.localPosition;
        _transform.localPosition = _startPosition; // make new shape appear in where the previous shape was
        _transform.rotation = Quaternion.Euler(0, 0, 0);
       
        CreateShape(shapeData);
        //BackToStartPosition();
    }

    public void CreateShape(ShapeData shapeData)
    {
        CurrentShapeData = shapeData;
        totalSquareNumber = GetNumberOfSquares(shapeData);

        while (_currentShape.Count < totalSquareNumber)
        {
            _currentShape.Add(Instantiate(squareShapeImage, transform) as GameObject);
        }

        foreach (var square in _currentShape)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x
            , squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInList = 0;

        // Set positions to display final shape
        for (var row = 0; row < shapeData.rows; row++)
        {
            for (var column = 0; column < shapeData.columns; column++)
            {
                if (shapeData.board[row].column[column])
                {
                    _currentShape[currentIndexInList].SetActive(true);
                    _currentShape[currentIndexInList].GetComponent<RectTransform>().localPosition =
                        new Vector2(GetXPositionForShapeSquare(shapeData, column, moveDistance), GetYPositionForShapeSquare(shapeData, row, moveDistance));

                    currentIndexInList++;
                }
            }

        }
    }

    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        float shiftOnX = 0f;
        if (shapeData.columns > 1)
        {
            float startXPos;
            if (shapeData.columns % 2 != 0)
                startXPos = (shapeData.columns / 2) * moveDistance.x * -1;
            else
                startXPos = ((shapeData.columns / 2) - 1) * moveDistance.x * -1 - moveDistance.x / 2;
            shiftOnX = startXPos + column * moveDistance.x;

        }
        return shiftOnX;
    }

    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float shiftOnY = 0f;
        if (shapeData.rows > 1)
        {
            float startYPos;
            if (shapeData.rows % 2 != 0)
                startYPos = (shapeData.rows / 2) * moveDistance.y;
            else
                startYPos = ((shapeData.rows / 2) - 1) * moveDistance.y + moveDistance.y / 2;
            shiftOnY = startYPos - row * moveDistance.y;
        }
        return shiftOnY;
    }


    private int GetNumberOfSquares(ShapeData shapeData) // Count the square that is active in ShapeData
    {
        int number = 0;

        foreach (var rowData in shapeData.board)
        {
            foreach (var active in rowData.column)
            {
                if (active)
                    number++;
            }
        }

        return number;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
        //if (Event.rotatable)
        //{


        //    if (!isClicked)
        //    {

        //        audioManager.PlaySFX(audioManager.blockRotate);
        //        StartCoroutine(RotateObjectCoroutine(Vector3.forward, 90, duration));
        //        isClicked = true;

        //    }

        //    if (_transform.rotation != Quaternion.Euler(Vector3.zero))
        //    {
        //        Event.rotatateRemain = false;
        //    }
        //    else
        //    {
        //        Event.rotatateRemain = true;
        //    }
        //}

        //dragLock = false;

        //StartCoroutine(RotateObjectCoroutine(Vector3.forward, 90, duration));
        //Debug.Log("Clicked");

    }


    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    isClicked = true;
    //    _transform.anchorMin = new Vector2(0.5f, 0.5f);
    //    _transform.anchorMax = new Vector2(0.5f, 0.5f);
    //    _transform.pivot = new Vector2(0.5f, 0.5f);
    //    this.GetComponent<RectTransform>().localScale = _shapeStartScale;
    //    //BackToStartPosition();
    //    Event.CheckPlaced();
    //}

    ////public void OnBeginDrag(PointerEventData eventData)
    ////{
    ////    this.GetComponent<RectTransform>().localScale = shapeSelectedScale;
    ////    //_transform.localPosition += (Vector3)offset;


    ////}

    //public void OnDrag(PointerEventData eventData)
    //{

    //    isClicked = true;
    //    _transform.anchorMin = new Vector2(0.5f, 0.5f);
    //    _transform.anchorMax = new Vector2(0.5f, 0.5f);
    //    _transform.pivot = new Vector2(0.5f, 0.5f);

    //    Vector2 pos;
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);
    //    _transform.localPosition = pos + offset;

    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    isClicked = true;
    //    _transform.anchorMin = new Vector2(0.5f, 0.5f);
    //    _transform.anchorMax = new Vector2(0.5f, 0.5f);
    //    _transform.pivot = new Vector2(0.5f, 0.5f);
    //    this.GetComponent<RectTransform>().localScale = _shapeStartScale;
    //    Event.CheckPlaced();

    //}

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    this.GetComponent<RectTransform>().localScale = shapeSelectedScale;

    //    _transform.anchorMin = new Vector2(0.5f, 0.5f);
    //    _transform.anchorMax = new Vector2(0.5f, 0.5f);
    //    _transform.pivot = new Vector2(0.5f, 0.5f);

    //    Vector2 pos;
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);
    //    _transform.localPosition = pos + offset;


    //    if (Event.rotatable)
    //    {
    //        //if (!Event.rotateLock)
    //        //{
    //        //    Event.timeRotate--;
    //        //    Debug.Log(Event.timeRotate);
    //        //    Event.rotateLock = true;
    //        //}

    //        if (!isClicked)
    //        {

    //            audioManager.PlaySFX(audioManager.blockRotate);
    //            StartCoroutine(RotateObjectCoroutine(Vector3.forward, 90, duration));
    //            isClicked = true;

    //        }

    //        //if (_transform.rotation != Quaternion.Euler(Vector3.zero))
    //        //{
    //        //    Event.rotatateRemain = false;
    //        //}
    //        //else
    //        //{
    //        //    Event.rotatateRemain = true;
    //        //}
    //    }
    //    //_transform.localPosition += (Vector3)offset;
    //}

    //private void BackToStartPosition()
    //{
    //    isClicked = false;
    //    audioManager.PlaySFX(audioManager.blockWithdraw);
    //    _transform.transform.localPosition = _startPosition;
    //}

    IEnumerator RotateObjectCoroutine(Vector3 axis, float angle, float duration) // Nội suy nội tuyến
    {
        
        float elapsed = 0.0f;
        Quaternion startRotation = transform.rotation; // Góc xoay ban đầu
        //float initialLocalPosY = _transform.localPosition.y; // Lưu giá trị localPosition.y ban đầu

        while (elapsed < duration)
        {
            isClicked = true;
            float rotateAngle = Mathf.Lerp(0, angle, elapsed / duration);
            Quaternion targetRotation = Quaternion.Euler(axis * rotateAngle);
            transform.rotation = startRotation * targetRotation; // Áp dụng góc xoay vào transform.rotation
            //_transform.localPosition = new Vector3(_transform.localPosition.x, initialLocalPosY, _transform.localPosition.z); // Giữ nguyên giá trị localPosition.y
            elapsed += Time.deltaTime;
            yield return null;
        }

        
        transform.rotation = startRotation * Quaternion.Euler(axis * angle);
        //isClicked = false;
        //isClicked = true;
        //_transform.localPosition = new Vector3(_transform.localPosition.x, initialLocalPosY, _transform.localPosition.z); // Đảm bảo giá trị localPosition.y sau khi xoay
    }

    

    public void OnBeginDrag(PointerEventData eventData)
    {
        _transform.SetAsLastSibling();
        _transform.DOScale(shapeSelectedScale, 0.1f);
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isClicked = true;
        _isDragging = true;
        _transform.anchorMin = new Vector2(0.5f, 0.5f);
        _transform.anchorMax = new Vector2(0.5f, 0.5f);
        _transform.pivot = new Vector2(0.5f, 0.5f);
        _transform.SetAsLastSibling();
        onHintDragging?.Invoke();

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);
        _transform.DOLocalMove(pos + offset, 0.1f).SetEase(Ease.OutExpo);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isClicked = true;
        _isDragging = false;

        _transform.anchorMin = new Vector2(0.5f, 0.5f);
        _transform.anchorMax = new Vector2(0.5f, 0.5f);
        _transform.pivot = new Vector2(0.5f, 0.5f);

        _transform.DOScale(_shapeStartScale, 0.1f);
        //Event.CheckPlaced();
    }
    //public void OnPointerUp(PointerEventData eventData) { }
    //public void OnPointerDown(PointerEventData eventData) { }
    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = true;
        _transform.anchorMin = new Vector2(0.5f, 0.5f);
        _transform.anchorMax = new Vector2(0.5f, 0.5f);
        _transform.pivot = new Vector2(0.5f, 0.5f);

        _transform.DOScale(_shapeStartScale, 0.1f);
        _isDragging = false;
        Event.CheckPlaced();

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //gameObject.GetComponent<BoxCollider2D>().enabled = false;
        _isDragging = true;

        _transform.DOScale(shapeSelectedScale, 0.05f);

        _transform.anchorMin = new Vector2(0.5f, 0.5f);
        _transform.anchorMax = new Vector2(0.5f, 0.5f);
        _transform.pivot = new Vector2(0.5f, 0.5f);
        //_transform.SetAsLastSibling();
        onHintDragging?.Invoke();
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);
        _transform.DOLocalMove(pos + offset, 0.1f).SetEase(Ease.OutExpo);

        if (Event.rotatable)
        {
            if (!isClicked)
            {
                //if (gameObject.tag == "ShapePlay")
                //{
                    audioManager.PlaySFX(audioManager.blockRotate);
                    StartCoroutine(RotateObjectCoroutine(Vector3.forward, -90, duration));
                    isClicked = true;
                //}
            }
        }
    }

    private void BackToStartPosition()
    {
        isClicked = false;
        //_isDragging = false;
        audioManager.PlaySFX(audioManager.blockWithdraw);
        _transform.DOLocalMove(_startPosition, 0.1f).SetEase(Ease.OutExpo);
    }

    public float rotationSpeed = 100f; // Tốc độ xoay

    private IEnumerator RotateContinuously()
    {
        while (true) // Vòng lặp vô hạn
        {
            // Xoay vật thể xung quanh trục Z với tốc độ rotationSpeed mỗi giây
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            // Đợi 1 frame và tiếp tục vòng lặp
            yield return null;
        }
    }
}
