using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Vector2 _positionOffSet = new Vector2(0, 0.8f);

    private Slider _slider;
    private Health _health;

    void OnEnable()
    {
        _health.OnTakeDamage += UpdateHealthBar;
    }

    void OnDisable()
    {
        _health.OnTakeDamage -= UpdateHealthBar;
    }

    void Awake()
    {
        _health = GetComponent<Health>();
        _slider = GetComponentInChildren<Slider>();
    }

    void Update()
    {
        _slider.transform.rotation = Camera.main.transform.rotation;
        _slider.transform.position = transform.position + (Vector3)_positionOffSet;
    }

    private void UpdateHealthBar(float value)
    {
        _slider.value = value;
    }
}
