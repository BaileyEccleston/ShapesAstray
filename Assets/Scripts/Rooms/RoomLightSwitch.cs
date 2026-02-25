using UnityEngine;

public class RoomLightSwitch : MonoBehaviour
{
    public GameObject room;

    [SerializeField] bool lightSwitch;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseScreen = Input.mousePosition;

            // Distance from camera (-10) to objects at 0
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
            Debug.Log("Switch light off");
            lightSwitch = false;
            room.SetActive(true);
        }
        else
        {
            Debug.Log("Switch light on");
            lightSwitch = true;
            room.SetActive(false);
        }
    }


}
