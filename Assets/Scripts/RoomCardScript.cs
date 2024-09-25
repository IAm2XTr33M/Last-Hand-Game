using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks.Data;

public class RoomCardScript : MonoBehaviour
{
    Lobby roomLobby;
    [SerializeField] Button joinButton;
    [SerializeField] TextMeshProUGUI roomNameText;

    private void Start()
    {
        joinButton.onClick.AddListener(Join);
    }
    void Join()
    {
        GameManager.instance.JoinPublicRoom(roomLobby);
    }

    public void SetCard(Lobby _lobby , string _user)
    {
        roomNameText.text = _user + "'s room";
        roomLobby = _lobby;

        RectTransform trans = GetComponent<RectTransform>();

        Vector3 pos = trans.anchoredPosition;
        trans.localScale = new Vector3(1, 1, 1);
        trans.localPosition = new Vector3(pos.x, pos.y, 0); 
    }

    public void JoinLobby()
    {
        GameManager.instance.JoinPublicRoom(roomLobby);
    }
}
