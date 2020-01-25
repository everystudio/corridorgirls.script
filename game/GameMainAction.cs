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
	public class DataWait : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain.m_panelCameraScaler.gameObject.SetActive(false);
		}
		public override void OnUpdate()
		{
			base.OnUpdate();

			if (DataManagerGame.Instance.Initialized)
			{
				Finish();
			}
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Startup : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			gameMain.gauge_mp.Setup();

			Finish();
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
			DataManagerGame.Instance.dataCard.list.Clear();

			int serial = 1;

			List<DataUnitParam> unit_param_list = DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara" && (p.position == "left" || p.position == "right"));
			foreach (DataUnitParam unit in unit_param_list)
			{
				List<MasterCharaCardParam> card_list = DataManagerGame.Instance.masterCharaCard.list.FindAll(p => p.chara_id == unit.chara_id);
				foreach (MasterCharaCardParam c in card_list)
				{
					DataCardParam dc = new DataCardParam();

					dc.chara_id = c.chara_id;
					dc.card_id = c.card_id;
					dc.card_serial = serial;
					dc.status = (int)DataCard.STATUS.DECK;
					serial += 1;

					DataManagerGame.Instance.dataCard.list.Add(dc);
				}
			}

			DataManagerGame.Instance.dataCard.CardFill(5);

			gameMain.CardSetup(DataManagerGame.Instance.dataCard.list.FindAll(p=>p.status == (int)DataCard.STATUS.HAND));

			gameMain.CardOrder();



			// スキル関係
			gameMain.ClearSkill();
			gameMain.m_panelStatus.SetupSkill(DataManagerGame.Instance.dataSkill.list.FindAll(p => 0 < p.status), DataManagerGame.Instance.masterSkill.list);



			Finish();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class UnitSetup : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain.panelStatus.Initialize(DataManagerGame.Instance.dataUnit, DataManagerGame.Instance.masterChara);

			Finish();
		}

	}




	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CreateDungeon : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			DataManagerGame.Instance.dataCorridor.Create(DataManagerGame.Instance.masterCorridor.list.FindAll(p => p.stage_id == 1));

			foreach (DataCorridorParam param in DataManagerGame.Instance.dataCorridor.list)
			{
				//Debug.Log(string.Format("x={0} y={1}", param.master.x, param.master.y));

				Corridor corr = PrefabManager.Instance.MakeScript<Corridor>(gameMain.m_prefCorridor, gameMain.m_goStageRoot);
				corr.Initialize(param);
				//obj.transform.localPosition = new Vector3(param.master.x, param.master.y, 0.0f);
			}
			gameMain.chara_control.SetCorridor(DataManagerGame.Instance.dataCorridor.list.Find(p => p.index == 1));

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
			gameMain.m_panelCameraScaler.gameObject.SetActive(true);

			int hand_card_num = DataManagerGame.Instance.dataCard.list.FindAll(p => p.status == (int)DataCard.STATUS.HAND).Count;
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

	[ActionCategory("Common")]
	[HutongGames.PlayMaker.Tooltip("Common")]
	public class CardFill : GameMainActionBase
	{
		public FsmInt fill_num;
		public override void OnEnter()
		{
			base.OnEnter();

			List<DataCardParam> add_list = new List<DataCardParam>();
			bool bResult = DataManagerGame.Instance.dataCard.CardFill(fill_num.Value, ref add_list);

			foreach( DataCardParam card in add_list)
			{
				GameMain.Instance.CardAdd(card);
			}
			GameMain.Instance.CardOrder();

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

	[ActionCategory("Common")]
	[HutongGames.PlayMaker.Tooltip("Common")]
	public class DeckShuffle : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			DataManagerGame.Instance.dataCard.DeckShuffle();

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

				DataCardParam card =  DataManagerGame.Instance.dataCard.list.Find(p => p.card_serial == select_card_serial.Value);

				GameMain.Instance.SelectCharaId = card.chara_id;
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

	[ActionCategory("Common")]
	[HutongGames.PlayMaker.Tooltip("Common")]
	public class SkillSelect : GameMainActionBase
	{
		public FsmInt skill_id;
		public override void OnEnter()
		{
			base.OnEnter();
			//Debug.Log("SkillSelect");
			foreach( BtnSkill btn in GameMain.Instance.m_panelStatus.m_btnSkillList)
			{
				//Debug.Log(btn.gameObject.name);
				btn.OnSkillButton.AddListener(OnSkill);
			}
		}

		private void OnSkill(BtnSkill arg0)
		{
			skill_id.Value = arg0.m_masterSkillParam.skill_id;
			Fsm.Event("skill");
		}

		public override void OnExit()
		{
			base.OnExit();
			if (GameMain.Instance != null)
			{
				foreach (BtnSkill btn in GameMain.Instance.m_panelStatus.m_btnSkillList)
				{
					btn.OnSkillButton.RemoveListener(OnSkill);
				}
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class idle_menu : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain.m_panelGameControlButtons.ShowButtonNum(0, null);

			gameMain.m_panelStatus.m_btnStatus.onClick.AddListener(() =>
			{
				Fsm.Event("status");
			});
			gameMain.m_panelStatus.m_btnItem.onClick.AddListener(() =>
			{
				Fsm.Event("item");
			});
			gameMain.m_panelStatus.m_btnDeck.onClick.AddListener(() =>
			{
				Fsm.Event("deck");
				//m_panelPlayerDeck.Show();
			});
		}

		public override void OnExit()
		{
			base.OnExit();
			gameMain.m_panelStatus.m_btnStatus.onClick.RemoveAllListeners();
			gameMain.m_panelStatus.m_btnItem.onClick.RemoveAllListeners();
			gameMain.m_panelStatus.m_btnDeck.onClick.RemoveAllListeners();
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class ShowDeck : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain.m_panelPlayerDeck.m_btnClose.gameObject.SetActive(false);
			gameMain.m_panelPlayerDeck.Show();

			gameMain.m_panelGameControlButtons.ShowButtonNum(1,new string[1]{ "閉じる"});

			gameMain.m_panelGameControlButtons.OnClickButton.AddListener((int _iIndex) =>
			{
				Finish();
			});
		}
		public override void OnExit()
		{
			base.OnExit();
			gameMain.m_panelPlayerDeck.Close();
			gameMain.m_panelGameControlButtons.OnClickButton.RemoveAllListeners();
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class ShowStatus : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain.m_panelGameStatus.gameObject.SetActive(true);
			gameMain.m_panelGameStatus.Show(DataManagerGame.Instance.dataUnit.list, DataManagerGame.Instance.masterChara.list);

			gameMain.m_panelGameControlButtons.ShowButtonNum(1, new string[1] { "閉じる" });

			gameMain.m_panelGameControlButtons.OnClickButton.AddListener((int _iIndex) =>
			{
				Finish();
			});
		}
		public override void OnExit()
		{
			base.OnExit();
			gameMain.m_panelGameStatus.gameObject.SetActive(false);
			gameMain.m_panelGameControlButtons.OnClickButton.RemoveAllListeners();
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class SkillShow : GameMainActionBase
	{
		public FsmGameObject panel_skill_detail;
		public FsmInt skill_id;
		public FsmString situation;
		public FsmBool skill_used;

		private PanelSkillDetail m_panelSkillDetail;

		public override void OnEnter()
		{
			base.OnEnter();
			GameMain.Instance.CardSelectUp(0);

			m_panelSkillDetail = panel_skill_detail.Value.GetComponent<PanelSkillDetail>();
			m_panelSkillDetail.Initialize(skill_id.Value , situation.Value , skill_used.Value);

			m_panelSkillDetail.m_btnUse.onClick.AddListener(OnSkill);
			m_panelSkillDetail.m_btnCancel.onClick.AddListener(OnCancel);
		}

		private void OnSkill()
		{
			Fsm.Event("skill");
		}

		private void OnCancel()
		{
			Fsm.Event("cancel");
		}

		public override void OnExit()
		{
			base.OnExit();
			m_panelSkillDetail.m_btnCancel.onClick.RemoveListener(OnCancel);
			m_panelSkillDetail.m_btnUse.onClick.RemoveListener(OnSkill);
			if (m_panelSkillDetail != null)
			{
				m_panelSkillDetail.gameObject.SetActive(false);
			}
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class SkillUse : GameMainActionBase
	{
		public FsmInt skill_id;
		public FsmGameObject panel_skill_detail;

		private MasterSkillParam masterSkillParam;
		public override void OnEnter()
		{
			base.OnEnter();
			if (panel_skill_detail != null)
			{
				panel_skill_detail.Value.SetActive(false);
			}

			SkillMain.Instance.SkillFinishHandler.AddListener(OnSkillFinished);
			SkillMain.Instance.SkillRequest.Invoke(skill_id.Value);

			masterSkillParam = DataManagerGame.Instance.masterSkill.list.Find(p => p.skill_id == skill_id.Value);
			DataManagerGame.Instance.dataQuest.AddInt(Defines.KEY_MP, -1 * masterSkillParam.mp);

			Finish();
		}

		private void OnSkillFinished(bool arg0)
		{
			Finish();
		}
		public override void OnExit()
		{
			base.OnExit();
			SkillMain.Instance.SkillFinishHandler.RemoveListener(OnSkillFinished);
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
			//DataCardParam card = DataManagerGame.Instance.dataCard.list.Find(p => p.card_serial == select_card_serial.Value);

			StartCoroutine(gameMain.chara_control.RequestMove(selected_card.data_card.master.power, () =>
			{
				Finish();
			}));

			selected_card.data_card.status = (int)DataCard.STATUS.REMOVE;
			selected_card.m_animator.SetBool("delete", true);

			gameMain.card_list_hand.Remove(selected_card);
			gameMain.CardOrder();
		}

		public override void OnExit()
		{
			base.OnExit();

			gameMain.m_panelCameraScaler.gameObject.SetActive(false);

		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class AddMp : GameMainActionBase
	{
		public FsmInt add_mp;
		public override void OnEnter()
		{
			base.OnEnter();
			DataManagerGame.Instance.dataQuest.AddInt(Defines.KEY_MP, add_mp.Value);
			Finish();
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class AddMp_CardSerial : GameMainActionBase
	{
		public FsmInt card_serial;
		public override void OnEnter()
		{
			base.OnEnter();
			Card selected_card = GameMain.Instance.card_list_hand.Find(p => p.data_card.card_serial == card_serial.Value);
			DataManagerGame.Instance.MpHeal(selected_card.data_card.master.power);
			Finish();
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
			switch ((MasterCorridorEvent.CORRIDOR_EVENT_TYPE)gameMain.chara_control.target_corridor.corridor_event.type)
			{
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.GOLD:
					event_name = "gold";
					break;
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.CARD:
					event_name = "card";
					break;
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.ITEM:
					event_name = "item";
					break;
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.MISSION:
					event_name = "mission";
					break;
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.HEAL:
					event_name = "heal";
					break;
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.SHOP:
					event_name = "shop";
					break;
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.BATTLE:
					event_name = "battle";
					break;
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.BOSS:
					event_name = "boss";
					break;

				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.NONE:
					// ゴール
					if (gameMain.chara_control.target_corridor.corridor_event.sub_type == 2)
					{
						event_name = "goal";
					}
					break;

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
			MasterItemParam master_item = DataManagerGame.Instance.masterItem.list.Find(p => p.item_id == arg0);
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
			gameMain.panelGetCard.stage_id = stage_id.Value;
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
				gameMain.battleMain.BattleClose();
				Finish();
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Mission : GameMainActionBase
	{
		public FsmInt stage_id;
		public override void OnEnter()
		{
			base.OnEnter();
			PanelMission.Instance.OnFinished.AddListener(OnFinished);

			MasterStageMissionParam m = DataManagerGame.Instance.masterStageMission.Select(stage_id.Value);

			PanelMission.Instance.RequestMission(m.mission_id);
		}

		private void OnFinished()
		{
			Finish();
		}
		public override void OnExit()
		{
			base.OnExit();
			if (PanelMission.Instance != null)
			{
				PanelMission.Instance.OnFinished.RemoveListener(OnFinished);
			}

		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class EnterGoal : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain.m_bIsGoal = true;
			Finish();
		}

	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class TurnEnt : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			if (gameMain.m_bIsGoal)
			{
				Fsm.Event("goal");
			}
			else {
				Finish();
			}
		}

	}

}