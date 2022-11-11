using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorNormal;
    public Vector2 normalCursorHotSpot;

    public Texture2D cursorInteract;
    public Vector2 interactCursorHotSpot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // change cursor when hovering over the radio
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.name == "Radio")
            {
                Cursor.SetCursor(cursorInteract, interactCursorHotSpot, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(cursorNormal, normalCursorHotSpot, CursorMode.Auto);
            }
        }
        
        
    }

    public void OnButtonCursorEnter()
    {
        Cursor.SetCursor(cursorInteract, interactCursorHotSpot, CursorMode.Auto);
    }

    public void OnButtonCursorExit()
    {
        Cursor.SetCursor(cursorNormal, normalCursorHotSpot, CursorMode.Auto);
    }
}
