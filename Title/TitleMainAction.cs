using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleMainAction {

	[ActionCategory("TitleMainAction")]
	[HutongGames.PlayMaker.Tooltip("TitleMainAction")]
	public class TitleMainActionBase : FsmStateAction
	{
		protected TitleMain title;
		public override void OnEnter()
		{
			base.OnEnter();
			title = Owner.GetComponent<TitleMain>();
		}
	}

	[ActionCategory("TitleMainAction")]
	[HutongGames.PlayMaker.Tooltip("TitleMainAction")]
	public class startup : TitleMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			title.m_txtVersion.text = string.Format("Ver:{0}", Application.version);

			title.user_data.SetSaveFilename(DataManagerGame.FILENAME_USERDATA);
			title.user_data.Load();

			Finish();
		}
	}

	[ActionCategory("TitleMainAction")]
	[HutongGames.PlayMaker.Tooltip("TitleMainAction")]
	public class check : TitleMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			if( title.user_data.HasKey("is_game") == false)
			{
				Fsm.Event("tutorial");
			}
			else if(title.user_data.ReadInt("is_game") == 0)
			{
				Fsm.Event("camp");
			}
			else if(title.user_data.ReadInt("is_game") != 0)
			{
				Fsm.Event("game");
			}
			else
			{
				// 何がはいる？
			}

		}
	}



}
