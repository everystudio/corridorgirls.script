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
	public class check_agree : TitleMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			if (title.user_data.HasKey(Defines.KEY_AGREE) == false)
			{
				Fsm.Event("check");
			}
			else
			{
				Finish();
			}
		}
	}

	[ActionCategory("TitleMainAction")]
	[HutongGames.PlayMaker.Tooltip("TitleMainAction")]
	public class agree : TitleMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			title.user_data.Write(Defines.KEY_AGREE, "agree");
			title.user_data.Save();
			Finish();
		}
	}


	[ActionCategory("TitleMainAction")]
	[HutongGames.PlayMaker.Tooltip("TitleMainAction")]
	public class check : TitleMainActionBase
	{
		public FsmString mode_name;
		public override void OnEnter()
		{
			base.OnEnter();

			title.tapScreen.OnTapScreenFinish.AddListener(() =>
			{

				if (title.user_data.HasKey(Defines.KEY_GAMEMODE) == false)
				{
					mode_name.Value = "game_tutorial";
				}
				else
				{
					mode_name.Value = title.user_data.Read(Defines.KEY_GAMEMODE);
				}
				Finish();
			});
			title.tapScreen.m_animator.SetBool("decide" , true);
		}
	}



}
