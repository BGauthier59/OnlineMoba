using Photon.Pun;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class UICommonPlayers : MonoBehaviour
{
    public static UICommonPlayers Instance;

    private PhotonView _photonView;
    [SerializeField] private TextMeshProUGUI violetTeamScore;
    [SerializeField] private TextMeshProUGUI orangeTeamScore;
    public Cashier orangeTeamCashier;
    public Cashier violetTeamCashier;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        _photonView = PhotonView.Get(gameObject);
        
        violetTeamScore.text = 0.ToString();
        orangeTeamScore.text = 0.ToString();
    }

    [PunRPC]
    public void SyncAllScores()
    {
        violetTeamScore.text = violetTeamCashier.cashierPoint.ToString();
        orangeTeamScore.text = orangeTeamCashier.cashierPoint.ToString();
    }

    public void OnScoreChange()
    {
        _photonView.RPC("SyncAllScores", RpcTarget.All);
    }
}
