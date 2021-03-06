﻿using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PanelMissionAction {

	public class PanelMissionActionBase : FsmStateAction
	{
		protected PanelMission panelMission;
		public override void OnEnter()
		{
			base.OnEnter();
			panelMission = Owner.GetComponent<PanelMission>();
		}
	}


	[ActionCategory("PanelMissionAction")]
	[HutongGames.PlayMaker.Tooltip("PanelMissionAction")]
	public class wait : PanelMissionActionBase
	{
		public FsmInt mission_id;
		public FsmBool debug_skip;
		public FsmInt debug_mission_id;
		public override void OnEnter()
		{
			base.OnEnter();
			panelMission.RequestMissionHandler.AddListener(OnRequestMission);
			panelMission.Close();
		}

		private void OnRequestMission(int arg0)
		{
			mission_id.Value = arg0;

			MasterMissionParam mission = DataManagerGame.Instance.masterMission.list.Find(p => p.mission_id == mission_id.Value);

			string next_event = "";

			switch( mission.type )
			{
				case "yesno":
				case "choice":
					next_event = "yesno";
					break;

				case "item":
					next_event = "item";
					break;
				default:
					Debug.LogError(mission.type);
					break;
			}

			Fsm.Event(next_event);

			
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			if( debug_skip.Value)
			{
				mission_id.Value = debug_mission_id.Value;
				OnRequestMission(mission_id.Value);
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			panelMission.RequestMissionHandler.RemoveListener(OnRequestMission);
		}
	}

	[ActionCategory("PanelMissionAction")]
	[HutongGames.PlayMaker.Tooltip("PanelMissionAction")]
	public class show_intro : PanelMissionActionBase
	{
		public FsmInt mission_id;
		public override void OnEnter()
		{
			base.OnEnter();
			panelMission.set_mission(mission_id.Value);
			panelMission.ShowTwoButton("intro");

			panelMission.m_btnYes.onClick.AddListener(OnYes);
			panelMission.m_btnNo.onClick.AddListener(OnNo);
		}

		private void OnYes()
		{
			Fsm.Event("yes");
		}

		private void OnNo()
		{
			Fsm.Event("no");
		}
		public override void OnExit()
		{
			base.OnExit();
			panelMission.m_btnYes.onClick.RemoveListener(OnYes);
			panelMission.m_btnNo.onClick.RemoveListener(OnNo);
		}
	}

	[ActionCategory("PanelMissionAction")]
	[HutongGames.PlayMaker.Tooltip("PanelMissionAction")]
	public class show_two_button : PanelMissionActionBase
	{
		public FsmInt mission_id;
		public FsmString key;

		private float aging_timer;

		public override void OnEnter()
		{
			base.OnEnter();
			panelMission.set_mission(mission_id.Value);
			panelMission.ShowTwoButton(key.Value);

			panelMission.m_btnYes.onClick.AddListener(OnYes);
			panelMission.m_btnNo.onClick.AddListener(OnNo);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			#region Aging
			if (DataManagerGame.Instance.IsAging)
			{
				aging_timer += Time.deltaTime;
				if (2.5f < aging_timer)
				{
					OnYes();
				}
			}
			#endregion
		}

		private void OnYes()
		{
			Fsm.Event("yes");
		}

		private void OnNo()
		{
			Fsm.Event("no");
		}
		public override void OnExit()
		{
			base.OnExit();
			panelMission.m_btnYes.onClick.RemoveListener(OnYes);
			panelMission.m_btnNo.onClick.RemoveListener(OnNo);
		}
	}


	[ActionCategory("PanelMissionAction")]
	[HutongGames.PlayMaker.Tooltip("PanelMissionAction")]
	public class show_use_check : PanelMissionActionBase
	{
		public FsmInt mission_id;
		public override void OnEnter()
		{
			base.OnEnter();
			panelMission.set_mission(mission_id.Value);

			MasterMissionDetailParam detail = panelMission.masterMissionDetailParamList.Find(p => p.type == "intro_have");

			MasterItemParam item = DataManagerGame.Instance.masterItem.list.Find(p => p.item_id == panelMission.masterMissionParam.item_id);

			string message = string.Format(detail.message, item.name);

			panelMission.ShowMessageTwoButton(message);

			panelMission.m_btnYes.onClick.AddListener(OnYes);
			panelMission.m_btnNo.onClick.AddListener(OnNo);
		}

		private void OnYes()
		{
			Fsm.Event("yes");
		}

		private void OnNo()
		{
			Fsm.Event("no");
		}
		public override void OnExit()
		{
			base.OnExit();
			panelMission.m_btnYes.onClick.RemoveListener(OnYes);
			panelMission.m_btnNo.onClick.RemoveListener(OnNo);
		}
	}

	[ActionCategory("PanelMissionAction")]
	[HutongGames.PlayMaker.Tooltip("PanelMissionAction")]
	public class yesno_check : PanelMissionActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			if( panelMission.masterMissionParam.IsSuccess())
			{
				Fsm.Event("success");
			}
			else
			{
				Fsm.Event("fail");
			}
		}
	}


	[ActionCategory("PanelMissionAction")]
	[HutongGames.PlayMaker.Tooltip("PanelMissionAction")]
	public class item_check : PanelMissionActionBase
	{
		public FsmInt mission_id;

		public override void OnEnter()
		{
			base.OnEnter();
			panelMission.set_mission(mission_id.Value);

			MasterMissionParam mission = DataManagerGame.Instance.masterMission.list.Find(p => p.mission_id == mission_id.Value);

			if( DataManagerGame.Instance.dataItem.HasItem( mission.item_id))
			{
				Fsm.Event("yes");
			}
			else
			{
				Fsm.Event("no");
			}

		}
	}


	[ActionCategory("PanelMissionAction")]
	[HutongGames.PlayMaker.Tooltip("PanelMissionAction")]
	public class show_success : PanelMissionActionBase
	{
		private float aging_timer;
		public override void OnEnter()
		{
			base.OnEnter();
			panelMission.ShowSuccess();
			panelMission.m_btnContinue.onClick.AddListener(OnContinue);
			aging_timer = 0.0f;
		}

		private void OnContinue()
		{
			panelMission.prize_list.Clear();
			foreach ( MasterMissionDetailParam p in panelMission.masterMissionDetailParamList.FindAll(p => p.type == "prize_success"))
			{
				panelMission.prize_list.Add(p);
			}
			Finish();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			#region Aging
			if (DataManagerGame.Instance.IsAging)
			{
				aging_timer += Time.deltaTime;
				if (2.0f < aging_timer)
				{
					OnContinue();
				}
			}
			#endregion
		}

		public override void OnExit()
		{
			base.OnExit();
			panelMission.m_btnContinue.onClick.RemoveListener(OnContinue);
		}
	}
	[ActionCategory("PanelMissionAction")]
	[HutongGames.PlayMaker.Tooltip("PanelMissionAction")]
	public class show_fail : PanelMissionActionBase
	{
		private float aging_timer;
		public override void OnEnter()
		{
			base.OnEnter();
			aging_timer = 0.0f;
			panelMission.ShowFail();
			panelMission.m_btnContinue.onClick.AddListener(OnContinue);
		}

		private void OnContinue()
		{
			//prize.Value = panelMission.masterMissionDetailParamList.Find(p => p.type == "prize_fail").param;
			panelMission.prize_list.Clear();
			foreach (MasterMissionDetailParam p in panelMission.masterMissionDetailParamList.FindAll(p => p.type == "prize_fail"))
			{
				panelMission.prize_list.Add(p);
			}
			Finish();
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			#region Aging
			if (DataManagerGame.Instance.IsAging)
			{
				aging_timer += Time.deltaTime;
				if (2.0f < aging_timer)
				{
					OnContinue();
				}
			}
			#endregion
		}

		public override void OnExit()
		{
			base.OnExit();
			panelMission.m_btnContinue.onClick.RemoveListener(OnContinue);
		}
	}

	[ActionCategory("PanelMissionAction")]
	[HutongGames.PlayMaker.Tooltip("PanelMissionAction")]
	public class next_prize_check : PanelMissionActionBase
	{
		public FsmInt prize_index;
		public FsmString prize_type;
		public FsmString prize_type_sub;
		public FsmInt param;

		public override void OnEnter()
		{
			base.OnEnter();

			if( panelMission.prize_list.Count <= prize_index.Value)
			{
				Finish();
			}
			else
			{
				prize_type.Value = panelMission.prize_list[prize_index.Value].prize_type;
				prize_type_sub.Value = panelMission.prize_list[prize_index.Value].prize_type_sub;
				param.Value = panelMission.prize_list[prize_index.Value].param;
				Fsm.Event("prize");
			}

		}
	}


	[ActionCategory("PanelMissionAction")]
	[HutongGames.PlayMaker.Tooltip("PanelMissionAction")]
	public class result_prize : PanelMissionActionBase
	{
		public FsmString prize_type;
		public FsmString prize_type_sub;
		public FsmInt param;

		public override void OnEnter()
		{
			base.OnEnter();
			Debug.Log(prize_type.Value);
			Debug.Log(prize_type_sub.Value);
			Debug.Log(param.Value);

			bool bIsFinish = false;
			string event_name = "";
			switch(prize_type.Value)
			{
				case "heal":
					if( prize_type_sub.Value == "hp")
					{
						foreach (DataUnitParam u in DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara"))
						{
							u.HpHeal(param.Value);
						}
						GameMain.Instance.battleMain.HpRefresh();
						GameMain.Instance.CharaRefresh();
						bIsFinish = true;
					}
					else if( prize_type_sub.Value == "mp")
					{
						DataManagerGame.Instance.MpHeal(30);
						bIsFinish = true;
						GameMain.Instance.battleMain.HpRefresh();
						GameMain.Instance.CharaRefresh();
					}
					else if( prize_type_sub.Value == "tension")
					{
						foreach (DataUnitParam u in DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara"))
						{
							DataManagerGame.Instance.dataUnit.AddTension(u.chara_id, param.Value, DataManagerGame.Instance.masterChara.list);
						}
						GameMain.Instance.battleMain.HpRefresh();
						GameMain.Instance.CharaRefresh();
						bIsFinish = true;
					}
					break;
				case "buff":
					bIsFinish = true;
					foreach (DataUnitParam u in DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara"))
					{
						DataManagerGame.Instance.dataUnit.AddAssist(u,"mission","ミッション", u.chara_id, prize_type_sub.Value, param.Value, -1);
					}
					break;
				case "damage":
					bIsFinish = true;
					foreach (DataUnitParam u in DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara"))
					{
						u.TrapDamage(param.Value);
					}
					GameMain.Instance.battleMain.HpRefresh();
					GameMain.Instance.CharaRefresh();
					break;
				case "item":
					event_name = "item";
					break;
				case "battle":
					event_name = "battle";
					panelMission.m_goRoot.SetActive(false);
					break;

				case "":
				case "none":
				default:
					bIsFinish = true;
					break;
			}

			if(bIsFinish)
			{
				Finish();
			}
			else
			{
				Fsm.Event(event_name);
			}
		}
		public override void OnExit()
		{
			base.OnExit();
		}
	}

	[ActionCategory("PanelMissionAction")]
	[HutongGames.PlayMaker.Tooltip("PanelMissionAction")]
	public class show_no : PanelMissionActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			panelMission.ShowNo();
			panelMission.m_btnContinue.onClick.AddListener(OnContinue);
		}
		private void OnContinue()
		{
			Finish();
		}
		public override void OnExit()
		{
			base.OnExit();
			panelMission.m_btnContinue.onClick.RemoveListener(OnContinue);
		}
	}
	[ActionCategory("PanelMissionAction")]
	[HutongGames.PlayMaker.Tooltip("PanelMissionAction")]
	public class show_no_item : PanelMissionActionBase
	{
		public FsmInt mission_id;

		private float aging_timer;
		public override void OnEnter()
		{
			base.OnEnter();
			aging_timer = 0.0f;

			MasterMissionDetailParam detail = panelMission.masterMissionDetailParamList.Find(p => p.type == "intro_no");
			MasterItemParam item = DataManagerGame.Instance.masterItem.list.Find(p => p.item_id == panelMission.masterMissionParam.item_id);
			string message = string.Format(detail.message, item.name);

			panelMission.ShowNoItem(message);
			panelMission.m_btnContinue.onClick.AddListener(OnContinue);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			#region Aging
			if (DataManagerGame.Instance.IsAging)
			{
				aging_timer += Time.deltaTime;
				if (2.5f < aging_timer)
				{
					OnContinue();
				}
			}
			#endregion
		}

		private void OnContinue()
		{
			Finish();
		}
		public override void OnExit()
		{
			base.OnExit();
			panelMission.m_btnContinue.onClick.RemoveListener(OnContinue);
		}
	}
	[ActionCategory("PanelMissionAction")]
	[HutongGames.PlayMaker.Tooltip("PanelMissionAction")]
	public class mission_finished : PanelMissionActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			panelMission.OnFinished.Invoke();
			Finish();
		}

	}

}
