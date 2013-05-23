using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {
	public GameObject prefab;
	public ShopItem.Type type;
	
	// Use this for initialization
	void Start () {
		if(type == ShopItem.Type.TYPE_COINS)
			CreateShopCoins();
		else
			CreateShopDiamond();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void CreateShopDiamond(){
		Utils.DestroyAllChild(transform);
		AddShopItem(false,40,  66 , 0 , "diamondPackage1@2x");
		AddShopItem(false,250, 329, 25, "diamondPackage2@2x");
		AddShopItem(false,1350, 1690,35,"diamondPackage3@2x");
		AddShopItem(true, 3000,3290,50, "diamondPackage4@2x");
		/*AddShopItem(false,32500, 799, 30, "coinPackage4@2x");
		AddShopItem(false,70000, 1690,40, "coinPackage5@2x");
		AddShopItem(true, 150000,3290,50, "coinPackage6@2x");*/		
	}
	
	void CreateShopCoins(){
		Utils.DestroyAllChild(transform);
		AddShopItem(false,2000,  66 , 0 , "coinPackage1@2x");
		AddShopItem(false,5500,  169, 10, "coinPackage2@2x");
		AddShopItem(false,12000, 329, 20, "coinPackage3@2x");
		AddShopItem(false,32500, 799, 30, "coinPackage4@2x");
		AddShopItem(false,70000, 1690,40, "coinPackage5@2x");
		AddShopItem(true, 150000,3290,50, "coinPackage6@2x");		
	}
	int pos = 0;
	
	void AddShopItem(bool best_value, int count, int cost, int discont, string icon_name){
		GameObject tempObject = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);
		tempObject.transform.parent = transform;
		tempObject.transform.localPosition = new Vector3(-4,244-pos*110,0);
		pos++;
		tempObject.transform.localScale = new Vector3(1,1,1);
		
		ShopItem shopItem = tempObject.GetComponent<ShopItem>();
		shopItem.type = type;
		shopItem.icon_best_value.gameObject.SetActive(best_value);
		shopItem.shop_icon.spriteName = icon_name;
		shopItem.shop_icon.MakePixelPerfect();
		shopItem.txt_shop_cost.text = cost.ToString();
		shopItem.txt_shop_count.text = count.ToString();
		if(discont == 0)
			shopItem.txt_shop_discont.gameObject.SetActive(false);
		else	
			shopItem.txt_shop_discont.text = "Bonus\n"+discont.ToString();		
	}
}
