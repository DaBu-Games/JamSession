using System;
using System.Runtime.InteropServices;
using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class FMODMusicEvents : MonoBehaviour
{
    public EventReference musicEvent;
    private EventInstance musicInstance;

    public static int currentBeat;
    public static bool inBeatWindow = true;
    
    private bool stopped = false;

    void Start()
    {
        musicInstance = RuntimeManager.CreateInstance(musicEvent);

        musicInstance.setCallback(
            MusicCallback,
            EVENT_CALLBACK_TYPE.TIMELINE_BEAT 
            | EVENT_CALLBACK_TYPE.TIMELINE_MARKER 
            | EVENT_CALLBACK_TYPE.STOPPED
        );

        musicInstance.start();
    }

    private void OnEnable()
    {
        GameEvents.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.GameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        if (!stopped)
        {
            musicInstance.stop(STOP_MODE.IMMEDIATE);
            musicInstance.release();
            stopped = true;
        }
    }

    private void OnDestroy()
    {
        OnGameOver();
    }

    private void Update()
    {
        if (!stopped)
        {
            PLAYBACK_STATE state;
            musicInstance.getPlaybackState(out state);
            
            if (state == PLAYBACK_STATE.STOPPED)
            {
                GameEvents.GameOver?.Invoke();
            }
        }
    }

    static RESULT MusicCallback(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        if (type == EVENT_CALLBACK_TYPE.TIMELINE_BEAT)
        {
            var beat = (TIMELINE_BEAT_PROPERTIES)
                Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_BEAT_PROPERTIES));

            currentBeat = beat.beat;
        }

        if (type == EVENT_CALLBACK_TYPE.TIMELINE_MARKER)
        {
            var marker = (TIMELINE_MARKER_PROPERTIES)
                Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_MARKER_PROPERTIES));

            if (marker.name == "In")
            {
                inBeatWindow = true;
            }
            else if (marker.name == "Out")
            {
                inBeatWindow = false;
            }
        }

        return RESULT.OK;
    }
}