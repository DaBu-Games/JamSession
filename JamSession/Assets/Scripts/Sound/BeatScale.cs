using UnityEngine;

public class BeatScale : MonoBehaviour
{
    [SerializeField] private float maxScale = 1.5f;
    [SerializeField] private float scaleSpeed = 8f;
    [SerializeField] private bool moveToBeat = true;

    private Vector3 startScale;
    private Vector3 targetScale;
    private float timer = 0f;
    
    public void SetMoveToBeat() => moveToBeat = false;

    void Start()
    {
        startScale = transform.localScale;
        targetScale = startScale;
    }

    void Update()
    {
        if (moveToBeat)
        {
            if (FMODMusicEvents.inBeatWindow)
            {
                bool isEvenBeat = FMODMusicEvents.currentBeat % 2 == 0;
                targetScale = isEvenBeat ? startScale * maxScale : startScale;
            }
            else
            {
                targetScale = startScale;
            }
        }
        else
        {
            timer += Time.deltaTime;
            
            float scaleFactor = 1f + Mathf.Sin(timer * scaleSpeed) * (maxScale - 1f);
            targetScale = startScale * scaleFactor;
        }

        // Smoothly interpolate scale
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * scaleSpeed
        );
    }
}