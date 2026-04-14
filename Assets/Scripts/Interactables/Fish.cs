using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField]
    public int health = 400;

    GridScript grid;
    public int detectionRange = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = FindAnyObjectByType<GridScript>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayerInRange();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    //hello
    /// 
    /// </summary>
    void CheckForPlayerInRange()
    {
        EnemyMove[] cats = FindObjectsOfType<EnemyMove>();

        Vector2Int fishCell = grid.WorldToGrid(transform.position);

        foreach (EnemyMove cat in cats)
        {
            Vector2Int catCell = grid.WorldToGrid(cat.transform.position);

            float distance = Vector2.Distance(fishCell, catCell);

            if (distance <= detectionRange)
            {
                cat.TargetFish(this.gameObject);
            }
        }
    }
}
