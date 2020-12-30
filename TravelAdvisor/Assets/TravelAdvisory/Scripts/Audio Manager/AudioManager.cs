using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource mainTheme;
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetVolume() //Called by options menu slider
    {
        mainTheme.volume = slider.value;
    }
}
