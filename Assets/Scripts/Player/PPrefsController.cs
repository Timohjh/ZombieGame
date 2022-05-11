using BNG;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PPrefsController : MonoBehaviour, IPPrefsManager
{
    [SerializeField] Text highScoreText;
    [SerializeField] UnityEngine.UI.Slider slider;
    int _highScore;
    int _rotationInt = 0;

    public void SaveVolume(float volume) => PlayerPrefs.SetFloat("VolumeValue", volume);

    public void SaveLocomotion(PlayerRotation rotationType)
    {
        if (rotationType.RotationType == RotationMechanic.Snap)
            _rotationInt = 1;
        else _rotationInt = 0;
        PlayerPrefs.SetInt("RotationValue", _rotationInt);
    }
    public void SaveQuality()
    {
        PlayerPrefs.SetInt("QualityValue", QualitySettings.GetQualityLevel());
    }

    public void SaveHighScore()
    {
        _highScore = PlayerPrefs.GetInt("HighScore");
        if (_highScore < GameManager.round)
        {
            PlayerPrefs.SetInt("HighScore", GameManager.round);
            highScoreText.text = "Highscore: " + GameManager.round.ToString();
        }
    }

    public void LoadValues(AudioMixer audioMixer, Toggle snapTurn, Text qualityText, PlayerRotation rotationType)
    {
        var volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        audioMixer.SetFloat("volume", volumeValue);
        slider.value = volumeValue;

        var qualityValue = PlayerPrefs.GetInt("QualityValue");
        if (qualityValue == 0)
        {
            QualitySettings.SetQualityLevel(0);
            qualityText.text = "High";
        }
        else
        {
            QualitySettings.SetQualityLevel(1);
            qualityText.text = "Low";
        }

        var rotationValue = PlayerPrefs.GetInt("RotationValue");
        if (rotationValue == 0)
        {
            snapTurn.isOn = false;
            rotationType.RotationType = RotationMechanic.Smooth;
        }
        else
        {
            snapTurn.isOn = true;
            rotationType.RotationType = RotationMechanic.Snap;
        }

        _highScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = "Highscore: " + _highScore;
    }
}
