using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using System;

namespace GameMainAction
{
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class GameMainActionBase : FsmStateAction
	{
		protected GameMain gameMain;
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain = Owner.GetComponent<GameMain>();
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Startup : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (DataManager.Instance.Initialized)
			{
				Finish();
			}
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CardSetup : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();


			// 初期設定
			DataManager.Instance.dataCard.list.Clear();

			List<MasterCharaCardParam> card_list = DataManager.Instance.masterCharaCard.list.FindAll(p => p.chara_id == 1);

			int serial = 1;
			foreach(MasterCharaCardParam c in card_list)
			{
				DataCardParam dc = new DataCardParam();

				dc.chara_id = c.chara_id;
				dc.card_id = c.card_id;
				dc.card_serial = serial;
				dc.status = (int)DataCard.STATUS.DECK;

				serial += 1;

				DataManager.Instance.dataCard.list.Add(dc);
			}

			DataManager.Instance.dataCard.CardFill(5);

			gameMain.CardSetup(DataManager.Instance.dataCard.list.FindAll(p=>p.status == (int)DataCard.STATUS.HAND));

			gameMain.CardOrder();

			Finish();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CreateDungeon : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			DataManager.Instance.dataCorridor.Create(DataManager.Instance.masterCorridor.list.FindAll(p => p.stage_id == 1));

			foreach (DataCorridorParam param in DataManager.Instance.dataCorridor.list)
			{
				//Debug.Log(string.Format("x={0} y={1}", param.master.x, param.master.y));

				Corridor corr = PrefabManager.Instance.MakeScript<Corridor>(gameMain.m_prefCorridor, gameMain.m_goStageRoot);
				corr.Initialize(param);
				//obj.transform.localPosition = new Vector3(param.master.x, param.master.y, 0.0f);
			}
			gameMain.chara_control.SetCorridor(DataManager.Instance.dataCorridor.list.Find(p => p.index == 1));

			Finish();
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class TurnStart : GameMainActionBase
	{
		public FsmInt card_fill_num;
		public override void OnEnter()
		{
			base.OnEnter();

			int hand_card_num = DataManager.Instance.dataCard.list.FindAll(p => p.status == (int)DataCard.STATUS.HAND).Count;
			Debug.Log(hand_card_num);
			if( hand_card_num <= card_fill_num.Value)
			{
				Fsm.Event("card_fill");
			}
			else
			{
				Finish();
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CardFill : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			List<DataCardParam> add_list = new List<DataCardParam>();
			bool bResult = DataManager.Instance.dataCard.CardFill(5 , ref add_list);

			foreach( DataCardParam card in add_list)
			{
				gameMain.CardAdd(card);
			}
			gameMain.CardOrder();

			if (bResult)
			{
				Finish();
			}
			else
			{
				Fsm.Event("shuffle");
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class DeckShuffle : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			DataManager.Instance.dataCard.DeckShuffle();

			Finish();
		}

	}



	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CardSelect : GameMainActionBase
	{
		public FsmInt select_card_serial;
		public override void OnEnter()
		{
			base.OnEnter();

			select_card_serial.Value = 0;

			foreach (Card card in gameMain.card_list_hand)
			{
				card.OnClickCard.AddListener(OnClickCard);
			}
		}
		private void OnClickCard(int arg0)
		{
			if (select_card_serial.Value == arg0)
			{
				Fsm.Event("select");
			}
			else {
				select_card_serial.Value = arg0;
				gameMain.CardSelectUp(select_card_serial.Value);
			}
		}
		public override void OnExit()
		{
			base.OnExit();
			foreach (Card card in gameMain.card_list_hand)
			{
				card.OnClickCard.RemoveListener(OnClickCard);
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CharaMove : GameMainActionBase
	{
		public FsmInt select_card_serial;

		public override void OnEnter()
		{
			base.OnEnter();
			Card selected_card = gameMain.card_list_hand.Find(p => p.data_card.card_serial == select_card_serial.Value);
			//DataCardParam card = DataManager.Instance.dataCard.list.Find(p => p.card_serial == select_card_serial.Value);

			StartCoroutine(gameMain.chara_control.RequestMove(selected_card.data_card.master.power, () =>
			{
				Finish();
			}));

			selected_card.data_card.status = (int)DataCard.STATUS.REMOVE;
			selected_card.m_animator.SetBool("delete", true);

			gameMain.card_list_hand.Remove(selected_card);
			gameMain.CardOrder();
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CheckCorridorEvent : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			string event_name = "none";
			switch (gameMain.chara_control.target_corridor.corridor_event.type)
			{
				case 1:
					event_name = "gold";
					break;
				case 2:
					event_name = "card";
					break;
				case 3:
					event_name = "item";
					break;
				case 4:
					event_name = "event";
					break;
				case 5:
					event_name = "heal";
					break;
				case 6:
					event_name = "shop";
					break;
				case 7:
					event_name = "battle";
					break;
				case 8:
					event_name = "boss";
					break;

				case 0:
				default:
					event_name = "none";
					break;
			}
			Fsm.Event(event_name);
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CorridorGetGold : GameMainActionBase
	{


	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CorridorHeal : GameMainActionBase
	{


	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class GetItem : GameMainActionBase
	{
		public FsmInt stage_id;
		public FsmInt item_id;
		public override void OnEnter()
		{
			base.OnEnter();

			gameMain.rouletteItem.Initialize(stage_id.Value, 0);
			gameMain.rouletteItem.gameObject.SetActive(true);
			gameMain.rouletteItem.OnSelectedItemId.AddListener(OnSelectedItemId);
		}

		private void OnSelectedItemId(int arg0)
		{
			item_id.Value = arg0;
			MasterItemParam master_item = DataManager.Instance.masterItem.list.Find(p => p.item_id == arg0);
			Debug.Log(master_item.name);
			Finish();
		}

		public override void OnExit()
		{
			base.OnExit();
			gameMain.rouletteItem.OnSelectedItemId.RemoveListener(OnSelectedItemId);
			gameMain.rouletteItem.gameObject.SetActive(false);
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class GetCard : GameMainActionBase
	{
		public FsmInt stage_id;
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain.panelGetCard.gameObject.SetActive(true);
			gameMain.panelGetCard.Initialize(stage_id.Value);
			gameMain.panelGetCard.OnSelectCardParam.AddListener(OnSelectCardParam);
		}

		private void OnSelectCardParam(DataCardParam arg0)
		{
			//Debug.LogWarning(string.Format("card:type={0} power={1}", (MasterCard.CARD_TYPE)arg0.card_type, arg0.power));
			Finish();
		}

		public override void OnExit()
		{
			base.OnExit();
			if (gameMain.panelGetCard != null)
			{
				gameMain.panelGetCard.OnSelectCardParam.RemoveListener(OnSelectCardParam);
				gameMain.panelGetCard.gameObject.SetActive(false);
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Battle : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			gameMain.battleMain.gameObject.SetActive(true);

			gameMain.battleMain.IsBattleFinished = false;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if(gameMain.battleMain.IsBattleFinished)
			{
				Finish();
			}


		}

	}


}