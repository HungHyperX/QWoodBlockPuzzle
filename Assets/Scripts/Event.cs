using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Event : MonoBehaviour
{

    public static Action CheckPlaced; // Action is basically an interface for any method which has no return and no input parameters
    // Delegate is similar to pointer C#
    // Can call it
    public static Action BackToStartPosition;

    public static Action RequestNewShapes;

    public static Action SetShapeInactive;

    public static Action<int> AddScores; // pass argument

    public static Action<bool> GameOver;

    public static Action<int, int> UpdateHighScoreText;

    public static Action ShowComboWriting;

    public static int clickCount = 4; // For rotate button

    public static int MagnetAble = 1; // For magnet button
    

    public static bool rotateSwitch = false;

    public static bool rotatable = false;

    public static bool rotatateRemain = true;

    public static bool rotateOut = false;

    public static bool rotateAni = false; // For Rotation Anim

    public static int _currentScores = 0; // Current Score

    public static bool emptyGrids = false; // For check grids are empty or not
}
