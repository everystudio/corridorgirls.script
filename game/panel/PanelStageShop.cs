using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelStageShop : MonoBehaviour {

	public List<IconStageShopItem> icon_list;
	public IconStageShopItem.HanderClickedIcon OnClickIcon = new IconStageShopItem.HanderClickedIcon();
	public TextMeshProUGUI m_txtTotalGold;

	public void Initialize( int _iStageId , int _iWave)
	{
		int iItemCount = icon_list.Count;

		List<MasterStageShopItemParam> appear_item = DataManagerGame.Instance.masterStageShopItem.list.FindAll(p => p.stage_id == _iStageId && p.wave == _iWave);
		if (appear_item.Count == 0)
		{
			appear_item = DataManagerGame.Instance.masterStageShopItem.list.FindAll(p => p.stage_id == _iStageId && p.wave == 0);
		}
		if (appear_item.Count == 0)
		{
			appear_item = DataManagerGame.Instance.masterStageShopItem.list.FindAll(p => p.stage_id == 0 && p.wave == 0);
		}

		int[] item_prob = new int[appear_item.Count];
		for (int i = 0; i < appear_item.Count; i++)
		{
			item_prob[i] = appear_item[i].prob;
		}

		for (int i = 0; i < iItemCount; i++)
		{
			int index = UtilRand.GetIndex(item_prob);
			int item_id = appear_item[index].item_id;

			MasterItemParam master = DataManagerGame.Instance.masterItem.list.Find(p => p.item_id == item_id);
			icon_list[i].Initialize(i, master);
			icon_list[i].OnClickIcon.RemoveAllListeners();
			icon_list[i].OnClickIcon.AddListener((IconStageShopItem _icon)=>
			{
				OnClickIcon.Invoke(_icon);
			});
		}
	}



}



