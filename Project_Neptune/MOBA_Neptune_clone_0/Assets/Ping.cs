using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Ping : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ping;
    [SerializeField] private TextMeshProUGUI master;
    [SerializeField] TMP_InputField tickInput;
    // Start is called before the first frame update
    void Start()
    {
        ping = GetComponent<TextMeshProUGUI>();
        master.text = $"Master : {PhotonNetwork.IsMasterClient}";
    }

    // Update is called once per frame
    void Update()
    {
        ping.text = $"Ping : {PhotonNetwork.GetPing()}";
    }

    public void UpdateTick()
    {
        PhotonNetwork.SerializationRate = Int32.Parse(tickInput.text);
    }
}
