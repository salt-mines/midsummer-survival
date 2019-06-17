using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class LifePanel : MonoBehaviour
{
    public int margin = 7;

    public GameObject lifePrefab;

    private RectTransform rectTransform;
    private RectTransform lifeTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        lifeTransform = lifePrefab.GetComponent<RectTransform>();
    }

    public void SetMaxLives(int maxLives)
    {
        var lifeWidth = lifeTransform.sizeDelta.x;
        var lifeHeight = lifeTransform.sizeDelta.y;

        rectTransform.sizeDelta = new Vector2(margin + (lifeWidth + margin) * maxLives, lifeHeight + margin * 2);

        // Create a life object for each max life
        for (int i = 0; i < maxLives; i++)
        {
            var life = Instantiate(lifePrefab, transform);
            var rect = life.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(margin * (i + 1) + lifeWidth * i, 0);
        }
    }

    public void SetCurrentLives(int currLives)
    {
        if (currLives < 0) return;

        // Destroy unneeded life objects starting from the end
        for (int i = transform.childCount - 1; i >= currLives; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
