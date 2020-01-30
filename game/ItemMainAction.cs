﻿using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemMainAction
{
	public class ItemMainActionBase : FsmStateAction
	{
		protected ItemMain itemMain;
		public override void OnEnter()
		{
			base.OnEnter();
			itemMain = Owner.GetComponent<ItemMain>();
		}
	}


	[ActionCategory("ItemMainAction")]
	[HutongGames.PlayMaker.Tooltip("ItemMainAction")]
	public class startup : ItemMainActionBase
	{
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (DataManagerGame.Instance.Initialized)
			{
				Finish();
			}
		}
	}
	[ActionCategory("ItemMainAction")]
	[HutongGames.PlayMaker.Tooltip("ItemMainAction")]
	public class idle : ItemMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			itemMain.m_panelItemDetail.gameObject.SetActive(false);
			itemMain.m_panelItemList.gameObject.SetActive(false);

			itemMain.OnClose.Invoke();

			itemMain.RequestShow.AddListener(() =>
			{
				Fsm.Event("list");
			});
		}

		public override void OnExit()
		{
			base.OnExit();
			itemMain.RequestShow.RemoveAllListeners();
		}

	}
	[ActionCategory("ItemMainAction")]
	[HutongGames.PlayMaker.Tooltip("ItemMainAction")]
	public class ShowList : ItemMainActionBase
	{
		public FsmInt item_serial;
		public override void OnEnter()
		{
			base.OnEnter();

			itemMain.m_panelItemList.gameObject.SetActive(true);

			itemMain.m_panelItemList.Show(
				DataManagerGame.Instance.dataItem.list.FindAll(p => p.status == (int)DataItem.STATUS.STANDBY),
				DataManagerGame.Instance.masterItem.list
				);
			itemMain.m_panelItemList.OnSelectItem.AddListener((int _iSerial) =>
			{
				item_serial.Value = _iSerial;
				Fsm.Event("select");
			});

			GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(1, new string[1] { "閉じる" });
			GameMain.Instance.m_panelGameControlButtons.OnClickButton.AddListener((int _iIndex) =>
			{
				Fsm.Event("close");
			});
		}
		public override void OnExit()
		{
			base.OnExit();
			itemMain.m_panelItemList.OnSelectItem.RemoveAllListeners();
			itemMain.m_panelItemList.gameObject.SetActive(false);

			GameMain.Instance.m_panelGameControlButtons.OnClickButton.RemoveAllListeners();
		}
	}

	[ActionCategory("ItemMainAction")]
	[HutongGames.PlayMaker.Tooltip("ItemMainAction")]
	public class UseCheck : ItemMainActionBase
	{
		public FsmInt item_serial;
		public FsmInt chara_id;

		public override void OnEnter()
		{
			base.OnEnter();
			DataItemParam data_item = DataManagerGame.Instance.dataItem.list.Find(p => p.serial == item_serial.Value);
			MasterItemParam master_item = DataManagerGame.Instance.masterItem.list.Find(p => p.item_id == data_item.item_id);

			itemMain.m_panelItemDetail.gameObject.SetActive(true);
			itemMain.m_panelItemDetail.Initialize(master_item);

			chara_id.Value = 0;

			itemMain.m_panelItemDetail.m_partyHolder.OnClickIcon.AddListener((CharaIcon icon) =>
			{
				chara_id.Value = icon.m_masterChara.chara_id;

				// あまり良くないけどこっちが呼ばれるってことはキャラ選択が必要だったってわけ
				GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(
					2,
					new string[2] { "使う", "キャンセル" },
					new bool[2] { true, true });

				itemMain.m_panelItemDetail.m_partyHolder.Cover(icon.m_masterChara.chara_id);
			});


			GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(
				2,
				new string[2] { "使う", "キャンセル" },
				new bool[2] { !itemMain.m_panelItemDetail.m_partyHolder.gameObject.activeSelf , true });
			GameMain.Instance.m_panelGameControlButtons.OnClickButton.AddListener((int _iIndex) =>
			{
				if (_iIndex == 0)
				{
					Fsm.Event("use");
				}
				else {
					Fsm.Event("close");
				}
			});
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}

	[ActionCategory("ItemMainAction")]
	[HutongGames.PlayMaker.Tooltip("ItemMainAction")]
	public class UseItem : ItemMainActionBase
	{
		public FsmInt item_serial;
		public FsmInt chara_id;

		public FsmString item_type;
		public FsmString item_type_sub;
		public FsmInt range;
		public FsmInt turn;
		public FsmInt param;
		public FsmInt param2;
		public FsmInt param3;

		public override void OnEnter()
		{
			base.OnEnter();

			DataItemParam data_item = DataManagerGame.Instance.dataItem.list.Find(p => p.serial == item_serial.Value);
			MasterItemParam master_item = DataManagerGame.Instance.masterItem.list.Find(p => p.item_id == data_item.item_id);

			item_type.Value = master_item.item_type;
			item_type_sub.Value = master_item.item_type_sub;
			range.Value = master_item.range;
			turn.Value = master_item.turn;
			param.Value = master_item.param;
			param2.Value = master_item.param2;
			param3.Value = master_item.param3;

			Fsm.Event(master_item.item_type);
		}

	}

	[ActionCategory("ItemMainAction")]
	[HutongGames.PlayMaker.Tooltip("ItemMainAction")]
	public class ItemHeal : ItemMainActionBase
	{
		public FsmInt chara_id;
		public FsmString item_type_sub;
		public FsmInt param;

		public override void OnEnter()
		{
			base.OnEnter();

			if (item_type_sub.Value == "hp")
			{
				if (chara_id.Value == 0)
				{
					foreach (DataUnitParam unit in DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara"))
					{
						unit.HpHeal(param.Value);
					}
				}
				else
				{
					DataUnitParam unit = DataManagerGame.Instance.dataUnit.list.Find(p => p.unit == "chara" && p.chara_id == chara_id.Value);
					unit.HpHeal(param.Value);
				}
				BattleMain.Instance.HpRefresh();
				GameMain.Instance.CharaRefresh();
			}
			else if( item_type_sub.Value == "mp")
			{
				DataManagerGame.Instance.MpHeal(param.Value);
			}
			else
			{
				Debug.LogError(item_type_sub.Value);
			}

			Finish();
		}

	}
	[ActionCategory("ItemMainAction")]
	[HutongGames.PlayMaker.Tooltip("ItemMainAction")]
	public class ItemBuff : ItemMainActionBase
	{
		public FsmInt chara_id;
		public FsmString item_type_sub;
		public FsmInt param;
		public FsmInt turn;

		public override void OnEnter()
		{
			base.OnEnter();

			if( chara_id.Value != 0)
			{
				DataManagerGame.Instance.dataUnit.AddAssist("item", chara_id.Value, item_type_sub.Value, param.Value, turn.Value);
			}
			else
			{
				foreach( DataUnitParam unit in DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara"))
				{
					DataManagerGame.Instance.dataUnit.AddAssist("item", unit.chara_id, item_type_sub.Value, param.Value, turn.Value);
				}
			}
			Finish();
		}
	}

	[ActionCategory("ItemMainAction")]
	[HutongGames.PlayMaker.Tooltip("ItemMainAction")]
	public class ItemCure : ItemMainActionBase
	{
		public FsmInt chara_id;
		public FsmString item_type_sub;
		public FsmInt param;
		public FsmInt turn;
		public override void OnEnter()
		{
			base.OnEnter();
		}
	}

	[ActionCategory("ItemMainAction")]
	[HutongGames.PlayMaker.Tooltip("ItemMainAction")]
	public class ItemDice : ItemMainActionBase
	{
		public FsmInt param;
		public override void OnEnter()
		{
			base.OnEnter();
			itemMain.move = param.Value;
			Finish();
		}
	}
	[ActionCategory("ItemMainAction")]
	[HutongGames.PlayMaker.Tooltip("ItemMainAction")]
	public class ItemKey : ItemMainActionBase
	{
	}

	[ActionCategory("ItemMainAction")]
	[HutongGames.PlayMaker.Tooltip("ItemMainAction")]
	public class ItemRemove : ItemMainActionBase
	{
		public FsmInt item_serial;
		public override void OnEnter()
		{
			base.OnEnter();

			DataItemParam data_item = DataManagerGame.Instance.dataItem.list.Find(p => p.serial == item_serial.Value);
			data_item.status = (int)DataItem.STATUS.REMOVE;

			Finish();
		}

	}




}
