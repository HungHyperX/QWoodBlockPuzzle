using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source -----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----------- Audio Clip ------------")]
    public AudioClip BGM;
    public AudioClip gameOverMusic;
    public AudioClip blockPutDown;
    public AudioClip blockDrop;
    public AudioClip blockRotate;
    public AudioClip blockWithdraw;
    public AudioClip buttonClicked;
    public AudioClip magnetDown;

    public List<AudioClip> comboSFX;

    private void Awake()
    {
        if (Application.isEditor == false)
        {
            Debug.unityLogger.logEnabled = false;
        }
    }

    public void Start()
    {
        bool _bgm = true; // Giá trị mặc định nếu không tìm thấy
        bool _sfx = true; // Giá trị mặc định nếu không tìm thấy

        if (PlayerPrefs.HasKey("BGM_ON_OFF"))
        {
            string bgm_s = PlayerPrefs.GetString("BGM_ON_OFF");
            if (!bool.TryParse(bgm_s, out _bgm))
            {
                Debug.LogWarning($"Invalid BGM_ON_OFF value: {bgm_s}. Defaulting to true.");
            }
        }

        if (PlayerPrefs.HasKey("SFX_ON_OFF"))
        {
            string sfx_s = PlayerPrefs.GetString("SFX_ON_OFF");
            if (!bool.TryParse(sfx_s, out _sfx))
            {
                Debug.LogWarning($"Invalid SFX_ON_OFF value: {sfx_s}. Defaulting to true.");
            }
        }

        TurnOnOffSFX(_sfx);
        TurnOnOffBGM(_bgm);
        musicSource.clip = BGM;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayComboSFX(List<AudioClip> comboSFX, int comboNum)
    {
        SFXSource.PlayOneShot(comboSFX[comboNum - 1]);
    }

    public void TurnOnOffBGM(bool flag)
    {
        musicSource.mute = !flag;
    }

    public void TurnOnOffSFX(bool flag)
    {
        SFXSource.mute = !flag;
    }
}
