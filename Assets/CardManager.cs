using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;

public class CardManager : NetworkBehaviour
{
    public List<CardCombo> cardCombos = new List<CardCombo>();

    public List<CardCombo> playerOneCombos = new List<CardCombo>();
    public List<CardCombo> playerTwoCombos = new List<CardCombo>();

    private TaskCompletionSource<int> randomCardNumberTaskSource;

    bool isPlayerOne = false;

    private void Start()
    {
        isPlayerOne = IsHost;
    }

    //Get a random card combo
    public CardCombo GetRandomCard()
    {
        CardCombo randomlyPickedCard = cardCombos[Random.RandomRange(0, cardCombos.Count)];

        if (isPlayerOne) { 
            playerOneCombos.Add(randomlyPickedCard); }
        else { 
            playerTwoCombos.Add(randomlyPickedCard); }

        cardCombos.Remove(randomlyPickedCard);



        return (randomlyPickedCard);
    }

    [ServerRpc]
    public void CardPicked_ServerRPC(int number, int player)
    {

    }
    [ClientRpc]
    public void CardPicked_ClientRPC(int number, int player)
    {
        
    }

    bool DoCombosContainNumber(int number)
    {
        bool result = false;
        foreach (var combo in cardCombos)
        {
            if(combo.number == number) { result = true; }
        }
        return result;
    }
}

[System.Serializable]
public class CardCombo
{
    public PlayingCard card;
    public int number;
}
