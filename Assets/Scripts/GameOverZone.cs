using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    public static float yPosition;
    private bool canTrigger = false;
    private void Start()
    {
        yPosition = transform.position.y;
        Invoke(nameof(EnableTrigger), 1.5f); 
    }
    void EnableTrigger()
    {
        canTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canTrigger) return;
        if (collision.GetComponent<Bubble>() != null && collision.transform.parent != null)
        {
            if (collision.transform.parent.CompareTag("BubbleGroup"))
            {
                Debug.Log("Grid bubble touched GameOver line!");
                UIManager.instance.ShowGameOver();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 left = transform.position + Vector3.left * 5f;
        Vector3 right = transform.position + Vector3.right * 5f;
        Gizmos.DrawLine(left, right);
    }
}