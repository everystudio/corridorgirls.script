using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleMainAction
{

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class BattleMainActionBase : FsmStateAction
	{
		protected BattleMain battleMain;
		public override void OnEnter()
		{
			base.OnEnter();
			battleMain = Owner.GetComponent<BattleMain>();
		}
	}


	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class opening : BattleMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			battleMain.m_goBattleRoot.SetActive(true);

			battleMain.m_goPanelEnemyInfo.SetActive(true);

			Finish();
		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class card_select_battle : BattleMainActionBase
	{
		public FsmInt select_card_serial;
		public override void OnEnter()
		{
			base.OnEnter();

			select_card_serial.Value = 0;

			foreach (Card card in battleMain.gameMain.card_list_hand)
			{
				card.OnClickCard.AddListener(OnClickCard);
			}
		}
		private void OnClickCard(int arg0)
		{
			if (select_card_serial.Value == arg0)
			{
				Fsm.Event("select");
			}
			else {
				select_card_serial.Value = arg0;
				battleMain.gameMain.CardSelectUp(select_card_serial.Value);
			}
		}
		public override void OnExit()
		{
			base.OnExit();
			foreach (Card card in battleMain.gameMain.card_list_hand)
			{
				card.OnClickCard.RemoveListener(OnClickCard);
			}
		}


	}






}
