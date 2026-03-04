using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RottenPotato : MonoBehaviour
{
    bool coroutineRunning = false;
    private Coroutine coroutine = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void Placed()
    {
        Debug.Log("Placed");
        if (!coroutineRunning)
        {
            coroutine = StartCoroutine(RottenPotatoLife());
        }
        else
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }

    IEnumerator RottenPotatoLife()
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(15f);
        coroutineRunning = false;
        Destroy(gameObject);
    }
}
