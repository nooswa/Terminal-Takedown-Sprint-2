using UnityEngine;

public class HintManager : MonoBehaviour
{
    public GameObject MovementHintPack; //references
    public GameObject ClickHintPack;

    public float movementHintDelay = 5f; //hint delays for movement and click
    public float clickHintDelay = 8f;

    private float movementTimer = 0f; //for tracking
    private float clickTimer = 0f;

    private Vector2 lastPosition; //track of last pos
    private bool movementHintShown = false; //flags to check shown hints
    private bool clickHintShown = false;
    private bool hintsDisabled = false;

    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        lastPosition = playerTransform.position;

        MovementHintPack.SetActive(false); //hidden initially via start
        ClickHintPack.SetActive(false);
    }

    void Update()
    {
        // Track movement
        if ((Vector2)playerTransform.position != lastPosition) //if player moves
        {
            movementTimer = 0f; //reset timer and hints
            lastPosition = playerTransform.position;

            MovementHintPack.SetActive(false);
            movementHintShown = false;
        }
        else
        {
            movementTimer += Time.deltaTime; //if not then idle timer updates
        }

        // Track click
        if (Input.GetMouseButtonDown(0)) //checks for player clicks
        {
            clickTimer = 0f; //resets timer and hide hint
            ClickHintPack.SetActive(false);
            clickHintShown = false;
        }
        else
        {
            clickTimer += Time.deltaTime;
        }

        // Show movement hint (if player reaches criteria for hint displays
        if (movementTimer >= movementHintDelay && !movementHintShown && !hintsDisabled)
        {
            MovementHintPack.SetActive(true); //sets active
            movementHintShown = true;
        }

        // Show click hint
        if (clickTimer >= clickHintDelay && !clickHintShown && !hintsDisabled) //checks for if hints r still not active after criterias met
        {
            ClickHintPack.SetActive(true); //sets active
            clickHintShown = true;
        }

        // Toggle hints manually
        if (Input.GetKeyDown(KeyCode.H)) //manual hint displays
        {
            bool anyActive = MovementHintPack.activeSelf || ClickHintPack.activeSelf;

            if (anyActive) //if on turns off
            {
                MovementHintPack.SetActive(false);
                ClickHintPack.SetActive(false);
                hintsDisabled = true;
            }
            else //if off turns on and reset timer count
            {
                hintsDisabled = false;
                movementTimer = 0f;
                clickTimer = 0f;
            }
        }
    }
}
