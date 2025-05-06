using System;
using System.Collections;
using UnityEngine;

public class SlotSpinHandler : MonoBehaviour
{
    private static int SpinCompletedCount = 0; //keeps track of slot spin complete status

    [SerializeField]
    private float _spinDuration = 5f; //initial spin duration before calculating random symbol pos to stop to

    private readonly WaitForSeconds _spinDelayTime = new(GameConfig.SLOT_SPIN_DELAY); //delay between each slot spin update

    private readonly System.Random _randomNumberGenerator = new(); //system class random to get random number

    private Coroutine _spinCoroutine;

    private void Awake() {
        Init();
    }
    private void OnDestroy() {
        GameManager.OnBetClick -= OnBetClicked;
    }
    //==========================================================//
    private void Init() {
        GameManager.OnBetClick += OnBetClicked;
    }
    private void OnBetClicked() {
        if(_spinCoroutine != null) { 
            StopCoroutine(_spinCoroutine);
        }

        _spinCoroutine = StartCoroutine(Spin());
    }
    /// <summary>
    /// Handles slot spin updates
    /// </summary>
    /// <returns></returns>
    private IEnumerator Spin() {
        var timeElapsed = Time.time;
        //Fixed set time spin 
        while ((Time.time - timeElapsed) < _spinDuration) {
            SymbolsMovement(); 
            
            timeElapsed += Time.deltaTime;

            yield return _spinDelayTime;
        }

        //calculate random symbol pos to stop
        var randomIdx = _randomNumberGenerator.Next(0, GameConfig.MAX_SYMBOLS);
        var stopYPos = GameConfig.SLOT_SYMBOLS_yPOS[randomIdx];

        GameManager.Instance.CheckAndUpdateSlotResults(stopYPos); //update slot result list with its respective symbol

        int currentRound = 0; 
        // spin to desired random symbol pos after configured rounds 
        while (currentRound < GameConfig.MAX_FINAL_SPIN_ROUNDS) {
            SymbolsMovement();

            if(transform.position.y ==  stopYPos ) { //reached resultant symbol yPos so updating round
                currentRound++;
            }

            yield return _spinDelayTime;
        }

        SpinCompletedCount++; 
        if(SpinCompletedCount is GameConfig.MAX_SLOTS) { //all slot spin finished
            SpinCompletedCount = 0;
            GameManager.OnSlotSpinComplete.Invoke(); //trigger spin complete event
        }
    }
    /// <summary>
    /// handles symbol movement distance and circular motion reset for symbol obj
    /// </summary>
    private void SymbolsMovement() {
        transform.position += Vector3.up * GameConfig.SLOT_SPIN_SPEED; //vertical motion

        if (transform.position.y is GameConfig.SLOT_SPIN_MAX_yTHRESHOLD) { //slot holder reaches max symbol dummy point 
            //reset to beginning of symbol dummy to give an illusion of circular motion of symbols
            transform.position = new Vector3(transform.position.x, GameConfig.SLOT_SPIN_RESET_yPOS, transform.position.z);
        }
    }
}
