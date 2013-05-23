using UnityEngine;
using System.Collections;

public class ShopItem : MonoBehaviour {
	public enum Type{
		TYPE_COINS,
		TYPE_DIAMOND
	}
	
	public UISprite icon_best_value;
	public UISprite shop_icon;
	
	public UILabel txt_shop_cost;
	public UILabel txt_shop_count;
	public UILabel txt_shop_discont;
	
	public Type type;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
