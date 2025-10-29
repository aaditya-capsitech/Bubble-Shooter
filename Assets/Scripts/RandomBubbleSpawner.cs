using UnityEngine;

public class RandomBubbleSpawner : MonoBehaviour
{
    public GameObject[] bubblePrefabs;
    public int rows = 6;
    public int columns = 7;
    public float bubbleSpacing = 0.5f;
    public Vector2 startPosition = new Vector2(-3.5f, 3f);
    //public Transform transform;
    public Transform bubbleGroup;
    void Start()
    {
        SpawnBubbleGrid();
    }

    void SpawnBubbleGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int index = Random.Range(0, bubblePrefabs.Length);

                float x = startPosition.x + (col * bubbleSpacing);
                float y = startPosition.y - (row * bubbleSpacing);

                if (row % 2 == 1)
                    x += bubbleSpacing / 2f;

                Vector3 pos = new Vector3(x, y, 0);
                Instantiate(bubblePrefabs[index], pos, Quaternion.identity, bubbleGroup);
            }
        }
    }
}
