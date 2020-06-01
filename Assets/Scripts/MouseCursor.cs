using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorImage;
    
    void Start()
    {
        Vector2 cursorHotspot = new Vector2 (cursorImage.width / 2, cursorImage.height / 2);
        Cursor.SetCursor(cursorImage, cursorHotspot, CursorMode.ForceSoftware);    
    }

}
