using UnityEngine;
using System.Collections.Generic;

public static class PlayerPrefsHelper
{
    public static void SetEmptyList(string key)
    {
        // Chuyển đổi List<int> thành chuỗi
        List<int> list = new List<int>();
        string listString = string.Join(",", list);
        PlayerPrefs.SetString(key, listString);
    }

    public static void SetIntList(string key, List<int> list)
    {
        // Chuyển đổi List<int> thành chuỗi
        string listString = string.Join(",", list);
        PlayerPrefs.SetString(key, listString);
    }

    public static void SaveIntList(string key, List<int> list)
    {
        // Chuyển đổi List<int> thành chuỗi
        string listString = string.Join(",", list);
        PlayerPrefs.SetString(key, listString);
        PlayerPrefs.SetInt("RotateCount", Event.clickCount);
        PlayerPrefs.SetInt("MagnetAble", Event.MagnetAble);
        PlayerPrefs.SetInt("CurrentScore", Event._currentScores);
        Event.emptyGrids = false;
        Event.rotatable = false;
        //Event.rotateSwitch = false;
    }
    public static void SaveEmptyList(string key)
    {
        // Chuyển đổi List<int> thành chuỗi
        List<int> list = new List<int>();
        string listString = string.Join(",", list);
        PlayerPrefs.SetString(key, listString);
        PlayerPrefs.SetInt("MagnetAble", 1);
        PlayerPrefs.SetInt("RotateCount", 4);
        PlayerPrefs.SetInt("CurrentScore", 0);
        Event.emptyGrids = true;
        Event.rotatable = false;
        Event.rotateSwitch = false;
        //SetEmptyList("InactiveShapeIndexes");
        PlayerPrefsHelper.SetIntList("InactiveShapeIndexes", new List<int>() { 3 });
    }

    public static List<int> LoadIntList(string key)
    {
        // Kiểm tra xem có dữ liệu được lưu với key này hay không
        if (PlayerPrefs.HasKey(key))
        {
            // Lấy chuỗi đã lưu và chuyển đổi nó trở lại thành List<int>
            string listString = PlayerPrefs.GetString(key);
            string[] stringArray = listString.Split(',');

            List<int> intList = new List<int>();
            foreach (string s in stringArray)
            {
                if (int.TryParse(s, out int result))
                {
                    intList.Add(result);
                }
            }
            Event.MagnetAble = PlayerPrefs.GetInt("MagnetAble");
            Event.clickCount = PlayerPrefs.GetInt("RotateCount");
            Event._currentScores = PlayerPrefs.GetInt("CurrentScore");
            return intList;
        }
        else
        {
            // Trả về danh sách trống nếu không có dữ liệu được lưu
            return new List<int>();
        }
    }

    
}
