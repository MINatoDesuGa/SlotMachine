using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bet : MonoBehaviour
{
    [SerializeField]
    private int _amount;

    [SerializeField]
    private Button _betButton;
    [SerializeField]
    private TMP_Text _amountText;

    private void Start() {
        Init();
    }
    private void OnEnable() {
        UpdateButtonInteraction();
    }
    private void OnDestroy() {
        _betButton.onClick.RemoveAllListeners();
    }
    //=======================================================================================//
    private void Init() {
        _amountText.text = "BET " + _amount.ToString(); //inits bet amount text
        _betButton.onClick.AddListener(OnBetClick);
        UpdateButtonInteraction();
    }

    private void OnBetClick() {
        GameManager.Instance.UpdateBalance(_amount, false);
        GameManager.OnBetClick.Invoke(); //trigger bet click event
    }
    /// <summary>
    /// enable/disable bet button interaction based on balance amount
    /// </summary>
    private void UpdateButtonInteraction() {
        if(GameManager.Instance == null) return;
        _betButton.interactable = GameManager.Instance.IsBalanceSufficient(_amount);
    }
}
