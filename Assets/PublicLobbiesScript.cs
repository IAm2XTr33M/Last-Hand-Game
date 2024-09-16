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

public class PublicLobbiesScript : NetworkBehaviour
{
    public RoomCardScript roomCardPrefab;
    public GameObject content;

    bool isRefreshing = false;

    Dictionary<Lobby, RoomCardScript> roomLobbyCards = new Dictionary<Lobby, RoomCardScript>();


    private void Start()
    {
        StartCoroutine(RefreshDelay());
    }

    IEnumerator RefreshDelay()
    {
        RefreshPublicLobbyList();
        yield return new WaitForSeconds(1f);
        StartCoroutine(RefreshDelay());
    }

    public async void RefreshPublicLobbyList()
    {
        if (isRefreshing)
        {
            return;
        }

        isRefreshing = true;

        try
        {

            List<Lobby> publicLobbies = await GameManager.instance.GetAllPublicLobbies();

            List<Lobby> lobbiesToRemove = new List<Lobby>();

            foreach (var pair in roomLobbyCards)
            {
                if (!publicLobbies.Contains(pair.Key))
                {
                    Destroy(pair.Value.gameObject);
                    lobbiesToRemove.Add(pair.Key);
                }
                if (pair.Value == null)
                {
                    lobbiesToRemove.Add(pair.Key);
                }
            }
            foreach (var lobby in lobbiesToRemove)
            {
                roomLobbyCards.Remove(lobby);
            }

            foreach (var lobby in publicLobbies)
            {
                if (!roomLobbyCards.ContainsKey(lobby))
                {
                    RoomCardScript lobbyCard = Instantiate(roomCardPrefab);
                    lobbyCard.transform.SetParent(content.transform);
                    lobbyCard.SetCard(lobby,lobby.Owner.Name);
                    roomLobbyCards.Add(lobby, lobbyCard);
                }
            }
        }
        finally
        {
            isRefreshing = false;
        }
    }
}
