using UnityEngine;

public class RottenPotato : MonoBehaviour
{

    public float health = 1000;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
      //  health -= 0.4f;
    }
}
