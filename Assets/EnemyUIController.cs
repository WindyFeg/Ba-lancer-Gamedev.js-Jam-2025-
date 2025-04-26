using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;

public class EnemyUIController : MonoBehaviour
{
    private bool canClick = true;

    private void OnMouseDown()
    {
        // Check if the GameObject has the "Player" tag
        if (!CompareTag("Enemy"))
        {
            Debug.Log("Click ignored: GameObject is not tagged as 'Player'.");
            return;
        }

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

        if (collider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity))
        {
            // Call the function to handle the click event
            Debug.Log("Enemy UI Clicked");
            GameUIManager.Instance.OnEnemyUIClicked(hit.collider.gameObject.GetComponent<PlayerBehaviour>());
        }
    }

    private IEnumerator ClickCooldown()
    {
        canClick = false;
        yield return new WaitForSeconds(1f);
        canClick = true;
    }
}
