using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    private bool canClick = true;

    private void OnMouseDown()
    {
        if (!canClick)
        {
            Debug.Log("Click blocked: cooldown active.");
            return;
        }

        StartCoroutine(ClickCooldown());
        // Check if the mouse is over the collider
        BoxCollider collider = GetComponent<BoxCollider>();

        if (collider == null)
        {
            Debug.LogError("BoxCollider not found on PlayerUIController.");
            return;
        }

        if (collider != null && collider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity))
        {
            // Call the function to handle the click event
            Debug.Log("Player UI Clicked");
            GameUIManager.Instance.OnPlayerUIClicked(this);
        }
    }

    private IEnumerator ClickCooldown()
    {
        canClick = false;
        yield return new WaitForSeconds(0.5f);
        canClick = true;
    }
}
