using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RottenPotato : MonoBehaviour
{
    private Coroutine coroutine = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void Placed()
    {
        Debug.Log("Placed");
        if (coroutine == null)
        {
            coroutine = StartCoroutine(RottenPotatoLife());
        }
        else
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    IEnumerator RottenPotatoLife()
    {

        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
    }
}
