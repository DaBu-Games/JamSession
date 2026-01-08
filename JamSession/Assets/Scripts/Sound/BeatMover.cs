using UnityEngine;

public class BeatMover : MonoBehaviour
{
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private bool moveToBeat = true;

    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 chosenAxis;
    private float timer = 0f;
    
    public void SetMoveToBeat() => moveToBeat = false;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos;
        
        Vector3[] axes = { Vector3.right, Vector3.forward };
        chosenAxis = axes[Random.Range(0, axes.Length)];
    }

    void Update()
    {
        if (moveToBeat)
        {
            if (FMODMusicEvents.inBeatWindow)
            {
                bool isEvenBeat = FMODMusicEvents.currentBeat % 2 == 0;

                Vector3 direction = isEvenBeat ? chosenAxis : -chosenAxis;
                targetPos = startPos + direction * moveDistance;
            }
            else
            {
                targetPos = startPos;
            }
        }
        else
        {
            timer += Time.deltaTime;
            float offset = Mathf.Sin(timer * moveSpeed) * moveDistance;
            targetPos = startPos + chosenAxis * offset;
        }

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * moveSpeed
        );
    }
}