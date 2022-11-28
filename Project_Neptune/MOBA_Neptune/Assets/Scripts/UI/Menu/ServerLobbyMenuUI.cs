using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ServerLobbyMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField createRoomTMPInputField;
    [SerializeField] private TMP_InputField joinRoomTMPInputField;

    public void CreateRoom()
    {
        if (NetworkManager.Instance == null) Debug.LogError("no NetworkManager");
        NetworkManager.Instance?.CreateRoom(createRoomTMPInputField.text);
    }

    public void JoinRoom()
    {
        if(NetworkManager.Instance == null) Debug.LogError("no NetworkManager");
        NetworkManager.Instance?.JoinRoom(joinRoomTMPInputField.text);
    }
}
