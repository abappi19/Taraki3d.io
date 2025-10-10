using UnityEngine;
using System.Collections;

public class MouthController : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            EatFood(other.gameObject);
            if (playerController == null) return;
            playerController.ImproveHealth(other.gameObject.transform.localScale.x * 2f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter: " + collision.gameObject.name + " " + collision.gameObject.tag);
        if (collision.gameObject.transform.IsChildOf(player.transform)) return; // own body
        if (!collision.gameObject.CompareTag("Player")) return; // not collided with player
        if (playerController == null) return;
        playerController.DestroyPlayer();
    }

    private void EatFood(GameObject food)
    {
        StartCoroutine(EatFoodRoutine(food));
    }

    private IEnumerator EatFoodRoutine(GameObject food)
    {
        if (food == null) yield break;

        Collider foodCollider = food.GetComponent<Collider>();
        if (foodCollider != null) foodCollider.enabled = false;
        Rigidbody rb = food.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        Vector3 startPos = food.transform.position;
        Vector3 endPos = transform.position;
        Vector3 startScale = food.transform.localScale;
        Vector3 endScale = Vector3.zero;

        float duration = 0.25f;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float e = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t));
            food.transform.position = Vector3.Lerp(startPos, endPos, e);
            food.transform.localScale = Vector3.Lerp(startScale, endScale, e);
            yield return null;
        }

        if (food != null)
        {
            Destroy(food);
        }
    }
}