using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.Json;
using System;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;

[System.SerializableAttribute]
public class Player
{
    public string playerName;
    public string playFabID;
    public string playerID;
    public string customID;

	public int playerCurrentPuppyID;
    public int puppyCoins;
    public bool beagleUnlocked ,chiUnlocked,bullUnclocked,germanUnlocked;

	
	
}

public class PlayerManager : MonoBehaviour 
{


    public static PlayerManager instance = null;

    public Player player;
    public string playerJson ,playerJson2;
	public static Dictionary<string,int> VirtualCurrency;
    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PlayerManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

	void Awake()
	{
		 
	}
	// Use this for initialization
	void Start () 
	{
		AuthenticatePlayer();
 	 Debug.Log(PlayFabClientAPI.IsClientLoggedIn());
	}

    void AuthenticatePlayer()
    {
        Wenzil.Console.Console.Log("Calling local player login");

        UnityEngine.Social.localUser.Authenticate (success => 
		{
			if (success)
			{
				Wenzil.Console.Console.Log ("Authentication successful");
				string userInfo = "Username: " + UnityEngine.Social.localUser.userName + 
					"\nUser ID: " + UnityEngine.Social.localUser.id;//  + 
				
				player.playerID = UnityEngine.Social.localUser.id;
				player.playerName = UnityEngine.Social.localUser.userName;
				Wenzil.Console.Console.Log("PlayerName "+ player.playerName);
				Wenzil.Console.Console.Log("PlayerID "+ player.playerID);
				
				LoginWithPlayFab();
					 
				// if(String.IsNullOrEmpty(player.playerID))
				// {
				// 	// connect to net
				// 	Wenzil.Console.Console.Log("Player id is empty, connect to internet and try ");
				// 	//check from server
				// 	LoginWithPlayFab();
				// 	//GameLoader.instance.ShowErrorWindow();
				// }
				// else
				// {
				// 	// load from local
				// 	CheckAndLoadFromLocalStorage();
				// }	
			}
			else
			{
				Wenzil.Console.Console.Log ("Authentication failed Something went wrong.. Connect to Internet");
				// Restart the level..
				 
				//  
			}
	});
	}

	private void LoginWithPlayFab()
	{
		var request = new LoginWithCustomIDRequest
				{
					TitleId = "8D07",
					CreateAccount = true,
					CustomId = player.playerID 
				};
                    player.customID = player.playerName+ player.playerID ;
					PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
	}

	private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, Logining in...!");
		player.playFabID = result.PlayFabId;
		Wenzil.Console.Console.Log("Got PlayFabID: " + player.playFabID);
		if(result.NewlyCreated)
			{
				Wenzil.Console.Console.Log("(new account)");
				//SetPlayerData();
				GetPlayerDataJsonFromServer();
			}
			else
			{
				Wenzil.Console.Console.Log("(existing account)");
				//SetPlayerData();
				GetPlayerDataJsonFromServer();
			}
			Debug.Log(PlayFabClientAPI.IsClientLoggedIn());
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with Login ");
		Wenzil.Console.Console.Log("Error logging in player with custom ID:");
		Wenzil.Console.Console.Log(error.ErrorMessage);
		Wenzil.Console.Console.Log("Cant login Checking for local storage");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

	void CheckAndLoadFromLocalStorage()
	{
		playerJson = GetPlayerDataJsonFromLocal();
		Wenzil.Console.Console.Log("Player data Json" + playerJson);
		if(String.IsNullOrEmpty(playerJson))
		{
			Wenzil.Console.Console.Log("player json is empty so contacting server");
			 
			GetPlayerDataJsonFromServer();// loading data from server
		}
		else
		{
			 player = JsonUtility.FromJson<Player>(playerJson);
			 PuppyManager.Instance.GetPuppyDataJsonFromLocal();  //load puppy data from local
			  
		}
	}

	public void SetPlayerDataJsonToLocal()
	{
		 
		PlayerPrefs.SetString("PlayerData",playerJson);
		Wenzil.Console.Console.Log("PlayerData saved locally");
		Debug.Log("Player Data saved locally");
	 
	}
	public string GetPlayerDataJsonFromLocal()
	{
		return PlayerPrefs.GetString("PlayerData");		 
	}
   
	void RetrieveValuesFromServer()
	{

	}

	public void SetPlayerJsonDataToServer()
	{ 	 
		 
		playerJson = JsonUtility.ToJson(player); 
		UpdateUserDataRequest request = new UpdateUserDataRequest()
		{
			Data = new Dictionary<string, string>()
			{
				{"PlayerData", playerJson},
			}
		};

		PlayFabClientAPI.UpdateUserData(request, (result) =>
		{
			Wenzil.Console.Console.Log("Successfully updated user data");
			
		}, (error) =>
		{
			Wenzil.Console.Console.Log("Cant update UserData");
			Wenzil.Console.Console.Log(""+error.ErrorDetails);
		});
	}

	 
	 
	public void GetPlayerDataJsonFromServer()
	{
		GetUserDataRequest request = new GetUserDataRequest()
		{
			PlayFabId = player.playFabID,
			Keys = null
		};

		PlayFabClientAPI.GetUserData(request,(result) => 
		{
			Wenzil.Console.Console.Log("Getting user data:");
			if ((result.Data == null) || (result.Data.Count == 0))
			{
				Wenzil.Console.Console.Log("No user data available");
			}
			else
			{
				foreach (var item in result.Data)
				{
					Wenzil.Console.Console.Log("    " + item.Key + " == " + item.Value.Value);
					if(item.Key.Contains("PlayerData"))
					{
						playerJson = item.Value.Value;
					}
				}
				 //UpdateJson();
				 GetCurrency();
			}
		}, (error) => {
			Wenzil.Console.Console.Log("Got error retrieving user data:");
			Wenzil.Console.Console.Log(error.ErrorMessage);
		});
	}

	public void AddGameCurrency(int amount)
	{  
		AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest()
			{
				VirtualCurrency = "PC",
				Amount =amount
			};
		 
		PlayFabClientAPI.AddUserVirtualCurrency(request, (result) =>
		{
			Wenzil.Console.Console.Log("Adding Game Currency");
			player.puppyCoins = VirtualCurrency["PC"];
			GlobalVariables.TotalCoins = player.puppyCoins;
			
		}, (error) =>
		{
			Wenzil.Console.Console.Log("Cant Retrieve Inventory");
			Wenzil.Console.Console.Log(""+error.ErrorDetails);
		});
		 
	}
	void GetCurrency()
	{	 
		GetUserInventoryRequest request = new GetUserInventoryRequest();
	 
		PlayFabClientAPI.GetUserInventory(request, (result) =>
		{
			Wenzil.Console.Console.Log("Retrieved User Inventory");
			VirtualCurrency = result.VirtualCurrency;
			player.puppyCoins = VirtualCurrency["PC"];
			GlobalVariables.TotalCoins = player.puppyCoins;
			UpdateJson();
			Debug.Log("Currency retrieved and save to Json");
			
		}, (error) =>
		{
			Wenzil.Console.Console.Log("Cant Retrieve Inventory");
			Wenzil.Console.Console.Log(""+error.ErrorDetails);
		});
	}

 
	public void UpdateJson()
	{
		 JsonUtility.FromJson<Player>(playerJson);
	}

  

	 
	 

	
}
