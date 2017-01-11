using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

 

public class PuppyStoreManager : MonoBehaviour 
{
	
 	public Dictionary<string,uint> puppyStoreItems = new Dictionary<string, uint>(); 
	public string puppyStoreJson;


	[HeaderAttribute ("StoryBoard")]

	public GameObject storyBoardPanel;
	public GameObject[] storyBoards;
	public float waitTime;

	[HeaderAttribute ("PuppySelection")]

	public GameObject puppySelectionObjects; 

	public GameObject puppyBase;
	public GameObject[] puppies;
	public GameObject[] puppySelectionPanel;

	public Text beaglePrice, bullDogPrice,chiPrice, germanPrice;
	

	// Use this for initialization
	void Start () 
	{
		//  storyBoardPanel.SetActive(false);
		
		 puppySelectionObjects.SetActive(false);
		 HideStoryBoardPanel();	
		
		//  ShowStoryBoard();
	}

	public void GetPuppyStoreFromServer()
	{
		
		 GetStoreItemsRequest request = new GetStoreItemsRequest()
		 {
			 CatalogVersion = "PuppyCatalog",
			 StoreId = "PuppyStore"
		 };

		 PlayFabClientAPI.GetStoreItems(request,(result) => {

			 Wenzil.Console.Console.Log("GetStoreItems" + result.StoreId);
			 if(result.StoreId==null )
			 {
				 Wenzil.Console.Console.Log("Error No store found");
			 }
			 else
			 {
				  Wenzil.Console.Console.Log("    " + result.Request);
 	   
					Debug.Log( result.Store.Capacity);
					
					foreach( var item in result.Store)
					{ 
						
						Wenzil.Console.Console.Log("    " + item.ItemId + " == " + item.VirtualCurrencyPrices["PC"]); 
						puppyStoreItems.Add(item.ItemId,item.VirtualCurrencyPrices["PC"]);  
					 }
					puppyStoreJson = PlayFab.Json.JsonWrapper.SerializeObject(puppyStoreItems); 
					SetPuppuStoreDataToLocal();
					ShowStoryBoard();
					// Debug.Log(puppyStoreItems["Beagle"]);
					 
			 }
			 
		 },(error) => {
			Wenzil.Console.Console.Log("Got error retrieving Store data:");
			Wenzil.Console.Console.Log(error.ErrorMessage);
		});
	 }
	

	public string GetPuppyStoreDataFromLocal()
	{

		return PlayerPrefs.GetString("PuppyStore");	
		// storeItems = PlayFab.Json.JsonWrapper.DeserializeObject<Dictionary<string,uint>>(puppyStoreJson);
	}

	public void SetPuppuStoreDataToLocal()
	{
		PlayerPrefs.SetString("PuppyStore",puppyStoreJson);
		Wenzil.Console.Console.Log("PuppyStore saved locally");
		Debug.Log("PuppyStore Data saved locally");
	}

	
	public void ShowStoryBoard()
	{
		
		Debug.Log("calling story board");
		//if(PlayerManager.Instance.player.playerCurrentPuppyID==0)
		{
			StartCoroutine(ShowIntroStoryBoard(waitTime));
		}
		// else
		// {
		// 	// show puppy panel
		// 	ShowPuppySelectionPanel();
		// }
	}
	
	
	IEnumerator ShowIntroStoryBoard(float waitTime)
	{ 
		storyBoardPanel.SetActive(true);
		storyBoards[0].SetActive(true);
		for(int i =0;i< storyBoards.Length;i++)
		{
			yield return new WaitForSeconds(waitTime);
			storyBoards[i].SetActive(true);
		}
		 
	}
	void HideStoryBoardPanel()
	{
		for(int i =0;i<storyBoards.Length ;i++)
			{
				 
				storyBoards[i].SetActive(false);
			} 
	}
	public void ClickToContinue()
	{
		// show puppy panel
		HideStoryBoardPanel();
		ShowPuppySelectionPanel();
	} 


	void ShowPuppySelectionPanel()
	{
		puppySelectionObjects.SetActive(true);
		
		foreach( var item in puppyStoreItems)
		{
			if(item.Key =="Beagle")
			{
				beaglePrice.text = ""+item.Value;
			}
			if(item.Key =="BullDog")
			{
				bullDogPrice.text = ""+item.Value;
			}
			if(item.Key =="Chi")
			{
				chiPrice.text = ""+item.Value;
			}
			if(item.Key =="German")
			{
				germanPrice.text = ""+item.Value;
			}
		}
		
		EnablePuppyPanel(0);
	}

	public void EnablePuppyPanel(int index)
	{
		 
		for(int i=0;i<puppySelectionPanel.Length;i++)
		{
			if(i==index)
			{
				//puppies[i].SetActive(true);
				puppySelectionPanel[i].SetActive(true);
			}
			else
			{
				//puppies[i].SetActive(false);
				puppySelectionPanel[i].SetActive(false);
			}
		}
	}


	public void BuyDog(int puppyID)
	{
		Debug.Log("Purchasing Dog");
		switch(puppyID)
		{
			case 1:
				if(GlobalVariables.TotalCoins>=puppyStoreItems["Beagle"] )
				{
					GlobalVariables.TotalCoins-=(int) puppyStoreItems["Beagle"];
					Debug.Log("Purchasing Beagle Dog");
					// StoryBoardManager.instance.puppyID = puppyID;
					// StoryBoardManager.instance.HideShopPanel();
					// show beagle cutscenes
					// DialogManager.instance.ShowDialog("Dog Purchased! Press OK to Continue",true, false);
				}
				else
				{
					 
					//DialogManager.instance.ShowDialog("Need More Coins! Open Coins Shop?",true,true);
				}
			break;
			case 2:
				if(GlobalVariables.TotalCoins>= puppyStoreItems["BullDog"])
				{	
					GlobalVariables.TotalCoins-=(int) puppyStoreItems["BullDog"];
					Debug.Log("Purchasing Chi Dog");
					// StoryBoardManager.instance.puppyID = puppyID;
					// StoryBoardManager.instance.HideShopPanel();
				}
				else
				{
					//DialogManager.instance.ShowDialog("Need More Coins! Open Coins Shop?",true,true);
				}
			break;
			case 3:
				if(GlobalVariables.TotalCoins>= puppyStoreItems["Chi"])
				{
					GlobalVariables.TotalCoins-=(int) puppyStoreItems["Chi"];
					Debug.Log("Purchasing Bull Dog");
					// StoryBoardManager.instance.puppyID = puppyID;
					// StoryBoardManager.instance.HideShopPanel();
				}
				else
				{
					//DialogManager.instance.ShowDialog("Need More Coins! Open Coins Shop?",true,true);
				}
			break;
			case 4:
				if(GlobalVariables.TotalCoins>= puppyStoreItems["German"])
				{
					GlobalVariables.TotalCoins-=(int) puppyStoreItems["German"];
					Debug.Log("Purchasing German Dog");
					// StoryBoardManager.instance.puppyID = puppyID;
					// StoryBoardManager.instance.HideShopPanel();	 
				}
				else
				{
					//DialogManager.instance.ShowDialog("Need More Coins! Open Coins Shop?",true,true);
				}
			break;

		}
		 
	} 
}
