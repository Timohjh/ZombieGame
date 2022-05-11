using BNG;
using UnityEngine.Audio;
using UnityEngine.UI;

public interface IPPrefsManager
{
    public void SaveVolume(float volume);

    public void SaveLocomotion(PlayerRotation rotationType);
    public void SaveQuality();

    public void SaveHighScore();

    public void LoadValues(AudioMixer audioMixer, Toggle snapTurn, Text qualityText, PlayerRotation rotationType);
}

