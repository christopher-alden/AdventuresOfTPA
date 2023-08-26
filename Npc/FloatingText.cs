using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    private Transform cam;
    private TextMeshProUGUI textMeshPro;
    [SerializeField] private Transform player;
    [SerializeField] private float range = 10f;

    private void Start()
    {
        cam = Camera.main.transform;
        textMeshPro = GetComponent<TextMeshProUGUI>();
        SetTextVisible(true);
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);

        float distance = Vector3.Distance(player.position, transform.position);

        bool isInRange = distance <= range;
        SetTextVisible(isInRange);
    }

    private void SetTextVisible(bool isVisible)
    {
        textMeshPro.enabled = isVisible;
    }
}
