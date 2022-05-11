using BNG;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIActions : MonoBehaviour
{
    #region Config
    [SerializeField] Text qualityText;
    [SerializeField] Text roundText;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] GameObject player;
    [SerializeField] GameObject HandUI;
    [SerializeField] Toggle snapTurn;
    PlayerRotation _rotationType;
    IPPrefsManager _pPrefs;

    void Awake()
    { 
        _rotationType = player.GetComponent<PlayerRotation>();
        _pPrefs = GetComponent<IPPrefsManager>();
    }
    #endregion

    void Start() => _pPrefs.LoadValues(audioMixer, snapTurn, qualityText, _rotationType);

    void Update()
    {
        if (InputBridge.Instance && InputBridge.Instance.XButtonDown)
            HandUI.SetActive(!HandUI.activeSelf);
    }

    #region UI
    public void SetRound()
    {
        roundText.text = "Round: " + GameManager.round.ToString();
    }

    public void QuitTheGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         _pPrefs.SaveHighScore();
         Application.Quit();
#endif
    }

    public void SetQuality()
    {
        if (QualitySettings.GetQualityLevel() == 0)
        {
            QualitySettings.SetQualityLevel(1);
            qualityText.text = "Low";
            Debug.Log("set to low");
        }
        else
        {
            QualitySettings.SetQualityLevel(0);
            qualityText.text = "High";
            Debug.Log("set to high");
        }
        _pPrefs.SaveQuality();
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        if (volume == -60) SetVolume(-80);
        _pPrefs.SaveVolume(volume);
    }
    public void LocomotionChange()
    {
        if (snapTurn.isOn == true)
            _rotationType.RotationType = RotationMechanic.Snap;
        else
            _rotationType.RotationType = RotationMechanic.Smooth;
        _pPrefs.SaveLocomotion(_rotationType);
    }
    #endregion
}

