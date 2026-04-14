using UnityEngine;
using System.Collections;

public class RoomLightSwitch : MonoBehaviour
{
    public GameObject room;

    Renderer[] renderers;
    Color color;

    [SerializeField] bool lightSwitch;

    float fadeSpeed = 2f;

    void Start()
    {
        lightSwitch = true;
        renderers = room.GetComponentsInChildren<Renderer>();
        foreach(Renderer r in renderers)
        {
            color = r.material.color;
        }

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseScreen = Input.mousePosition;
            mouseScreen.z = -Camera.main.transform.position.z;

            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            Vector2 mousePos = new Vector2(mouseWorld.x, mouseWorld.y);

            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null && hit.gameObject == gameObject)
            {
                ToggleLight();
            }
        }
    }

    void ToggleLight()
    {
        if (lightSwitch)
        {
            StartCoroutine(Fade(1f, 0f));
            lightSwitch = false;
        }
        else
        {
            room.SetActive(true);
            StartCoroutine(Fade(0f, 1f));
            lightSwitch = true;
        }
    }

    IEnumerator Fade(float start, float end)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;

            float alpha = Mathf.Lerp(start, end, t);
            color.a = alpha;
            foreach (Renderer r in renderers)
            {
                r.material.color = color;
            }
          

            yield return null;
        }

        if (end == 0f)
        {
            room.SetActive(false);
        }
    }
}

