using UnityEngine;

public class SlotTrigger : MonoBehaviour {
    [SerializeField]
    private Sprite _idleState;
    [SerializeField]
    private Sprite _triggerState;
    [SerializeField]
    private SpriteRenderer _triggerSpriteRenderer;
    private void Awake() {
        //TODO: subscribe to trigger event
        GameManager.OnBetClick += OnBetClicked;
    }
    private void OnDestroy() {
        GameManager.OnBetClick -= OnBetClicked;    
    }
    /// <summary>
    /// update state on bet click
    /// </summary>
    private void OnBetClicked() {
        _triggerSpriteRenderer.sprite = _triggerState; //enables trigger activate sprite
        Invoke(nameof(ResetState), 0.5f); 
    }
    /// <summary>
    /// resets to idle state
    /// </summary>
    private void ResetState() { 
        _triggerSpriteRenderer.sprite = _idleState;
    }
}
