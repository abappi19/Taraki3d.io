using UnityEngine;
using System.Collections;

public class MouthController : MonoBehaviour
{
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            EatFood(other.gameObject);
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController == null) return;
            playerController.ImproveHealth();
        }
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