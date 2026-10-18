using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Player Weapon Reference")]
    public GameObject playerWeaponToActivate; // The gun nested under Camera Main

    [Header("Bob & Rotate Juice")]
    public float rotationSpeed = 45f;
    public float bobbingSpeed = 2f;
    public float bobbingHeight = 0.05f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Gentle rotation and bobbing effect so players can spot it
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        transform.position = startPos + new Vector3(0f, Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player walked into the pickup
        if (other.CompareTag("Player"))
        {
            if (playerWeaponToActivate != null)
            {
                playerWeaponToActivate.SetActive(true);
            }

            // Remove the world weapon model
            Destroy(gameObject);
        }
    }
}
