using DG.Tweening;
using MoreMountains.NiceVibrations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Grids : MonoBehaviour
{
    [Tooltip("Event")]
    public UnityEvent onRotate;
    public UnityEvent onFinishGridSquareDrop;
    public ShapeStorer shapeStorer;
    public int columns = 10;
    public int rows = 10;
    public float squaresGap = 0.1f;
    public GameObject gridSquare;
    public Vector2 startPosition = new(0.0f, 0.0f);
    public float squareScale = 0.5f;
    public float everySquareOffset = 0.0f;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();

    private LineIndicate _lineIndicate;

    public int lineFullNum { get; set; }

    //private Animator _animator;

    private AudioManager audioManager;

    [SerializeField]
    public SwitchToggle switchToggle;

    private bool vibrate;

    public static bool compareGhostAndSquare = false;

    //rivate int multiplier = 0;

    private void OnDisable()
    {
        Event.CheckPlaced -= CheckPlaced;
    }

    private void OnEnable()
    {
        Event.CheckPlaced += CheckPlaced;
    }

    private void Awake()
    {
        Event.rotateSwitch = false;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.Log("AUDIOMANAGER NOT FOUND");
        }

    }

    void Start()
    {
       
        _lineIndicate = GetComponent<LineIndicate>();
        if (_lineIndicate == null)
        {
            Debug.LogError("LineIndicate is not found on the GameObject");
        }
        CreateGrid();

        if (!PlayerPrefs.HasKey("InactiveShapeIndexes"))
        {
            PlayerPrefsHelper.SetIntList("InactiveShapeIndexes", new List<int>() { 3 });
        }
        List<int> inactiveShapeIndexes = PlayerPrefsHelper.LoadIntList("InactiveShapeIndexes");
        for (int index = 0; index < shapeStorer.shapeList.Count; index++)
        {
            if (inactiveShapeIndexes.Contains(index))
            {
                shapeStorer.shapeList[index].SetShapeInactive1();
                
            }
        }

        if (Event.emptyGrids)
        {
            shapeStorer.shapeList[3].SetShapeInactive1();
        }
    }

    private int frameCounter = 0;
    private int targetFrameInterval = 5;

    void FixedUpdate()
    {
        //CheckGameOver();



        frameCounter++;
        if (frameCounter >= targetFrameInterval)
        {
            var isShapeActive = shapeStorer.shapeList[0].CheckAnyActive();

            Shape currentShape = shapeStorer.shapeList[0]; // Lưu giá trị vào biến tạm thời

            if (CheckCanPlacedMoreOnGrid(ref currentShape, isShapeActive) && isShapeActive)
            {
                shapeStorer.shapeList[0]?.EnableShape();
                //shapeStorer.shapeList[0]?.ActivateShape();

            }
            else if (!CheckCanPlacedMoreOnGrid(ref currentShape, isShapeActive))
            {
                shapeStorer.shapeList[0]?.DisableShape();
            }

            isShapeActive = shapeStorer.shapeList[1].CheckAnyActive();

            currentShape = shapeStorer.shapeList[1]; // Lưu giá trị vào biến tạm thời

            if (CheckCanPlacedMoreOnGrid(ref currentShape, isShapeActive) && isShapeActive)
            {
                shapeStorer.shapeList[1]?.EnableShape();
                //shapeStorer.shapeList[1]?.ActivateShape();

            }
            else if (!CheckCanPlacedMoreOnGrid(ref currentShape, isShapeActive))
            {
                shapeStorer.shapeList[1]?.DisableShape();
            }

            isShapeActive = shapeStorer.shapeList[2].CheckAnyActive();

            currentShape = shapeStorer.shapeList[2]; // Lưu giá trị vào biến tạm thời

            if (CheckCanPlacedMoreOnGrid(ref currentShape, isShapeActive) && isShapeActive)
            {
                shapeStorer.shapeList[2]?.EnableShape();
                //shapeStorer.shapeList[2]?.ActivateShape();

            }
            else if (!CheckCanPlacedMoreOnGrid(ref currentShape, isShapeActive))
            {
                shapeStorer.shapeList[2]?.DisableShape();
            }

            isShapeActive = shapeStorer.shapeList[3].CheckAnyActive();

            currentShape = shapeStorer.shapeList[3]; // Lưu giá trị vào biến tạm thời

            if (CheckCanPlacedMoreOnGrid(ref currentShape, isShapeActive) && isShapeActive)
            {
                shapeStorer.shapeList[3]?.EnableShape();
                //shapeStorer.shapeList[3]?.ActivateShape();

            }
            else if (!CheckCanPlacedMoreOnGrid(ref currentShape, isShapeActive))
            {
                shapeStorer.shapeList[3]?.DisableShape();
            }

            //// Thực hiện hành động mỗi 3 frame
            //var squareIndexes = new List<int>();

            //foreach (var square in _gridSquares)
            //{
            //    var gridSquare = square.GetComponent<GridSquare>();

            //    if (gridSquare.Selected && !gridSquare.SquareOccupied)
            //    {
            //        squareIndexes.Add(gridSquare.SquareIndex);
            //        gridSquare.Selected = false;
            //    }
            //}

            //if (shapeStorer == null)
            //{
            //    Debug.LogError("ShapeStorer is not assigned");
            //    return;
            //}

            //var currentSelectedShape = shapeStorer.GetCurrentSelectedShape();
            //if (currentSelectedShape == null) return; // No selected shape

            //if (currentSelectedShape.totalSquareNumber == squareIndexes.Count) // Check ghost and shape have same number of squares
            //{
            //    compareGhostAndSquare = true;
            //    //UpdateHintLines();
            //}
            //else
            //{
            //    compareGhostAndSquare = false;

            //}


            //UpdateHintLines();

            // Reset bộ đếm frame
            frameCounter = 0;
        }

    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPositions();
        StartCoroutine("DisableShapes", 1f);
    } 

    public void MagnetDownGrids()
    {
        audioManager.PlaySFX(audioManager.magnetDown);
        MoveActivatedSquaresDown1();
        //CheckPlaced();
        //CheckFullLine();
    }

    private void MoveActivatedSquaresDown1()
    {
        StartCoroutine(MoveSquaresDownCoroutine());
        
    }

    private IEnumerator MoveSquaresDownCoroutine()
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = rows - 2; row >= 0; row--) // Start from second-to-last row upwards
            {
                int squareIndex = row * columns + col;
                var currentSquare = _gridSquares[squareIndex].GetComponent<GridSquare>();

                if (currentSquare.SquareOccupied)
                {
                    int targetRow = row;
                    while (targetRow < rows - 1)
                    {
                        int belowSquareIndex = (targetRow + 1) * columns + col;
                        var belowSquare = _gridSquares[belowSquareIndex].GetComponent<GridSquare>();

                        if (belowSquare.SquareOccupied)
                        {
                            break; // Stop if the square below is occupied
                        }

                        // Move the occupied state to the square below
                        belowSquare.SquareOccupied = true;
                        currentSquare.SquareOccupied = false;

                        // Update the visual state
                        belowSquare.ActivateSquare();
                        currentSquare.DeactivateSquare();

                       
                        yield return new WaitForSeconds(0.001f);

                        // Set currentSquare to the square below and continue moving down
                        currentSquare = belowSquare;
                        targetRow++;
                    }
                }
            }
        }
        CheckFullLine();
        List<int> activatedSquares = GetActivatedSquares();
        SaveInactiveShapeIndexes();
        PlayerPrefsHelper.SaveIntList("myNumbers", activatedSquares);
        PlayerPrefsHelper.SaveIntList("ShapeIndices", shapeStorer.shapeIndexList);
        onFinishGridSquareDrop?.Invoke();
        
    }
    private void MoveActivatedSquaresDown()
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = rows - 2; row >= 0; row--) // Start from second-to-last row upwards
            {
                int squareIndex = row * columns + col;
                var currentSquare = _gridSquares[squareIndex].GetComponent<GridSquare>();

                if (currentSquare.SquareOccupied)
                {
                    int targetRow = row;
                    while (targetRow < rows - 1)
                    {
                        int belowSquareIndex = (targetRow + 1) * columns + col;
                        var belowSquare = _gridSquares[belowSquareIndex].GetComponent<GridSquare>();

                        if (belowSquare.SquareOccupied)
                        {
                            break; // Stop if the square below is occupied
                        }
                        targetRow++;


                        if (targetRow != row)
                        {
                            int targetSquareIndex = targetRow * columns + col;
                            var targetSquare = _gridSquares[targetSquareIndex].GetComponent<GridSquare>();

                            // Move the occupied state to the target square
                            targetSquare.SquareOccupied = true;
                            currentSquare.SquareOccupied = false;

                            // Update the visual state
                            targetSquare.ActivateSquare();
                            currentSquare.DeactivateSquare();
                        }
                    }
                }
            }
            
        }
        //CheckFullLine();
    }



    private void SpawnGridSquares()
    {
        List<int> loadedNumbers = PlayerPrefsHelper.LoadIntList("myNumbers");
        //int loaded_index = 0;
        int square_index = 0;
        for (var row = 0; row < rows; row++)
        {
            for (var column = 0; column < columns; column++)
            {
                var newSquare = Instantiate(gridSquare) as GameObject;
                if (newSquare != null)
                {
                    _gridSquares.Add(newSquare);
                    var gridSquareComponent = newSquare.GetComponent<GridSquare>();
                    if (gridSquareComponent != null)
                    {
                        gridSquareComponent.SquareIndex = square_index;
                        if (loadedNumbers.Contains(square_index))
                        {
                            //gridSquareComponent.ActivateSquare();
                            //_gridSquares[square_index].GetComponent<GridSquare>().PlaceShapeOnBoard();
                            gridSquareComponent.ActivateSquareWhenContinue();
                            //loaded_index++;
                        }
                        else
                        {
                            gridSquareComponent.SetImage();
                        }
                    }
                    newSquare.transform.SetParent(this.transform);
                    newSquare.transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                    square_index++;
                }
            }
        }
        
    }
    private void SetGridSquaresPositions()
    {
        if (_gridSquares.Count == 0) return;

        int column_number = 0;
        int row_number = 0;
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;

        var square_rect = _gridSquares[0].GetComponent<RectTransform>();

        _offset.x = square_rect.rect.width * square_rect.transform.localScale.x + everySquareOffset;
        _offset.y = square_rect.rect.height * square_rect.transform.localScale.y + everySquareOffset;

        foreach (GameObject square in _gridSquares)
        {
            if (column_number + 1 > columns)
            {
                square_gap_number.x = 0;

                column_number = 0;
                row_number++;
                row_moved = true;
            }

            var pos_x_offset = _offset.x * column_number + (square_gap_number.x * squaresGap);
            var pos_y_offset = _offset.y * row_number + (square_gap_number.y * squaresGap);

            if (column_number > 0 && column_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squaresGap;
            }

            if (row_number > 0 && row_number % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += squaresGap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset, 0.0f);

            column_number++;
        }
    }

    private void CheckPlaced()
    {
       
        var squareIndexes = new List<int>();

        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();

            if (gridSquare.Selected && !gridSquare.SquareOccupied)
            {
                squareIndexes.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
            }
        }
        var currentSelectedShape = shapeStorer.GetCurrentSelectedShape();
        if (currentSelectedShape == null) return; // No selected shape

        if (currentSelectedShape.totalSquareNumber == squareIndexes.Count) // Check ghost and shape have same number of squares
        {
            
            audioManager.PlaySFX(audioManager.blockPutDown);

            foreach (var squareIndex in squareIndexes)
            {
                _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard();
            }

            // Check how many shape left in the shapeStorer
            int shapeLeft = 0;
            int flag = 0;
            foreach (var shape in shapeStorer.shapeList)
            {
                
                if (flag == 3)
                {
                    break;
                }
                if (shape.IsOnStartPosition() && shape.CheckAnyActive())
                {
                    shapeLeft++;
                }
                flag++;
            }
            //if (shapeStorer.shapeList[3].CheckAnyActive() && shapeLeft > 3)
            //{
            //    shapeLeft--;
            //}
            Debug.Log("ShapeLeft: " + shapeLeft);

            if (shapeLeft == 0)
            {
                RenewShapes();
                Debug.Log("ShapeLeft: " + shapeLeft);
            }
            else if (shapeLeft > 0)
            {
                //if (currentSelectedShape.IsOnStartPosition() == false && currentSelectedShape.CheckAnyActive())
                //{
                //    currentSelectedShape.SetShapeInactive1();
                //    //Event.SetShapeInactive();
                //}
                
                Event.SetShapeInactive();
            }
            Debug.Log("Shape3: " + shapeStorer.shapeList[3].CheckAnyActive()); 

            int index = shapeStorer.GetCurrentIndexShape();
            string active_s = "" + currentSelectedShape.CheckAnyActive();
            
            Event.rotateSwitch = false;
            Event.rotatable = false;
            if (Event.rotateAni)
            {
                onRotate?.Invoke();
                Event.rotateAni = false;
                shapeStorer.DeactivateAllRotateLabel();
            }
            CheckFullLine();
            SaveDataBoard();
        }
        else
        {
            Event.BackToStartPosition();
        }
    }

    public void SaveDataBoard()
    {
        List<int> activatedSquares = GetActivatedSquares();
        SaveInactiveShapeIndexes();
        //Debug.Log("Activated squares: " + string.Join(", ", activatedSquares));
        PlayerPrefsHelper.SaveIntList("myNumbers", activatedSquares);
        Debug.Log("Activated squares: " + string.Join(", ", shapeStorer.shapeIndexList));
        PlayerPrefsHelper.SaveIntList("ShapeIndices", shapeStorer.shapeIndexList);
    }

    public void RenewShapes()
    {
        Event.BackToStartPosition();

        do
        {
            Event.RequestNewShapes();

        } while (CheckGameOverWhenRequestNewShapes());
    }

    private void SaveInactiveShapeIndexes()
    {
        PlayerPrefsHelper.SetEmptyList("InactiveShapeIndexes");
        List<int> inactiveShapeIndexes = new List<int>();
        for (int index = 0; index < shapeStorer.shapeList.Count; index++)
        {
            var isShapeActive = shapeStorer.shapeList[index].CheckAnyActive();
            if (!isShapeActive)
            {
                inactiveShapeIndexes.Add(index);
            }
        }
        Debug.Log("Activated squares: " + string.Join(", ", inactiveShapeIndexes));
        PlayerPrefsHelper.SaveIntList("InactiveShapeIndexes", inactiveShapeIndexes);
    }
    private List<int> GetActivatedSquares()
    {
        List<int> activatedSquares = new List<int>();

        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            if (gridSquare.SquareOccupied)
            {
                activatedSquares.Add(gridSquare.SquareIndex);
            }
        }

        return activatedSquares;
    }


    void CheckFullLine()
    {
        if (_lineIndicate == null)
        {
            Debug.LogError("LineIndicate is not assigned or found");
            return;
        }

        ClearTip();

        List<int[]> lines = new List<int[]>();

        // columns
        foreach (var column in _lineIndicate.columnIndexes)
        {
            lines.Add(_lineIndicate.GetVerticalLine(column));
        }

        // rows
        for (var row = 0; row < 10; row++)
        {
            List<int> data = new List<int>(10);
            for (var index = 0; index < 10; index++)
            {
                data.Add(_lineIndicate.line_data[row, index]);
            }

            lines.Add(data.ToArray());
        }

        var fullLines = CheckFullSquares(lines);

        //Debug.Log("FullLine = " + fullLines);
        if (fullLines > 0)
        {
            bool _vir = true; // Giá trị mặc định nếu không tìm thấy

            if (PlayerPrefs.HasKey("VIBRATE_ON_OFF"))
            {
                string vir_s = PlayerPrefs.GetString("VIBRATE_ON_OFF");
                if (!bool.TryParse(vir_s, out _vir))
                {
                    Debug.LogWarning($"Invalid BGM_ON_OFF value: {vir_s}. Defaulting to true.");
                }
            }

            //string vibrate_s = PlayerPrefs.GetString("VIBRATE_ON_OFF");
            //vibrate = bool.Parse(vibrate_s);
            if (vibrate)
            {
                MMVibrationManager.Vibrate();

            }
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            if (fullLines > 2)
            {

                lineFullNum = fullLines;
                Event.ShowComboWriting();
                
                //Debug.Log("Combo");
            }
            audioManager.PlayComboSFX(audioManager.comboSFX, fullLines);

        }
        

        // Add Scores
        var totalScores = 0;
        switch (fullLines)
        {
            case 1:
                totalScores = 100;
                break;
            case 2:
                totalScores = 200;
                break;
            case 3:
                totalScores = 400;
                break;
            case 4:
                totalScores = 1000;
                break;
            case 5:
                totalScores = 2000;
                break;
            case 6:
                totalScores = 5000;
                break;
            case 7:
                totalScores = 10000;
                break;
            case 8:
                totalScores = 99999;
                break;
        }


        Event.AddScores(totalScores);
        PlayerPrefs.SetInt("CurrentScore", Event._currentScores);
        PlayerPrefs.Save(); // Save PlayerPrefs to disk
        //if (Event._currentScores >= 2000 * multiplier)
        //{
        //    Event.clickCount++;
        //    multiplier++;
        //}
        CheckGameOver();
        PlayerPrefsHelper.SaveEmptyList("myNumbers");
        
    }

    public void UpdateHintLinesWhenDragging()
    {
        var squareIndexes = new List<int>();

        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();

            if (gridSquare.Selected && !gridSquare.SquareOccupied)
            {
                squareIndexes.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
            }
        }

        if (shapeStorer == null)
        {
            Debug.LogError("ShapeStorer is not assigned");
            return;
        }

        var currentSelectedShape = shapeStorer.GetCurrentSelectedShape();
        if (currentSelectedShape == null) return; // No selected shape

        if (currentSelectedShape.totalSquareNumber == squareIndexes.Count) // Check ghost and shape have same number of squares
        {
            compareGhostAndSquare = true;
            //UpdateHintLines();
        }
        else
        {
            compareGhostAndSquare = false;

        }


        UpdateHintLines();
    }

    void UpdateHintLines()
    {
        // Ensure _lineIndicate is properly assigned
        if (_lineIndicate == null)
        {
            Debug.LogError("LineIndicate is not assigned or found");
            return;
        }

        List<int[]> newHintLines = new List<int[]>();
        List<int[]> oldHintLines = new List<int[]>(previousHintLines);

        // Get the new hint lines
        List<int[]> lines = new List<int[]>();

        // columns
        foreach (var column in _lineIndicate.columnIndexes)
        {
            lines.Add(_lineIndicate.GetVerticalLine(column));
        }

        // rows
        for (var row = 0; row < 10; row++)
        {
            List<int> data = new List<int>(10);
            for (var index = 0; index < 10; index++)
            {
                data.Add(_lineIndicate.line_data[row, index]);
            }

            lines.Add(data.ToArray());
        }

        foreach (var line in lines)
        {
            bool isLineFull = true;
            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                if (!comp.SquareOccupied && !comp.CheckUsed())
                {
                    isLineFull = false;
                    break;
                }
            }

            if (isLineFull)
            {
                newHintLines.Add(line);
            }
        }

        // Clear hint lines if no new hint lines found
        if (newHintLines.Count == 0)
        {
            ClearHintLines();
        }

        // Check if the hint lines have changed
        if (!AreHintLinesEqual(newHintLines, oldHintLines))
        {
            // Clear previous hint lines
            ClearHintLines();

            // Show new hint lines
            foreach (var line in newHintLines)
            {
              
                foreach (var squareIndex in line)
                {
                    var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                    
                    comp.ShowHint();
                }
            }

            // Update the previously shown hint lines
            previousHintLines = new List<int[]>(newHintLines);
        }
    }

    // Compares two lists of hint lines to determine if they are the same
    bool AreHintLinesEqual(List<int[]> lines1, List<int[]> lines2)
    {
        if (lines1.Count != lines2.Count)
            return false;

        foreach (var line in lines1)
        {
            bool found = false;
            foreach (var compareLine in lines2)
            {
                if (AreLinesEqual(line, compareLine))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                return false;
        }

        return true;
    }

    // Compares two lines to see if they are equal
    bool AreLinesEqual(int[] line1, int[] line2)
    {
        if (line1.Length != line2.Length)
            return false;

        for (int i = 0; i < line1.Length; i++)
        {
            if (line1[i] != line2[i])
                return false;
        }

        return true;
    }

    // Clears the hint lines
    void ClearHintLines()
    {
        foreach (var square in _gridSquares)
        {
            var comp = square.GetComponent<GridSquare>();
            comp.HideHint();
        }
    }

    // Variable to keep track of previously shown hint lines
    private List<int[]> previousHintLines = new List<int[]>();
    bool isFull;

    private int CheckFullSquares(List<int[]> data)
    {
        List<int[]> fullLines = new List<int[]>();

        var linesFull = 0;

        foreach (var line in data)
        {
            bool isLineFull = true;
            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                if (comp.SquareOccupied == false)
                {
                    isLineFull = false;
                }
            }

            if (isLineFull)
            {
                fullLines.Add(line);
            }
        }

        foreach (var line in fullLines)
        {
            isFull = false;

            //foreach (var squareIndex in line)
            //{
            //    var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
            //    comp.Deactivate();
            //    isFull = true;
            //}

            StartCoroutine(ClearLine(line));
            Debug.Log("isFull" + isFull);
            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.ClearOccupied();
            }

            if (isFull)
            {
                linesFull++;
            }
        }

        return linesFull;
    }

    //float delay = 0.03f;
    private float delay = 0.04f;
    IEnumerator ClearLine(int[] line)
    {
        foreach (var squareIndex in line)
        {
            var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();

            //_animator.SetTrigger("isActive");

            comp.Deactivate();
            isFull = true;
            yield return new WaitForSeconds(delay);
        }

    }

    private void CheckGameOver()
    {
        var shapeHolder = shapeStorer.shapeList[3];
        var isShapeActive = shapeHolder.CheckAnyActive();
        int validShapes = CheckValidShapes();

        if (validShapes == 0 && ((!CheckCanPlacedMoreOnGrid(ref shapeHolder, isShapeActive) && isShapeActive)))
        {
            // GAME OVER
            Event.GameOver(false);
            //List<int> holder_index = new List<int>() { 3};
            //PlayerPrefsHelper.SetEmptyList("InactiveShapeIndexes");
            PlayerPrefsHelper.SetIntList("InactiveShapeIndexes", new List<int>(){ 3});
            //Debug.Log("GAME OVER");
        }
    }

    private bool CheckGameOverWhenRequestNewShapes()
    {
        int validShapes = CheckValidShapes();

        if (validShapes == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DisableShapes()
    {
        for (int index = 0; index < shapeStorer.shapeList.Count; index++)
        {
            var isShapeActive = shapeStorer.shapeList[index].CheckAnyActive();

            Shape currentShape = shapeStorer.shapeList[index]; // Lưu giá trị vào biến tạm thời

            if (CheckCanPlacedMoreOnGrid(ref currentShape, isShapeActive) && isShapeActive)
            {

                //shapeStorer.shapeList[index]?.ActivateShape();
                shapeStorer.shapeList[index].EnableShape();
            }
            else if (!CheckCanPlacedMoreOnGrid(ref currentShape, isShapeActive))
            {
                shapeStorer.shapeList[index].DisableShape();

            }
        }
    }
    private int CheckValidShapes()
    {
        var validShapes = 0;

        for (int index = 0; index < shapeStorer.shapeList.Count; index++)
        {
            var isShapeActive = shapeStorer.shapeList[index].CheckAnyActive();

            Shape currentShape = shapeStorer.shapeList[index]; // Lưu giá trị vào biến tạm thời

            if (CheckCanPlacedMoreOnGrid(ref currentShape, isShapeActive) && isShapeActive)
            {
                
                //shapeStorer.shapeList[index]?.ActivateShape();
                shapeStorer.shapeList[index]?.EnableShape();
                validShapes++;

            }
            else if (!CheckCanPlacedMoreOnGrid(ref currentShape, isShapeActive))
            {
                shapeStorer.shapeList[index]?.DisableShape();
                
            }
        }

        return validShapes;
    }

    public bool CheckCanPlacedMoreOnGrid(ref Shape currentShape, bool isShapeActive)
    {
        var currentShapeData = currentShape.CurrentShapeData;
        var shapeColumns = currentShapeData.columns;
        var shapeRows = currentShapeData.rows;

        // Check for placement at each rotation (0, 90, 180, 270 degrees)
        for (int rotation = 0; rotation < 4; rotation++)
        {
            if (CanPlaceShapeAtRotation(ref currentShapeData, isShapeActive))
            {
                return true;
            }

            if (Event.rotateOut)
            {
                break;
            }
            //Rotate90Clockwise2(ref currentShapeData);
            if (shapeColumns != shapeRows)
            {
                Rotate90Clockwise2(ref currentShapeData);
            }
            else
            {
                Rotate90Clockwise(ref currentShapeData); // Rotate the shape data 90 degrees clockwise
            }
        }

        return false;
    }

    private bool CanPlaceShapeAtRotation(ref ShapeData shapeData, bool isShapeActive)
    {
        var shapeColumns = shapeData.columns;
        var shapeRows = shapeData.rows;

        var originalShapeFilledUpSquares = new List<int>();
        var squareIndex = 0;

        LoopToCheck(shapeRows, shapeColumns, ref shapeData, ref originalShapeFilledUpSquares, ref squareIndex);

        return Check(shapeColumns, shapeRows, originalShapeFilledUpSquares, isShapeActive);
    }

    private void Rotate90Clockwise(ref ShapeData shapeData)
    {
        int N = shapeData.rows;
        for (int i = 0; i < N / 2; i++)
        {
            for (int j = i; j < N - i - 1; j++)
            {
                // Swap elements of each cycle in clockwise direction
                bool temp = shapeData.board[i].column[j];
                shapeData.board[i].column[j] = shapeData.board[N - 1 - j].column[i];
                shapeData.board[N - 1 - j].column[i] = shapeData.board[N - 1 - i].column[N - 1 - j];
                shapeData.board[N - 1 - i].column[N - 1 - j] = shapeData.board[j].column[N - 1 - i];
                shapeData.board[j].column[N - 1 - i] = temp;
            }
        }
    }

    private void Rotate90Clockwise2(ref ShapeData shapeData)
    {
        int m = shapeData.rows;
        int n = shapeData.columns;

        // Tạo ma trận mới với kích thước nxm
        ShapeData.Row[] rotatedBoard = new ShapeData.Row[n];
        for (int i = 0; i < n; i++)
        {
            rotatedBoard[i] = new ShapeData.Row(m);
        }

        // Thực hiện xoay ma trận
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                rotatedBoard[j].column[m - 1 - i] = shapeData.board[i].column[j];
            }
        }

        // Cập nhật ShapeData với ma trận mới
        shapeData.board = rotatedBoard;
        shapeData.rows = n;
        shapeData.columns = m;
    }


    private void LoopToCheck(int shapeRows, int shapeColumns, ref ShapeData currentShapeData, ref List<int> originalShapeFilledUpSquares, ref int squareIndex)
    {
        for (var rowIndex = 0; rowIndex < shapeRows; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < shapeColumns; columnIndex++)
            {
                if (currentShapeData.board[rowIndex].column[columnIndex])
                {
                    originalShapeFilledUpSquares.Add(squareIndex);
                }

                squareIndex++;
            }
        }
    }

    private bool Check(int shapeColumns, int shapeRows, List<int> originalShapeFilledUpSquares, bool isShapeActive)
    {
        int countCanPlaced = 0;

        var squareList = GetAllSquaresCombination(shapeColumns, shapeRows);
        List<int> tipPositions = new List<int>();
        bool canBePlaced = false;

        foreach (var number in squareList)
        {
            List<int> tempTipPos = new List<int>();
            bool shapeCanBePlaceOnBoard = true;
            foreach (var squareIndexToCheck in originalShapeFilledUpSquares)
            {
                var comp = _gridSquares[number[squareIndexToCheck]].GetComponent<GridSquare>();
                if (comp.SquareOccupied)
                {
                    shapeCanBePlaceOnBoard = false;
                }
                tempTipPos.Add(number[squareIndexToCheck]);
            }
            if (shapeCanBePlaceOnBoard)
            {
                countCanPlaced++;
                if (countCanPlaced == 1)
                {
                    tipPositions = tempTipPos;
                }
                canBePlaced = true;
            }
            
        }

        

        if (countCanPlaced == 1 && isShapeActive)
        {

            foreach (var squareIndex in tipPositions)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();

                comp.ShowTip();
            }
            
        }
        
        
        return canBePlaced;
    }

    void ClearTip()
    {
        foreach (var square in _gridSquares)
        {
            var comp = square.GetComponent<GridSquare>();
            comp.HideTip();
        }
    }

    private List<int[]> GetAllSquaresCombination(int cols, int ros)
    {
        var squareList = new List<int[]>();
        var lastColumnIndex = 0;
        var lastRowIndex = 0;

        int safeIndex = 0;

        while (lastRowIndex + (ros - 1) < 10)
        {
            var rowData = new List<int>();

            for (var row = lastRowIndex; row < lastRowIndex + ros; row++)
            {
                for (var column = lastColumnIndex; column < lastColumnIndex + cols; column++)
                {
                    rowData.Add(_lineIndicate.line_data[row, column]);
                }
            }

            squareList.Add(rowData.ToArray());

            lastColumnIndex++;

            if (lastColumnIndex + (cols - 1) >= 10)
            {
                lastColumnIndex = 0;
                lastRowIndex++;
            }

            safeIndex++;
            if (safeIndex > 100) // make sure to end while loop
            {
                break;
            }
        }

        return squareList;
    }
}