using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;

[System.SerializableAttribute]
public class Beagle
{
	public string puppyName;
	public int puppyLevel;
	public int puppyXP;
	public int energy;  
	public int food;
	public int maxEnergy;
	public int maxFood;
	public int equippedDressItem;
	 

}
public class BullDog
{
	public string puppyName;
	public int puppyLevel;
	public int puppyXP;
	public int energy;  
	public int food;
	public int maxEnergy;
	public int maxFood;
	public int equippedDressItem;
	 

}
public class Chi
{
	public string puppyName;
	public int puppyLevel;
	public int puppyXP;
	public int energy;  
	public int food;
	public int maxEnergy;
	public int maxFood;
	public int equippedDressItem;
	 

}
public class German
{
	public string puppyName;
	public int puppyLevel;
	public int puppyXP;
	public int energy;  
	public int food;
	public int maxEnergy;
	public int maxFood;
	public int equippedDressItem;
	 

}
public class PuppyManager : MonoBehaviour 
{

	public Beagle beagle;
	public BullDog bullDog;
	public Chi chi;
	public German german;
 
	public string beagleJson, bullJson, chiJson, germanJson;

   public static  PuppyManager instance;

	public static PuppyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PuppyManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
	// Use this for initialization
	void Start () 
	{
		
	}

	public void GetPuppyDataJsonToServer(int puppyID)
	{
		Debug.Log("ID" + PlayerManager.Instance.player.playFabID);
		switch(puppyID)
		{
			case 1:
			GetUserDataRequest request = new GetUserDataRequest()
			{
				PlayFabId = PlayerManager.Instance.player.playFabID,
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
						if(item.Key.Contains("BeagleData"))
						{
							beagleJson = item.Value.Value;
						}
						beagle = JsonUtility.FromJson < Beagle >(beagleJson);
					}
				}
			}, (error) => {
				Wenzil.Console.Console.Log("Got error retrieving beagle data:");
				Wenzil.Console.Console.Log(error.ErrorMessage);
			});
			break;

			case 2:

			break;

			case 3:

			break;

			case 4:

			break;
		} 
	}

	public void SetPuppyDataJsonToServer(int puppyID)
	{ 	 

		switch(puppyID)
		{
			case 1:
			beagleJson = JsonUtility.ToJson(beagle);
			UpdateUserDataRequest request = new UpdateUserDataRequest()
			{
				Data = new Dictionary<string, string>()
				{
					{"BeagleData", beagleJson},
				}
			};
			PlayFabClientAPI.UpdateUserData(request, (result) =>
			{
				Wenzil.Console.Console.Log("Successfully updated beagle data");
				 

			}, (error) =>
			{
				Wenzil.Console.Console.Log("Cant update BeagleData");
				Wenzil.Console.Console.Log(""+error.ErrorDetails);
			});

			break;

			case 2:

			break;

			case 3:

			break;

			case 4:

			break;
		} 
		 

		
	}

	public void GetPuppyDataJsonFromLocal()
	{
		Wenzil.Console.Console.Log("Puppy Data loaded from local");
		Debug.Log("Puppy Data Loaded From Local");
		if(PlayerManager.Instance.player.playerCurrentPuppyID!=0)
		{
			if(PlayerManager.instance.player.beagleUnlocked)
			{ 
				beagleJson= PlayerPrefs.GetString("BeagleData");
				 beagle =  JsonUtility.FromJson<Beagle>(beagleJson);
			}
	 		if(PlayerManager.instance.player.bullUnclocked)
			{ 
				bullJson = PlayerPrefs.GetString("BullDogData");
				bullDog =  JsonUtility.FromJson<BullDog>(bullJson);
			}
			

			if(PlayerManager.instance.player.chiUnlocked)
			{ 
				chiJson= PlayerPrefs.GetString("ChihuData");
				 chi =  JsonUtility.FromJson<Chi>(chiJson);
			}
			

			if(PlayerManager.instance.player.germanUnlocked)
			{ 
				germanJson= PlayerPrefs.GetString("GermanData");
				 german =  JsonUtility.FromJson<German>(germanJson);
			}
			//load main menu
			Wenzil.Console.Console.Log("Beagle Json" + beagleJson);
			Wenzil.Console.Console.Log("Bull Json" + bullJson);
			Wenzil.Console.Console.Log("Chi Json" + chiJson);
			Wenzil.Console.Console.Log("German Json" + germanJson);

			Wenzil.Console.Console.Log("Data loaded locally and time to load the scene");
			 
		}
		else
		{
			DialogManager.Instance.ShowErrorWindow();
		}
	}

	public void SetPuppyDataJsonToLocal()
	{
		PlayerPrefs.SetString("BeagleData", beagleJson);
		PlayerPrefs.SetString("BullDogData", bullJson);
		PlayerPrefs.SetString("ChiData", chiJson);
		PlayerPrefs.SetString("GermanData", germanJson);
		Wenzil.Console.Console.Log("PlayerData saved locally");
		Debug.Log("Player Data saved locally");
	}


	
	
 

    
 

}
