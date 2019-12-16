using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RouletteItemAction {

	[ActionCategory("RouletteItemAction")]
	[HutongGames.PlayMaker.Tooltip("RouletteItemAction")]
	public class RouletteItemActionBase : FsmStateAction
	{
		protected RouletteItem roulette;
		public override void OnEnter()
		{
			base.OnEnter();
			roulette = Owner.GetComponent<RouletteItem>();
		}
	}

	[ActionCategory("RouletteItemAction")]
	[HutongGames.PlayMaker.Tooltip("RouletteItemAction")]
	public class startup : RouletteItemActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}
	}


	[ActionCategory("RouletteItemAction")]
	[HutongGames.PlayMaker.Tooltip("RouletteItemAction")]
	public class spin : RouletteItemActionBase
	{
		int select_index = 0;
		float time = 0.0f;
		public FsmFloat interval;

		public override void OnEnter()
		{
			base.OnEnter();
			time = 0.0f;

			roulette.m_txtMessage.text = "ボタンでアイテム決定";
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			time += Time.deltaTime;
			if( interval.Value < time)
			{
				time -= interval.Value;
				roulette.ChangeActive();
			}
		}
	}

	[ActionCategory("RouletteItemAction")]
	[HutongGames.PlayMaker.Tooltip("RouletteItemAction")]
	public class decide : RouletteItemActionBase
	{
		public FsmInt get_item_id;
		public override void OnEnter()
		{
			base.OnEnter();
			int select_index = UtilRand.GetRand(roulette.banner_list.Count);
			roulette.BannerActive(select_index);
			MasterItemParam master_item = roulette.GetBannerItem(select_index);
			get_item_id.Value = master_item.item_id;
			roulette.m_txtMessage.text = string.Format("<color=#0FF>{0}</color>\nを手に入れました", master_item.name);

			Finish();
		}
	}

	[ActionCategory("RouletteItemAction")]
	[HutongGames.PlayMaker.Tooltip("RouletteItemAction")]
	public class end : RouletteItemActionBase
	{
		public FsmInt get_item_id;
		public override void OnEnter()
		{
			base.OnEnter();
			roulette.OnSelectedItemId.Invoke(get_item_id.Value);
			Finish();
		}
	}




}
