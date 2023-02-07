using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSwitcher : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _mixer;
    [SerializeField] private Button _soundOff;
    [SerializeField] private Button _soundOn;
    
    private string _masterMixerName = "MasterVolume";
    private float _soundOffValue = -80;

    public void SwitchOffSound()
    {
        Switch(_soundOffValue, false, true);
    }
    
    public void SwitchOnSound()
    {
        Switch(0, true, false);
    }

    private void Switch(float soundOffValue, bool isActiveOffButton, bool IsActiveOnButton)
    {
        _mixer.audioMixer.SetFloat(_masterMixerName, soundOffValue);
        _soundOff.gameObject.SetActive(isActiveOffButton);
        _soundOn.gameObject.SetActive(IsActiveOnButton);
    }
}
