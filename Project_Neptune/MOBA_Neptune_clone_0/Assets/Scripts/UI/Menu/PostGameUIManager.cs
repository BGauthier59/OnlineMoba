using GameStates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostGameUIManager : MonoBehaviour
{
    public static PostGameUIManager Instance;

    [SerializeField] private GameObject postGameCanvas;
    [SerializeField] private TextMeshProUGUI winningTeamText;
    [SerializeField] private TextMeshProUGUI resultText;

    [SerializeField] private Button rematchButton;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    public void DisplayPostGame(Enums.Team winner)
    {
        postGameCanvas.SetActive(true);
        winningTeamText.text = $"{winner} has won!";

        var playerTeam = GameStateMachine.Instance.GetPlayerTeam();
        resultText.text = playerTeam == winner ? "You won!" : "You lost!";
    }

    public void OnRematchClick()
    {
        Debug.Log("Does not work yet");
        return;
        rematchButton.interactable = false;
        GameStateMachine.Instance.SendSetToggleReady(true);
    }
}
