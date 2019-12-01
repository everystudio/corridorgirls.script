using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

namespace GameMainAction
{
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class IdleWalk : FsmStateAction
	{
		public FsmInt selected_card_serial;

		public override void OnEnter ()
		{
			base.OnEnter ();

			if (CardHolder.Instance.IsNeedFill ()) {
				Fsm.Event ("fill");
			} else {
				CardHolder.Instance.OnSelectCard.AddListener(SelectedCard);
			}

		}

		public override void OnExit ()
		{
			CardHolder.Instance.OnSelectCard.RemoveListener(SelectedCard);
			base.OnExit ();
		}

		private void SelectedCard(DataCardParam _card){
			selected_card_serial.Value = _card.card_serial;

		}

	}

}