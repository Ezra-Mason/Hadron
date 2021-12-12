using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCameraShake : MonoBehaviour
{
    public float duration = 0.3f;
    public float amplitude = 1.3f;
    public float timeElapsed = 0f;

    public Cinemachine.CinemachineVirtualCamera vcam;
    public Cinemachine.CinemachineBasicMultiChannelPerlin noiseSettings;
    public bool isShaking = false;

    // Start is called before the first frame update
    void Start()
    {
        //Get the cinemachine camera and its noise settings
        vcam = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        if (vcam != null)
            noiseSettings = vcam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vcam != null || noiseSettings != null)
        {
            //shake the camera and decrease the timer
            if (timeElapsed > 0)
            {
                noiseSettings.m_AmplitudeGain = amplitude;
                timeElapsed -= Time.deltaTime;
            }
            else
            {
                noiseSettings.m_AmplitudeGain = 0f;
                timeElapsed = 0f;
            }

            //continuously shake while bool is true
            if (isShaking)
                noiseSettings.m_AmplitudeGain = amplitude;

        }
    }

    //start shaking method
    public void Shake()
    {
        timeElapsed = duration;
    }

    //start continuous shake method
    public void StartShake()
    {
        noiseSettings.m_AmplitudeGain = amplitude;
        isShaking = true;
    }

    //method for stopping an ongoing shake
    public void StopShake()
    {
        noiseSettings.m_AmplitudeGain = 0f;
        isShaking = false;
    }
}
