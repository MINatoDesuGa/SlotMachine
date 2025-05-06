using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static Action OnBetClick; //this event gets triggered on bet option click
    public static Action OnSlotSpinComplete; //this event gets triggered on all slot spin complete
    private int _currentBetAmount;

    private int _playerBalanceAmount = 100;

    [SerializeField]
    private GameObject _betOptionsHolder; //this obj holds bet & reset UI

    [Space(10)]
    [Header("Results panel")]
    [SerializeField]
    private GameObject _resultPanel; //feedback panel on each slot spin results
    [SerializeField]
    private TMP_Text _resultTxt; //feedback text based on slot spin results

    [Space(10)]
    [Header("Player Balance")]
    [SerializeField]
    private TMP_Text _balanceText; 


    private List<SlotSymbols> _slotSpinResults = new(); //this list is used to keep track of all slot spin results
    private void Awake() {
        if(Instance == null) { 
            Instance = this;
        } else {
            Destroy(this);
        }

        Init();
    }
    private void OnDestroy() {
        OnBetClick -= OnBetClicked;
        OnSlotSpinComplete -= OnSlotSpinCompleted;
    }
    //===================================================================//
    private void Init() {
        OnBetClick += OnBetClicked;
        OnSlotSpinComplete += OnSlotSpinCompleted;
    }
    /// <summary>
    /// reloads scene on reset click
    /// </summary>
    public void Reset() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// updates player balance amount
    /// </summary>
    /// <param name="amount"></param> amount to be updated
    /// <param name="isCredit"></param> either credit or debit
    public void UpdateBalance(int amount, bool isCredit) {
        if(isCredit) { 
            _playerBalanceAmount += amount;
        } else {
            _playerBalanceAmount -= amount;
        }
        _currentBetAmount = amount;
        _balanceText.text = _playerBalanceAmount.ToString();
    }
    public bool IsBalanceSufficient(int betAmount) {
        return _playerBalanceAmount >= betAmount;
    }
    /// <summary>
    /// On bet click event actions
    /// </summary>
    private void OnBetClicked() { 
        ActivateBetOptionsHolder(false); //disable bet options UI
    }
    /// <summary>
    /// enable / disable bet options UI
    /// </summary>
    /// <param name="active"></param>
    private void ActivateBetOptionsHolder(bool active) => _betOptionsHolder.SetActive(active);
    /// <summary>
    /// Slots spin complete event actions
    /// </summary>
    private void OnSlotSpinCompleted() {
        /*foreach(var symbols in _slotSpinResults) {
            print(symbols);
        }*/
        Invoke(nameof( CheckSymbolAndReward), 1); //after 1 sec, check slot results and reward based on results
    }
    /// <summary>
    /// Updates SlotSpinResult list based on slot spin results
    /// </summary>
    /// <param name="ySymbolPos"></param> keyfactor determining symbols from game config
    public void CheckAndUpdateSlotResults(float ySymbolPos) {
        switch(ySymbolPos) { 
            case GameConfig.BAR_SYMBOL_yPOS:
                _slotSpinResults.Add(SlotSymbols.BAR);
                break;
            case GameConfig.BELL_SYMBOL_yPOS:
                _slotSpinResults.Add(SlotSymbols.BELL);
                break;
            case GameConfig.JACKPOT_SYMBOL_yPOS:
                _slotSpinResults.Add(SlotSymbols.JACKPOT);
                break;
            case GameConfig.BERRIES_SYMBOL_yPOS:
                _slotSpinResults.Add(SlotSymbols.BERRIES);
                break;
        }
    }
    /// <summary>
    /// Check symbol spin results, update feedback popup and assign rewards based on results 
    /// </summary>
    private void CheckSymbolAndReward() {
        var symbolMatchStatus = CheckMatchingSymbols(_slotSpinResults[0]); //pass first slot result to check
        switch(symbolMatchStatus) {
            case SymbolMatchStatus.NONE: // no slot symbols matches
                _resultTxt.text = "Try again";
                break;
            case SymbolMatchStatus.DOUBLE: // atleast 2 slot symbols matches
                _resultTxt.text = "Wow 2 Matched";
                UpdateBalance(_currentBetAmount, true); //revert balance amount
                break;
            case SymbolMatchStatus.TRIPLE: // 3 slot symbols matches (not jackpot )
                _resultTxt.text = "Excellent!";
                UpdateBalance(_currentBetAmount * 2, true); //reward x2 bet amount in balance
                break;
            case SymbolMatchStatus.JACKPOT: // 3 jackpot symbols 
                _resultTxt.text = "JACKPOT!!!!";
                UpdateBalance(_currentBetAmount * 3, true); //reward x3 bet amount in balance
                break;
        }
         _resultPanel.SetActive(true); 

        _slotSpinResults.Clear(); //reset spin results
       // ActivateBetOptionsHolder(true);
    }
    /// <summary>
    /// Get symbol match results upon comparing all slot spin results
    /// </summary>
    /// <param name="firstSlotSymbol"></param> first slot result
    /// <returns></returns>
    private SymbolMatchStatus CheckMatchingSymbols(SlotSymbols firstSlotSymbol) {
        if( _slotSpinResults[1] == firstSlotSymbol && _slotSpinResults[2] == firstSlotSymbol) { //all symbols match
            //check for jackpot else normal triple
            return (firstSlotSymbol is SlotSymbols.JACKPOT ? SymbolMatchStatus.JACKPOT : SymbolMatchStatus.TRIPLE);
        }
        if((_slotSpinResults[1] == firstSlotSymbol || _slotSpinResults[2] == firstSlotSymbol )
            || _slotSpinResults[1] == _slotSpinResults[2]) { //2 symbols match
            return SymbolMatchStatus.DOUBLE;
        }
        return SymbolMatchStatus.NONE;
    }
}
