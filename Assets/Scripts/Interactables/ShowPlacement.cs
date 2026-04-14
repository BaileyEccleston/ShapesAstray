using UnityEngine;

public class ShowPlacement : MonoBehaviour
{

    GameObject placement;


    private void Start()
    {
        placement = transform.GetChild(0).gameObject;
    }


    private void OnMouseOver()
    {
        placement.SetActive(true);
    }

    private void OnMouseExit()
    {
        placement.SetActive(false);
    }

    private void OnMouseDrag()
    {
        placement.SetActive(true);
    }
}
