using Unity.VisualScripting;
using UnityEngine;

public class Mop : MonoBehaviour
{
    GridScript grid;

    SpriteRenderer spriteRenderer;


    public Sprite bucketed;
    public Sprite notBucketed;

    bool dragged = false;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        grid = GameObject.Find("GridManager").GetComponent<GridScript>();
    }

    private void OnMouseDrag()
    {
        dragged = true;
        spriteRenderer.sprite = notBucketed;
    }

   
    private void OnMouseUp()
    {
        if (dragged)
        {
            spriteRenderer.sprite = bucketed;
            dragged = false;
        }
  
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        GridObject gridObject = this.gameObject.GetComponent<GridObject>();

        if (collision.gameObject.tag == "Water" && gridObject.isDragging && collision.gameObject.name != "Water(Clone)")
        {
            Vector2Int gridPos = grid.WorldToGrid(collision.gameObject.transform.position);

            grid.levelGrid[gridPos.x, gridPos.y] = TileType.floor;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Splat")
        {
            Destroy(collision.gameObject);
        }
    }
}
