using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;


 

public class StoreManager : MonoBehaviour
{
 
	List<StoreItem> recentStoreItems;
	List<ItemInstance> userInventory;

	public Beagle beagle; 

	public string beagleJson;
    public string puppuStoreJson, dressStoreJson;
    // Use this for initialization
    void Start()
    {

    }


	
	public void GetStoreItems( string storeID)
	{
		 GetStoreItemsRequest request = new GetStoreItemsRequest()
		 {
			 CatalogVersion = "PuppyCatalog",
			 StoreId = storeID
		 };

		 PlayFabClientAPI.GetStoreItems(request,(result) => {

			 Wenzil.Console.Console.Log("GetStoreItems" +storeID);
			 if(result.StoreId==null )
			 {
				 Wenzil.Console.Console.Log("Error No store found");
			 }
			 else
			 {
				  Wenzil.Console.Console.Log("    " + result.Request);
 	  
				 foreach( var item in result.Store)
				 {  
				
					 Wenzil.Console.Console.Log("    " + item.ItemId + " == " + item.VirtualCurrencyPrices["PC"]); 
					 
				 }
			 }
		 },(error) => {
			Wenzil.Console.Console.Log("Got error retrieving Store data:");
			Wenzil.Console.Console.Log(error.ErrorMessage);
		});
	 }

	 public void GetUserInventory()
	 {
		 GetUserInventoryRequest request = new GetUserInventoryRequest()
		 {
			 
		 };

		 PlayFabClientAPI.GetUserInventory(request,(result) =>
		 {
			 Debug.Log("User inventory "+ result.Inventory);
			 userInventory =result.Inventory;

			 for(int i =0;i< userInventory.Count;i++)
			 {
				 Dictionary<string,string> inventoryDic = userInventory[i].CustomData;
				
				 foreach(var item in inventoryDic)
				 {
					Debug.Log(item.Key +"==" + item.Value);
				 }
				 Debug.Log("Items are " + userInventory[i].ItemInstanceId);
			 }
		 },(error) =>
		 {
			 Debug.Log("Cant retrieve user inventory");
			 Debug.Log ("Got an error: " + error.ErrorMessage);	
		 });
	 }
	 

	 public void GetPuppyData()
	 {
		 Debug.Log("ID" + PlayerManager.Instance.playerJson);
		// PuppyManager.instance.GetPuppyData(1);
	 }
    // Update is called once per frame
    void Update()
    {

    }
}
