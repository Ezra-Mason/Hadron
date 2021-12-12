using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
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
        //pass this to the game manager to call from any script
        GameManager.instance.shaker = this;
        //get the cinemachine camera and its noise settings for the shake
        vcam = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        if (vcam != null)
            noiseSettings = vcam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vcam!=null || noiseSettings!=null)
        {
            //apply shake while timer is going and decreasing timer
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

            //continuous shake mode while bool is true
            if (isShaking)
                noiseSettings.m_AmplitudeGain = amplitude;
            
        }
    }

    //method for timed screen shake
    public void Shake()
    {
        timeElapsed = duration;
    }

    //starting the continuous shake
    public void StartShake()
    {
        noiseSettings.m_AmplitudeGain = amplitude;
        isShaking = true;
    }

    //stop the continuous shake
    public void StopShake()
    {
        noiseSettings.m_AmplitudeGain = 0f;
        isShaking = false;
    }
}
