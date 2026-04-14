using UnityEngine;

public class PlugLeakingPipe : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GridObject gridObject = this.gameObject.GetComponent<GridObject>();
        if (collision.gameObject.name == "LeakyPipe(Clone)")
        {
            Water[] waters = FindObjectsByType<Water>(FindObjectsSortMode.InstanceID);

            for (int i = 0;  i < waters.Length; i++)
            {
                waters[i].shouldSpread = false; 
                LeaveBehindObject leaveBehindObject = this.gameObject.GetComponent<LeaveBehindObject>();
                Destroy(leaveBehindObject.source);
            }
        }
    }
}
