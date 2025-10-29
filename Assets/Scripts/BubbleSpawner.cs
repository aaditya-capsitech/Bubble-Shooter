using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public static BubbleSpawner instance;
    private int currentRowCount = 0;

    [Header("Shooting")]
    public GameObject[] shootingPrefabs;
    public Transform shootingPoint;
    public float bubbleSpeed = 20f;

    [Header("Bubble Grid")]
    public Transform bubbleGroup; 
    public GameObject[] bubblePrefabs;
    public int columns = 7;
    public float bubbleSpacing = 0.5f;
    public float pushDownDistance = 0.5f;

    [Header("Miss Counter")]
    private int missedShots = 0;
    public int maxMissBeforePushDown = 3;

    //[Header("Shooting Angle Limit")]
    //[Range(0, 90)] public float maxShootingAngle = 70f;
    //public float arcRadius = 2f;

    private GameObject currentBubble;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnNextBubble();
    }

    public void HideAllBubbles()
    {
        if (bubbleGroup != null)
        {
            foreach (Transform child in bubbleGroup)
            {
                if (child.CompareTag("Bubble"))
                {
                    Destroy(child.gameObject);
                }
            }
        }

        Bubble[] allBubbles = FindObjectsOfType<Bubble>();
        foreach (Bubble b in allBubbles)
        {
            if (b != null)
                Destroy(b.gameObject);
        }

        // Destroy the current shooting bubble if still alive
        if (currentBubble != null)
        {
            Destroy(currentBubble);
            currentBubble = null;
        }
        if (bubbleGroup != null)
            bubbleGroup.gameObject.SetActive(false);

        Debug.Log("All bubbles removed for Game Over ");
    }
    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
            ShootCurrentBubble();
        }
    }
    void SpawnNextBubble()
    {
        if (currentBubble != null) return;

        int index = Random.Range(0, shootingPrefabs.Length);
        currentBubble = Instantiate(shootingPrefabs[index], shootingPoint.position, Quaternion.identity);

        Rigidbody2D rb = currentBubble.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.freezeRotation = true;
        }

        Bubble bubble = currentBubble.GetComponent<Bubble>();
        PlayerBubble pb = currentBubble.GetComponent<PlayerBubble>();
        if (bubble != null && pb != null)
        {
            pb.type = bubble.type;
        }
    }
    void ShootCurrentBubble()
    {
        if (currentBubble == null) return;

        Rigidbody2D rb = currentBubble.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 direction = (mousePos - shootingPoint.position).normalized;

        // prevent shooting downward
        if (direction.y < 0) return;
        //ARCH
        float angleUp = Vector2.Angle(Vector2.up, direction);
        if (angleUp > 60f)
        {
            Debug.Log("Shot angle too wide ignored");
            return;
        }

        rb.linearVelocity = direction * bubbleSpeed;
        currentBubble = null;
        Invoke(nameof(SpawnNextBubble), 0.3f);
    }
    // when a shot doesn’t result in a match
    public void RegisterMissShot()
    {
        missedShots++;
        Debug.Log("Missed shots: " + missedShots);

        if (missedShots >= maxMissBeforePushDown)
        {
            missedShots = 0;
            PushBubblesDown();
            SpawnTopRow();
        }
    }

    void PushBubblesDown()
    {
        if (bubbleGroup == null) return;

        bubbleGroup.position += Vector3.down * pushDownDistance;
        Debug.Log("Pushed all bubbles down!");
        //Game Over check 
        Bubble[] allBubbles = FindObjectsOfType<Bubble>();
        PlayerBubble[] shootingBubbles = FindObjectsOfType<PlayerBubble>();

        foreach (Bubble b in allBubbles)
        {
            if (b != null && b.transform.position.y <= GameOverZone.yPosition)
            {
                if (UIManager.instance != null)
                    UIManager.instance.ShowGameOver();
                return;
            }
        }
        foreach (PlayerBubble pb in shootingBubbles)
        {
            if (pb != null && pb.transform.position.y <= GameOverZone.yPosition)
            {
                if (UIManager.instance != null)
                    UIManager.instance.ShowGameOver();
                return;
            }
        }
    }

    void SpawnTopRow()
    {
        if (bubblePrefabs.Length == 0 || bubbleGroup == null) return;

        Vector2 startPosition = new Vector2(-4.5f, 3.9f);
        float topY = float.MinValue;
        foreach (Transform child in bubbleGroup)
        {
            if (child.CompareTag("Bubble") && child.position.y > topY)
                topY = child.position.y;
        }
        float newRowY = topY + bubbleSpacing;
        bool isOffsetRow = (currentRowCount % 2 == 1);

        for (int col = 0; col < columns; col++)
        {
            int index = Random.Range(0, bubblePrefabs.Length);

            float x = startPosition.x + (col * bubbleSpacing);
            if (isOffsetRow)
                x += bubbleSpacing / 2f;

            Vector3 pos = new Vector3(x, newRowY, 0);
            Instantiate(bubblePrefabs[index], pos, Quaternion.identity, bubbleGroup);
        }
        currentRowCount++;
        Debug.Log("Spawned new top row with same pattern as initial grid!");
    }
    private void OnDrawGizmos()
    {
        if (shootingPoint == null)
            return;
        Gizmos.color = Color.green;
        float previewLength = 4f;
        float maxAngle = 60f; 

        Vector3 leftDir = Quaternion.Euler(0, 0, 90 + maxAngle) * Vector3.right;
        Gizmos.DrawLine(shootingPoint.position, shootingPoint.position + leftDir * previewLength);

        Vector3 rightDir = Quaternion.Euler(0, 0, 90 - maxAngle) * Vector3.right;
        Gizmos.DrawLine(shootingPoint.position, shootingPoint.position + rightDir * previewLength);
    }
}
