using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RoundManager : NetworkBehaviour
{
    bool myTurn = false;

    //Will be mainly server / host code

    int playersStayed = 0;

    ulong playerOneClientId;
    ulong playerTwoClientId;

    List<CardCombo> playerOneCards = new List<CardCombo>();
    List<CardCombo> playerTwoCards = new List<CardCombo>();

    private void Start()
    {
        if (IsHost)
        {
            int turn = Random.Range(0, 1); //pick random turn to start of with
            SetMyTurn(turn == 0); // Set turn

            playerOneClientId = NetworkManager.Singleton.LocalClientId; // Set player 1 id
            foreach (var id in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if(id != playerOneClientId)
                {
                    playerTwoClientId = id;
                }
            } // Set player 2 id


            //StartFirstTurn_ServerRPC();
        }
    }

    public void Hit()
    {
        //Use CardManager to get a random card
        PlayerHit_ServerRPC();
    }

    //Make sure this also gets called after time runs out !!!!!!!!!!!!!!!!!!!!!!
    public void Stay()
    {
        PlayerStay_ServerRPC();
    }

    [ServerRpc]
    void PlayerHit_ServerRPC()
    {
        playersStayed = 0;
    }

    [ServerRpc]
    void PlayerStay_ServerRPC()
    {
        playersStayed++;
        if(playersStayed == 2)
        {
            EndRound_ClientRPC();
        }
    }

    [ClientRpc] // Check whos closer to the desired win amount here or after this
    void EndRound_ClientRPC()
    {
        //Do everything that finishes round on both clients

        if (IsHost)
        {
            //Calculate the winner

            //Get both scores
            int playerOnePoints = GetPlayerPoints(playerOneCards);
            int playerTwoPoints = GetPlayerPoints(playerTwoCards);

            if (playerOnePoints == playerTwoPoints) //check if i should be a draw due to equal points
            {
                GameDraw_ServerRPC();
                return;
            }

            int desiredPointsWinAmount = GameSettings.instance.desiredPointsWinAmount;

            bool playerOneResult = playerOnePoints < desiredPointsWinAmount;
            bool playerTwoResult = playerTwoPoints < desiredPointsWinAmount;

            if(!playerOneResult && !playerTwoResult) //check if it should be a draw if they are both over
            {
                GameDraw_ServerRPC();
                return;
            }

            if(playerOneResult && !playerTwoResult)
            {
                SetWinner_ServerRPC(playerOneClientId, playerTwoClientId);
                return;
            } //Player One wins if

            if(!playerOneResult && playerTwoResult)
            {
                SetWinner_ServerRPC(playerTwoClientId, playerOneClientId);
                return;
            }

        }
        else
        {
            //Do everything that finishes round client side
        }
        Debug.Log("Round will now be evaluated");
    }

    [ServerRpc]
    void SetWinner_ServerRPC(ulong winner, ulong loser)
    {

    }

    [ClientRpc]
    void RoundWon_ClientRPC(ulong id)
    {

    }

    [ClientRpc]
    void RoundLost_ClientRPC(ulong id)
    {

    }

    [ServerRpc]
    void GameDraw_ServerRPC()
    {

    }
    [ClientRpc]
    void GameDraw_ClientRPC()
    {

    }

    //Turns
    void SetMyTurn(bool _turn)
    {
        myTurn = _turn;
        SetOtherTurn_ClientRPC(!_turn);
    }
    [ClientRpc]
    void SetOtherTurn_ClientRPC(bool _turn)
    {
        if (!IsHost) { myTurn = _turn; }
    }

    int GetPlayerPoints(List<CardCombo> _list)
    {
        int points = 0;
        foreach (var combo in _list)
        {
            points += combo.number;
        }
        return points;
    }
}
