using UnityEngine;

[CreateAssetMenu(menuName = "Ships/Ship Damage States")]
public class ShipDamageStatesSO : ScriptableObject
{
    [SerializeField]
    private Sprite _healthySprite;

    [SerializeField]
    private Sprite _damagedSprite;

    [SerializeField]
    private Sprite _badlyDamagedSprite;

    [SerializeField]
    private Sprite _deadSprite;

    public Sprite HealthySprite => _healthySprite;
    public Sprite DamagedSprite => _damagedSprite;
    public Sprite BadlyDamagedSprite => _badlyDamagedSprite;
    public Sprite DeadSprite => _deadSprite;
}
