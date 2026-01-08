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

    void Start()
    {
        musicInstance = RuntimeManager.CreateInstance(musicEvent);

        musicInstance.setCallback(
            MusicCallback,
            EVENT_CALLBACK_TYPE.TIMELINE_BEAT | EVENT_CALLBACK_TYPE.TIMELINE_MARKER
        );

        musicInstance.start();
    }

    void OnDestroy()
    {
        musicInstance.stop(STOP_MODE.IMMEDIATE);
        musicInstance.release();
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
                Debug.Log("In");
            }
            else if (marker.name == "Out")
            {
                inBeatWindow = false;
                Debug.Log("Out");
            }
        }

        return RESULT.OK;
    }
}