using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

 

public class PuppyStoreManager : MonoBehaviour 
{
	
	public string beaglePrice, bullDogPrice,chiPrice,germanPrice;
 	public Dictionary<string,uint> storeItems = new Dictionary<string, uint>(); 
	public string puppyStoreJson;

	
	// Use this for initialization
	void Start () 
	{
		 
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
						storeItems.Add(item.ItemId,item.VirtualCurrencyPrices["PC"]);  
					 }
					puppyStoreJson = PlayFab.Json.JsonWrapper.SerializeObject(storeItems); 
					 
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

	
 
	 
}
