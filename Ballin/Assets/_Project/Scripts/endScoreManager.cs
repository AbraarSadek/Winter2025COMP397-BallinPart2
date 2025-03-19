using UnityEngine;
using TMPro;

public class EndScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private TMP_Text coinDisplay;

    [SerializeField] private FloatSO score;
    [SerializeField] private FloatSO cointCount;
    void Start()
    {
        scoreDisplay.text = score.Value.ToString();
        coinDisplay.text = cointCount.Value.ToString();

    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
