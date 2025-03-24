using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Game : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _movementPointText;
    [SerializeField] TextMeshProUGUI textTimer;

    public RSO_MovementPoints movementPoints;

    [SerializeField] private RSE_OnPlayerDie rseOnPlayerDie;
    [SerializeField] private RSE_OnPlayerFinishLevel rseOnPlayerFinishLevel;

    [SerializeField] private int TimeMax;

    private int time;
    private Coroutine timer;

    private void OnEnable()
    {
        movementPoints.onValueChanged += UpdateMovementPointText;
        rseOnPlayerFinishLevel.action += ResetTimer;
    }

    private void OnDisable()
    {
        if(timer != null)
        {
            StopCoroutine(timer);
        }
        
        movementPoints.onValueChanged -= UpdateMovementPointText;
        rseOnPlayerFinishLevel.action -= ResetTimer;
    }

    private void Start()
    {
        time = 0;
        textTimer.text = $"Timer Left: {TimeMax}";
        timer = StartCoroutine(Timer());
    }

    private void ResetTimer()
    {
        time = 0;
        textTimer.text = $"Timer Left: {TimeMax}";

        StopCoroutine(timer);

        timer = StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);

        time++;

        textTimer.text = $"Timer Left: {TimeMax - time}";

        if (time >= TimeMax)
        {
            rseOnPlayerDie.RaiseEvent();
        }

        timer = StartCoroutine(Timer());
    }

    private void UpdateMovementPointText(int movementPoint)
    {
        _movementPointText.text = $"Movement Points Left: {movementPoint}";
    }
}
