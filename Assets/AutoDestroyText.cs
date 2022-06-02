using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AutoDestroyText : MonoBehaviour
{
    public float AutoDestroyTime;

    private TextMeshProUGUI Text;
    private float SpawnTime;

    public delegate void DeathEvent(AutoDestroyText Instance, Vector2 Position);
    public DeathEvent OnDeath;
    public UnityEvent<AutoDestroyText, Vector2> UnityEventOnDeath;

    private void Update()
    {
        float remainingTime = (SpawnTime + AutoDestroyTime) - Time.time;

        Text.SetText($"{remainingTime:N2}");

        if (remainingTime <= 0)
        {
            if (UnityEventOnDeath != null)
            {
                UnityEventOnDeath.Invoke(this, transform.position);
            }
            gameObject.SetActive(false);
            OnDeath?.Invoke(this, transform.position);
        }
    }

    private void Awake()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        SpawnTime = Time.time;
    }
}
