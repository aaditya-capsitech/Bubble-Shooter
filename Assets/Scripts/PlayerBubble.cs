using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerBubble : MonoBehaviour
{
    public Bubble.BubbleColor type;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bubble hitBubble = collision.gameObject.GetComponent<Bubble>();
        if (hitBubble == null) return;

        if (hitBubble.type == type)
        {
            Debug.Log("Matched color with " + hitBubble.name);

            List<Bubble> connected = FindConnectedSameColor(hitBubble);

            // Only destroy if 3 or more connected bubbles
            if (connected.Count >= 3)
            {
                if (UIManager.instance != null)
                    UIManager.instance.AddScore(connected.Count);

                foreach (Bubble b in connected)
                {
                    if (AudioManager.instance != null)
                        AudioManager.instance.PlayAudio();

                    Color popColor = GetBubbleColor(b.type);
                    if (BubblePop.instance != null)
                        BubblePop.instance.PlayEffect(b.transform.position, popColor);

                    Destroy(b.gameObject);
                }

                if (BubblePop.instance != null)
                    BubblePop.instance.PlayEffect(transform.position, GetBubbleColor(type));

                Destroy(gameObject);
                //helper 

                Color GetBubbleColor(Bubble.BubbleColor type)
                {
                    switch (type)
                    {
                        case Bubble.BubbleColor.Red: return Color.red;
                        case Bubble.BubbleColor.Blue: return Color.blue;
                        case Bubble.BubbleColor.Green: return Color.green;
                        case Bubble.BubbleColor.Yellow: return Color.yellow;
                        default: return Color.white;
                    }
                }

                if (FindObjectsOfType<Bubble>().Length == 0)
                {
                    if (UIManager.instance != null)
                        UIManager.instance.ShowWin();
                }
            }
            else
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }
                transform.SetParent(collision.transform);
                Debug.Log("Less than 3 connected.");
            }
        }
        else
        {

            Debug.Log("Not same color");
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
               // rb.bodyType = RigidbodyType2D.Kinematic;
            }
            transform.SetParent(collision.transform);

            if (BubbleSpawner.instance != null)
                BubbleSpawner.instance.RegisterMissShot();
        }
    }
    // Recursive neighbor check
    List<Bubble> FindConnectedSameColor(Bubble root)
    {
        List<Bubble> result = new List<Bubble>();
        Queue<Bubble> toCheck = new Queue<Bubble>();

        toCheck.Enqueue(root);
        result.Add(root);

        while (toCheck.Count > 0)
        {
            Bubble current = toCheck.Dequeue();
            Collider2D[] hits = Physics2D.OverlapCircleAll(current.transform.position, 1);

            foreach (Collider2D c in hits)
            {
                Bubble neighbor = c.GetComponent<Bubble>();
                if (neighbor != null && neighbor.type == root.type && !result.Contains(neighbor))
                {
                    result.Add(neighbor);
                    toCheck.Enqueue(neighbor);
                }
            }
        }

        return result;
    }
}