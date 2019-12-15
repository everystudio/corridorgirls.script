using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RouletteItemAction {

	[ActionCategory("RouletteItemAction")]
	[HutongGames.PlayMaker.Tooltip("RouletteItemAction")]
	public class RouletteItemActionBase : FsmStateAction
	{
		protected RouletteItem roulette;
		public override void OnEnter()
		{
			base.OnEnter();
			roulette = Owner.GetComponent<RouletteItem>();
		}
	}

	[ActionCategory("RouletteItemAction")]
	[HutongGames.PlayMaker.Tooltip("RouletteItemAction")]
	public class startup : RouletteItemActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}
	}


	[ActionCategory("RouletteItemAction")]
	[HutongGames.PlayMaker.Tooltip("RouletteItemAction")]
	public class spin : RouletteItemActionBase
	{
		int select_index = 0;
		float time = 0.0f;
		public FsmFloat interval;

		public override void OnEnter()
		{
			base.OnEnter();
			time = 0.0f;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			time += Time.deltaTime;
			if( interval.Value < time)
			{
				time -= interval.Value;
				roulette.ChangeActive();
			}
		}

	}



}
