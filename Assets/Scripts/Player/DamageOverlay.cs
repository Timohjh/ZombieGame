using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageOverlay : MonoBehaviour
{
    [SerializeField] float fadeSpeed = 0.0015f;
    [SerializeField] float vignetteMax = 0.5f;
    [SerializeField] Volume volume;

    Vignette _vignette;
    CanvasGroup _canvas;

    PlayerPVE _playerHealth;
    bool _isEnabled = false;
    float _vLerp;

    void Awake()
    {
        _canvas = GetComponent<CanvasGroup>();
        volume.profile.TryGet(out _vignette);
        _playerHealth = GameObject.FindGameObjectWithTag("XRRig").GetComponent<PlayerPVE>();
    }

    void Update()
    {
        if (_isEnabled)
        {
            _canvas.alpha -= fadeSpeed;
            if (_canvas.alpha <= 0f)
                _isEnabled = false;

            _vignette.intensity.value -= fadeSpeed * 0.5f;
        }
    }

    [ContextMenu("Enable")]
    public void EnableOverlay()
    {
        _canvas.alpha = 0.9f;
        _isEnabled = true;
        vignetteMax += 0.05f;
        _vignette.intensity.value = vignetteMax;
    }
}
