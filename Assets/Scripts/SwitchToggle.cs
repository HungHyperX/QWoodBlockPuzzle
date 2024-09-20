using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using MoreMountains.NiceVibrations;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField]
    private RectTransform uiHandleRectTransform;
    [SerializeField]
    private Image iconImage;
    //[SerializeField]
    //private Color backgroundActiveColor;
    [Header("---------- ToggleBackground -----------")]
    [SerializeField]
    private Sprite backgroundActiveImage;
    [SerializeField]
    private Sprite backgroundInactiveImage;
    [Header("---------- Icon -----------")]
    [SerializeField]
    private Sprite iconActiveImage;
    [SerializeField]
    private Sprite iconInactiveImage;
    [Header("---------- Audio Manager -----------")]
    [SerializeField]
    private AudioManager audioManager;

    //private Color backgroundDefaultColor;
    private Image backgroundImage;


    private Toggle toggle;

    private Vector2 handlePosition;

    public bool _bgm { get; set; }
    private bool _sfx;
    private bool _vibrate;

    //private bool _vibrate;

    private const string SAVE_SEPARATOR = "#SAVE-VALUE#";

    private void Awake()
    {
        toggle = GetComponent<Toggle>();

        handlePosition = uiHandleRectTransform.anchoredPosition;
        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        //iconImage = transform.FindChild("MusicIcon").GetComponent<Image>();

        //backgroundDefaultColor = backgroundImage.color;

        toggle.onValueChanged.AddListener(OnSwitch);
        if (!PlayerPrefs.HasKey("BGM_ON_OFF"))
        {
            SaveSettingsData(true, true, true);
        }
        LoadSettingsData();

        if (gameObject.CompareTag("BGM"))
        {
            toggle.isOn = _bgm;
        }
        else if (gameObject.CompareTag("SFX"))
        {
            toggle.isOn = _sfx;
        }
        else if (gameObject.CompareTag("Vibrate"))
        {
            toggle.isOn = _vibrate;
        }

        if (toggle.isOn)
        {
            OnSwitch(true);
        }
    }

    private void OnSwitch(bool on)
    {
        //Debug.Log(on);
        if (gameObject.CompareTag("BGM"))
        {
            _bgm = on;
            audioManager.TurnOnOffBGM(on);

        }
        else if (gameObject.CompareTag("SFX"))
        {
            _sfx = on;
            audioManager.TurnOnOffSFX(on);
        }
        else if (gameObject.CompareTag("Vibrate"))
        {
            _vibrate = on;
            MMVibrationManager.SetHapticsActive(on);
        }
        SaveSettingsData(_bgm, _sfx, _vibrate);
        //if (on)
        //{
        //    uiHandleRectTransform.anchoredPosition = handlePosition;
        //}
        //else
        //{
        //    uiHandleRectTransform.anchoredPosition = handlePosition * (-1);
        //}
        audioManager.PlaySFX(audioManager.buttonClicked);
        uiHandleRectTransform.DOAnchorPos(on ? handlePosition : -handlePosition, .4f).SetEase(Ease.InOutBack);
        //backgroundImage.color = on? backgroundActiveColor : backgroundDefaultColor;
        backgroundImage.sprite = on ? backgroundActiveImage : backgroundInactiveImage;
        iconImage.sprite = on ? iconActiveImage : iconInactiveImage;

    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }

    //Save and load data from text file
    //public void SaveSettingsData(bool _bgm, bool _sfx, bool _vibrate)
    //{

    //    string[] contents = new string[]
    //    {
    //        "" + _bgm,
    //        "" + _sfx,
    //        "" + _vibrate
    //    };
    //    string saveString = string.Join(SAVE_SEPARATOR, contents);
    //    File.WriteAllText(Application.dataPath + "/save.txt", saveString);

    //}

    //public void LoadSettingsData()
    //{
    //    string saveString = File.ReadAllText(Application.dataPath + "/save.txt");

    //    string[] contents = saveString.Split(new[] { SAVE_SEPARATOR }, System.StringSplitOptions.None);
    //    _bgm = bool.Parse(contents[0]);
    //    _sfx = bool.Parse(contents[1]);
    //    _vibrate = bool.Parse(contents[2]);
    //}

    // Save and load data from JSON file

    //private class SettingsData
    //public class SettingsData
    //{
    //    public bool bgm;
    //    public bool sfx;
    //    public bool vibrate;

    //    //public SettingsData(bool bgm, bool sfx, bool vibrate)
    //    //{
    //    //    this.bgm = bgm;
    //    //    this.sfx = sfx;
    //    //    this.vibrate = vibrate;
    //    //}
    //}

    //public void SaveSettingsData(bool _bgm, bool _sfx, bool _vibrate)
    //{
    //    //SettingsData data = new SettingsData(_bgm, _sfx, _vibrate);

    //    SettingsData settingsData = new SettingsData
    //    {
    //        bgm = _bgm,
    //        sfx = _sfx,
    //        vibrate = _vibrate
    //    };
    //    string json = JsonUtility.ToJson(settingsData);

    //    File.WriteAllText(Application.dataPath + "/save.json", json);

    //}

    //public void LoadSettingsData()
    //{
    //    string saveString = File.ReadAllText(Application.dataPath + "/save.json");

    //    SettingsData settingsData = JsonUtility.FromJson<SettingsData>(saveString);

    //    _bgm = settingsData.bgm;
    //    _sfx = settingsData.sfx;
    //    _vibrate = settingsData.vibrate;

    //}

    // Save and load data with PlayerPrefs
    public void SaveSettingsData(bool _bgm, bool _sfx, bool _vibrate)
    {
        string bgm_s = "" + _bgm;
        string sfx_s = "" + _sfx;
        string vibrate_s = "" + _vibrate;
        PlayerPrefs.SetString("BGM_ON_OFF", bgm_s);
        PlayerPrefs.SetString("SFX_ON_OFF", sfx_s);
        PlayerPrefs.SetString("VIBRATE_ON_OFF", vibrate_s);
        PlayerPrefs.Save();

    }

    public void LoadSettingsData()
    {
        if (PlayerPrefs.HasKey("BGM_ON_OFF"))
        {
            string bgm_s = PlayerPrefs.GetString("BGM_ON_OFF");
            string sfx_s = PlayerPrefs.GetString("SFX_ON_OFF");
            string vibrate_s = PlayerPrefs.GetString("VIBRATE_ON_OFF");
            _bgm = bool.Parse(bgm_s);
            _sfx = bool.Parse(sfx_s);
            _vibrate = bool.Parse(vibrate_s);   
        }
        else
        {
            // No save available
            Debug.Log("No Save");
        }
    }
}
