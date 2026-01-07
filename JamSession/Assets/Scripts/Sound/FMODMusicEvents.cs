using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class FMODMusicEvents : MonoBehaviour
{
    public EventReference musicEvent;
    private EventInstance musicInstance;

    // The object to animate
    public Transform objectToAnimate;

    // Animation parameters
    public float beatScaleAmount = 1.5f;
    public float scaleSpeed = 5f;

    // Internal
    private bool beatTriggered = false;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = objectToAnimate.localScale;

        // Create FMOD instance
        musicInstance = RuntimeManager.CreateInstance(musicEvent);

        // Listen for beat callbacks
        musicInstance.setCallback(
            (type, instance, paramPtr) =>
            {
                if (type == EVENT_CALLBACK_TYPE.TIMELINE_BEAT)
                {
                    Beat();
                }
                return RESULT.OK;
            },
            EVENT_CALLBACK_TYPE.TIMELINE_BEAT
        );

        musicInstance.start();
    }

    void Update()
    {
        // Animate scale smoothly toward target
        Vector3 targetScale = beatTriggered ? originalScale * beatScaleAmount : originalScale;
        objectToAnimate.localScale = Vector3.Lerp(objectToAnimate.localScale, targetScale, Time.deltaTime * scaleSpeed);

        // Reset beat trigger after scaling
        if (beatTriggered && Vector3.Distance(objectToAnimate.localScale, targetScale) < 0.01f)
            beatTriggered = false;
    }

    private void Beat()
    {
        beatTriggered = true;
    }

    void OnDestroy()
    {
        musicInstance.stop(STOP_MODE.IMMEDIATE);
        musicInstance.release();
    }
}