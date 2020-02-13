using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MessageControlAction
{
	[ActionCategory("MessageControlAction")]
	[HutongGames.PlayMaker.Tooltip("MessageControlAction")]
	public class kusokosss: FsmStateAction
	{
		public FsmString message;
		public override void OnEnter()
		{
			base.OnEnter();
			// こっちはポップアップ
			//MessageControl.Instance.Show(message.Value);
			PanelLogMessage.Instance.AddMessage(message.Value);
		}
	}


}
