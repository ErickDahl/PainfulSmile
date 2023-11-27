using UnityEngine;
using TMPro;

public class ShowGameTime : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    void Update()
    {
        _text.text = GameManager.Instance.GameSessionTime.ToString();
    }
}
