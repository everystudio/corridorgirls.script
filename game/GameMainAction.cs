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
			gameMain.CardSetup(DataManager.Instance.dataCard.list);

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

			StartCoroutine(gameMain.chara_control.RequestMove(selected_card.data_card.power, () =>
			{
				Finish();
			}));

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



}