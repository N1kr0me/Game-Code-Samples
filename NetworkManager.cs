using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject[] characterPrefabs;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        SpawnCharacter();
    }

    private void SpawnCharacter()
    {
        int characterIndex = PlayerPrefs.GetInt("CharacterType");
        Vector3 spawnPosition = new Vector3(-7.5f, 11f,-1f);
        Quaternion spawnRotation = Quaternion.identity;
        PhotonNetwork.Instantiate(characterPrefabs[characterIndex].name, spawnPosition, spawnRotation);
    }
}