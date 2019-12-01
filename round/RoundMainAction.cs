using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

namespace RoundMainAction 
{
	abstract public class RoundMainActionBase : FsmStateAction{

		protected RoundMain roundMain;

		public override void OnEnter ()
		{
			base.OnEnter ();
			roundMain = Owner.GetComponent<RoundMain> ();
		}
	}

	[ActionCategory("RoundMainAction")]
	[HutongGames.PlayMaker.Tooltip("RoundMainAction")]
	public class Sample : RoundMainActionBase
	{
		public override void OnEnter(){
			base.OnEnter();
		}
	}

}
