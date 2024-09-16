using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using Steamworks;
using System.Threading.Tasks;
using Steamworks.Data;
using System;
using Netcode.Transports.Facepunch;


public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public string gameName = "";
    public bool onlySearchThisGame = true;

    public FacepunchTransport transport;
    [HideInInspector] public Lobby? currentLobby;

    [SerializeField] GameObject HomePageUI;
    [SerializeField] GameObject joinRoomsPageUI;
    [SerializeField] GameObject RoomPageUI;
    [SerializeField] GameObject LoadingScreenUI;

    bool isCreatingLobby = false;

    private void Awake()
    {
        if(instance == null) { instance = this; } else { Destroy(this); }
    }

    private void Start()
    {
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
        //SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeft;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        //NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }


    async public void CreateLobby()
    {
        if (!isCreatingLobby)
        {
            Debug.Log("CreatingLobby");

            isCreatingLobby = true;
            HomePageUI.SetActive(false);
            LoadingScreenUI.SetActive(true);

            NetworkManager.Singleton.StartHost();

            Lobby? createdLobby = await SteamMatchmaking.CreateLobbyAsync(2);

            createdLobby.Value.SetData("LobbyOwnerID", createdLobby.Value.Owner.Id.ToString());
            createdLobby.Value.SetData("GameName", gameName);

            createdLobby.Value.SetGameServer(createdLobby.Value.Owner.Id);

            currentLobby = createdLobby;

            isCreatingLobby = false;
        }
    }

    public async void JoinPublicRoom(Lobby _lobby)
    {
        RoomEnter joinedLobby = await _lobby.Join();
        if (joinedLobby != RoomEnter.Success)
        {
            Debug.Log("Failed to join lobby");
        }
        else
        {
            currentLobby = _lobby;
            if (!NetworkManager.Singleton.IsHost)
            {
                Start_Client(_lobby.Owner.Id);
            }
        }

        NetworkManager.Singleton.StartClient();
    }
    public void Start_Client(SteamId _sId)
    {
        transport.targetSteamId = _sId.Value;

        NetworkManager.Singleton.StartClient();
    }

    public async Task<List<Lobby>> GetAllPublicLobbies()
    {
        try
        {
            Lobby[] foundLobbies = await SteamMatchmaking.LobbyList.RequestAsync();
            List<Lobby> lobbies = foundLobbies != null ? new List<Lobby>(foundLobbies) : new List<Lobby>();
            List<Lobby> filteredLobbies = new List<Lobby>();
            if (onlySearchThisGame)
            {
                foreach (var lobby in lobbies)
                {
                    if (lobby.GetData("GameName") == gameName)
                    {
                        filteredLobbies.Add(lobby);
                    }
                }
                return filteredLobbies;
            }
            else
            {
                return lobbies;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return new List<Lobby>();
        }
    }

    public async Task<bool> CheckIfJoinable(Lobby? _lobby)
    {
        List<Lobby> currentPublicLobbies = await GetAllPublicLobbies();
        return currentPublicLobbies.Contains(_lobby.Value);
    }

    void OnLobbyCreated(Result _result, Lobby _lobby)
    {
        if (_result == Result.OK)
        {
            RoomPageUI.SetActive(true);
            LoadingScreenUI.SetActive(false);
            RoomPageUI.GetComponent<RoomController>().SetPlayerOneInfo(_lobby.Owner);
        }
        else
        {
            isCreatingLobby = false;
            HomePageUI.SetActive(true);
            RoomPageUI.SetActive(false);
            LoadingScreenUI.SetActive(false);
        }
    }

    void OnLobbyMemberJoined(Lobby _lobby, Friend _player)
    {
        RoomPageUI.GetComponent<RoomController>().SetPlayerTwoInfo(_player);
    }

    void OnClientConnected(ulong _client)
    {

    }
}
