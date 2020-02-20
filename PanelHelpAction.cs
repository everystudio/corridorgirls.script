using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PanelHelpAction {

	public class PanelHelpActionBase : FsmStateAction
	{
		protected PanelHelp panel;
		public override void OnEnter()
		{
			base.OnEnter();
			panel = Owner.GetComponent<PanelHelp>();
		}
	}

	public class HelpGame : FsmStateAction
	{
		public int help_id;
		public override void OnEnter()
		{
			base.OnEnter();
			MasterHelpParam master = DataManagerGame.Instance.masterHelp.list.Find(p => p.help_id == help_id);
			PanelHelp.Instance.btn.onClick.AddListener(Close);
			PanelHelp.Instance.Show(master);
		}

		private void Close()
		{
			Finish();
		}

		public override void OnExit()
		{
			PanelHelp.Instance.btn.onClick.RemoveListener(Close);
			base.OnExit();
		}
	}

	/*
	// 原則キャンプモードで利用
	public class Show : PanelHelpActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}
	}
	*/







}




