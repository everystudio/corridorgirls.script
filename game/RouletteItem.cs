using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RouletteItem : MonoBehaviour {

	public List<BannerRouletteItem> banner_list = new List<BannerRouletteItem>();

	public UnityEventInt OnSelectedItemId = new UnityEventInt();

	public TextMeshProUGUI m_txtMessage;

	public void Initialize(int _iStageId , int _iWave)
	{
		int iItemCount = banner_list.Count;

		List<MasterStageItemParam> appear_item = DataManagerGame.Instance.masterStageItem.list.FindAll(p => p.stage_id == _iStageId && p.wave == _iWave);
		if( appear_item.Count == 0)
		{
			appear_item = DataManagerGame.Instance.masterStageItem.list.FindAll(p => p.stage_id == _iStageId && p.wave == 0);
		}

		int[] item_prob = new int[appear_item.Count];
		for( int i = 0; i < appear_item.Count; i++)
		{
			item_prob[i] = appear_item[i].prob;
		}

		for( int i = 0; i < iItemCount; i++)
		{
			int index = UtilRand.GetIndex(item_prob);
			int item_id = appear_item[index].item_id;
			banner_list[i].Initialize(item_id);
		}
	}

	public void BannerActive( int _iIndex)
	{
		for( int i = 0; i < banner_list.Count; i++)
		{
			banner_list[i].m_animator.SetBool("is_active", i == _iIndex);
		}
	}
	public MasterItemParam GetBannerItem(int _iIndex)
	{
		return banner_list[_iIndex].masterItem;
	}

	public void ChangeActive()
	{
		int index = UtilRand.GetRand(banner_list.Count);
		BannerActive(index);
	}


}
