using UnityEngine;

public class PushableBox : MonoBehaviour
{
    public float pushForce = 3f;

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Vector3 pushDirection = collision.contacts[0].normal * -1f;
            rb.AddForce(pushDirection * pushForce, ForceMode.Force);
        }
    }
}
