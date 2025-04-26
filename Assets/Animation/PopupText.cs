using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    public void ShowText(string text)
    {
        GetComponent<TextMesh>().text = text;
    }
    public void DestroyPopup()
    {
        Destroy(gameObject);
    }
}
