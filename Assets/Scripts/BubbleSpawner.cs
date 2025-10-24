using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject[] shootingPrefabs;
    public Transform shootingPoint;
    public float bubbleSpeed = 20f;

    private GameObject currentBubble;

    void Start()
    {
        SpawnNextBubble();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
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

            SpriteRenderer sr = currentBubble.GetComponent<SpriteRenderer>();
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

        rb.linearVelocity = direction * bubbleSpeed;

        currentBubble = null;
        Invoke(nameof(SpawnNextBubble), 0.3f);
    }
}

