using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks;
using System.Threading.Tasks;
using System;
using Steamworks.Data;
using Unity.VisualScripting;

public class RoomController : MonoBehaviour
{
    public RawImage playerOneImage;
    public TextMeshProUGUI playerOneName;
    Friend playerOne;

    public RawImage playerTwoImage;
    public TextMeshProUGUI playerTwoName;
    Friend playerTwo;

    public async void SetPlayerOneInfo(Friend _player)
    {
        playerOne = _player;
        playerOneName.text = playerOne.Name;

        playerOneImage.texture = await GetUserProfilePicture((ulong)playerOne.Id);

    }

    public async void SetPlayerTwoInfo(Friend _player)
    {
        playerTwo = _player;
        playerTwoName.text = playerTwo.Name;
        playerTwoImage.texture = await GetUserProfilePicture((ulong)playerTwo.Id);
    }

    public async Task<Texture2D> GetUserProfilePicture(ulong _SteamId)
    {
        Debug.Log(_SteamId);
        if (_SteamId != null)
        {
            Steamworks.Data.Image? profilepic = await SteamFriends.GetLargeAvatarAsync(_SteamId);

            if (profilepic.HasValue)
            {
                var avatar = new Texture2D((int)profilepic.Value.Width, (int)profilepic.Value.Height, TextureFormat.ARGB32, false);
                avatar.filterMode = FilterMode.Trilinear;

                for (int x = 0; x < profilepic.Value.Width; x++)
                {
                    for (int y = 0; y < profilepic.Value.Height; y++)
                    {
                        var p = profilepic.Value.GetPixel(x, y);
                        avatar.SetPixel(x, (int)profilepic.Value.Height - y, new UnityEngine.Color(p.r / 255.0f, p.g / 255.0f, p.b / 255.0f, p.a / 255.0f));
                    }
                }

                avatar.Apply();
                return avatar;
            }
        }
        return new Texture2D(1, 1, TextureFormat.ARGB32, false);
    }
}
