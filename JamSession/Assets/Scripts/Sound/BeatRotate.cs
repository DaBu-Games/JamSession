using UnityEngine;

public class BeatRotate : MonoBehaviour
{
    [SerializeField] private float maxRotation = 40f;
    [SerializeField] private float rotateSpeed = 8f;
    [SerializeField] private bool moveToBeat = true;

    private Quaternion startRotation;
    private Quaternion targetRotation;
    private Vector3 chosenAxis;
    private float timer = 0f;
    
    public void SetMoveToBeat() => moveToBeat = false;

    void Start()
    {
        startRotation = transform.localRotation;
        targetRotation = startRotation;
        
        Vector3[] axes = { Vector3.right, Vector3.up, Vector3.forward };
        chosenAxis = axes[Random.Range(0, axes.Length)];
    }

    void Update()
    {
        if (moveToBeat)
        {
            if (FMODMusicEvents.inBeatWindow)
            {
                bool isEvenBeat = FMODMusicEvents.currentBeat % 2 == 0;
                float direction = isEvenBeat ? -1f : 1f;

                targetRotation = startRotation * Quaternion.AngleAxis(maxRotation * direction, chosenAxis);
            }
            else
            {
                targetRotation = startRotation;
            }
        }
        else
        {
            timer += Time.deltaTime;
            float angle = Mathf.Sin(timer * rotateSpeed) * maxRotation;
            targetRotation = startRotation * Quaternion.AngleAxis(angle, chosenAxis);
        }

        // Smoothly rotate
        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation,
            targetRotation,
            rotateSpeed * Time.deltaTime * 100f
        );
    }
}