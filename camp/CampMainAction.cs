using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CampMainAction {
	public class CampMainActionBase : FsmStateAction
	{
		protected CampMain campMain;
		public override void OnEnter()
		{
			base.OnEnter();
			campMain = Owner.GetComponent<CampMain>();
		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class startup : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if(DMCamp.Instance.Initialized)
			{
				Finish();
			}
		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class stage_top : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			campMain.m_panelStage.gameObject.SetActive(true);
			campMain.m_panelStage.ShowList();

			campMain.m_panelStage.m_btnClose.onClick.AddListener(OnClose);
		}

		private void OnClose()
		{
			campMain.m_panelStage.gameObject.SetActive(false);
			Fsm.Event("close");
		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelStage.m_btnClose.onClick.RemoveListener(OnClose);
		}

	}
	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class chara_top : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			campMain.m_panelChara.gameObject.SetActive(true);
			//campMain.m_panelChara.ShowList();

			campMain.m_panelChara.m_btnClose.onClick.AddListener(OnClose);
		}

		private void OnClose()
		{
			campMain.m_panelChara.gameObject.SetActive(false);
			Fsm.Event("close");
		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelChara.m_btnClose.onClick.RemoveListener(OnClose);
		}

	}
	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class skill_top : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			campMain.m_panelSkill.gameObject.SetActive(true);
			//campMain.m_panelStage.ShowList();

			campMain.m_panelSkill.m_btnClose.onClick.AddListener(OnClose);
		}

		private void OnClose()
		{
			campMain.m_panelSkill.gameObject.SetActive(false);
			Fsm.Event("close");
		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelSkill.m_btnClose.onClick.RemoveListener(OnClose);
		}

	}



}
