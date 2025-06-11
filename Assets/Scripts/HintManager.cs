using UnityEngine;

public class HintManager : MonoBehaviour
{
    public GameObject MovementHintPack;
    public GameObject ClickHintPack;

    public float movementHintDelay = 5f;
    public float clickHintDelay = 8f;

    private float movementTimer = 0f;
    private float clickTimer = 0f;

    private Vector2 lastPosition;
    private bool movementHintShown = false;
    private bool clickHintShown = false;
    private bool hintsDisabled = false;

    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        lastPosition = playerTransform.position;

        MovementHintPack.SetActive(false);
        ClickHintPack.SetActive(false);
    }

    void Update()
    {
        // Track movement
        if ((Vector2)playerTransform.position != lastPosition)
        {
            movementTimer = 0f;
            lastPosition = playerTransform.position;

            MovementHintPack.SetActive(false);
            movementHintShown = false;
        }
        else
        {
            movementTimer += Time.deltaTime;
        }

        // Track click
        if (Input.GetMouseButtonDown(0))
        {
            clickTimer = 0f;
            ClickHintPack.SetActive(false);
            clickHintShown = false;
        }
        else
        {
            clickTimer += Time.deltaTime;
        }

        // Show movement hint
        if (movementTimer >= movementHintDelay && !movementHintShown && !hintsDisabled)
        {
            MovementHintPack.SetActive(true);
            movementHintShown = true;
        }

        // Show click hint
        if (clickTimer >= clickHintDelay && !clickHintShown && !hintsDisabled)
        {
            ClickHintPack.SetActive(true);
            clickHintShown = true;
        }

        // Toggle hints manually
        if (Input.GetKeyDown(KeyCode.H))
        {
            bool anyActive = MovementHintPack.activeSelf || ClickHintPack.activeSelf;

            if (anyActive)
            {
                MovementHintPack.SetActive(false);
                ClickHintPack.SetActive(false);
                hintsDisabled = true;
            }
            else
            {
                hintsDisabled = false;
                movementTimer = 0f;
                clickTimer = 0f;
            }
        }
    }
}
