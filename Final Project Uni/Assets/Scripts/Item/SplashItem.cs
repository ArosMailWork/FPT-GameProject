using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpeningEffect : MonoBehaviour
{
    public GameObject chestLid; // Reference to the chest lid
    public List<GameObject> items; // List of items to burst out
    public Transform spawnPoint; // Where the items will spawn from
    public float lidOpenAngle = 90f; // How much the lid should open
    public float minBurstForce = 3f; // Minimum force applied to items
    public float maxBurstForce = 7f; // Maximum force applied to items
    public float burstRadius = 1f; // Radius of the burst effect
    public float minUpForce = 1f; // Minimum upward force applied to items
    public float maxUpForce = 3f; // Maximum upward force applied to items

    private bool isOpened = false;

    void OnMouseDown()
    {
        if (!isOpened)
        {
            OpenChest();
            BurstItems();
            isOpened = true;
        }
    }

    void OpenChest()
    {
        chestLid.transform.Rotate(Vector3.right, lidOpenAngle);
    }

    public void BurstItems()
    {
        foreach (GameObject item in items)
        {
            GameObject newItem = Instantiate(item, spawnPoint.position, Quaternion.identity);
            newItem.SetActive(true);
            Rigidbody rb = newItem.GetComponent<Rigidbody>();

            if (rb != null)
            {
                float randomBurstForce = Random.Range(minBurstForce, maxBurstForce);
                float randomUpForce = Random.Range(minUpForce, maxUpForce);
                Vector3 force = Random.insideUnitSphere * randomBurstForce + Vector3.up * randomUpForce;
                rb.AddForce(force, ForceMode.Impulse);
            }
        }
    }
}

