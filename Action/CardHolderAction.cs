using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardHolderAction
{
	abstract public class CardHolderActionBase : FsmStateAction{

		protected CardHolder cardholder;

		public override void OnEnter ()
		{
			base.OnEnter ();
			cardholder = Owner.GetComponent<CardHolder> ();
		}
	}

	[ActionCategory("CardHolderAction")]
	[HutongGames.PlayMaker.Tooltip("CardHolderAction")]
	public class Clear : CardHolderActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			cardholder.Clear(() =>
			{
				Finish();
			});
		}
	}

	[ActionCategory("CardHolderAction")]
	[HutongGames.PlayMaker.Tooltip("CardHolderAction")]
	public class Charge : CardHolderActionBase
	{
		public FsmInt hand_num;

		public override void OnEnter ()
		{
			base.OnEnter ();
			cardholder.Clear(() =>
				{
					Finish();
				});

		}

	}

	[ActionCategory("CardHolderAction")]
	[HutongGames.PlayMaker.Tooltip("CardHolderAction")]
	public class DeckShuffle : CardHolderActionBase
	{

	}

	[ActionCategory("CardHolderAction")]
	[HutongGames.PlayMaker.Tooltip("CardHolderAction")]
	public class Fill : CardHolderActionBase
	{

	}

	[ActionCategory("CardHolderAction")]
	[HutongGames.PlayMaker.Tooltip("CardHolderAction")]
	public class Idle : CardHolderActionBase
	{
		public override void OnEnter ()
		{
			base.OnEnter ();
			cardholder.OnRequestFill.AddListener (request_charge);

			StartCoroutine (delayaction());
		}
		public override void OnExit ()
		{
			cardholder.OnRequestFill.RemoveListener (request_charge);
			base.OnExit ();
		}

		private void request_charge(){
			Debug.Log ("called request_charge");
			Fsm.Event ("charge");
		}
		IEnumerator delayaction(){
			yield return new WaitForSeconds (3);
			cardholder.OnRequestFill.Invoke ();
		}

	}

	[ActionCategory("CardHolderAction")]
	[HutongGames.PlayMaker.Tooltip("CardHolderAction")]
	public class SelectMode : CardHolderActionBase
	{
		public FsmInt card_serial;

		void on_select_card (DataCardParam arg0)
		{
			// 選ばれたカードおシリアル番号を入力
			card_serial.Value = arg0.card_serial;
			Fsm.Event ("select");
		}

		public override void OnEnter ()
		{
			base.OnEnter ();
			cardholder.OnSelectCard.AddListener (on_select_card);
		}
		public override void OnExit ()
		{
			base.OnExit ();
			cardholder.OnSelectCard.RemoveListener (on_select_card);
		}
	}

	[ActionCategory("CardHolderAction")]
	[HutongGames.PlayMaker.Tooltip("CardHolderAction")]
	public class CheckSelectCard : CardHolderActionBase
	{
		// 選択済みのカード
		public FsmInt card_serial;
		public FsmInt other_serial;

		void on_select_card (DataCardParam arg0)
		{
			// 選ばれたカードおシリアル番号を入力
			if (card_serial.Value == arg0.card_serial) {
				Fsm.Event ("decide");
			} else {
				card_serial.Value = arg0.card_serial;
				Fsm.Event ("other");
			}
		}

		public override void OnEnter ()
		{
			base.OnEnter ();
			cardholder.OnSelectCard.AddListener (on_select_card);
		}
		public override void OnExit ()
		{
			base.OnExit ();
			cardholder.OnSelectCard.RemoveListener (on_select_card);
		}
	}



	[ActionCategory("CardHolderAction")]
	[HutongGames.PlayMaker.Tooltip("CardHolderAction")]
	public class SwitchCard : CardHolderActionBase
	{
		// 選択済みのカード
		public FsmInt card_serial;
		public FsmInt other_serial;

		public override void OnEnter ()
		{
			base.OnEnter ();
			int temp = card_serial.Value;
			card_serial.Value = other_serial.Value;
			other_serial.Value = temp;

			Finish ();
		}
		public override void OnExit ()
		{
			base.OnExit ();
		}
	}


}


















