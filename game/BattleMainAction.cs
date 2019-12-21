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

			battleMain.Opening();

			Finish();
		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class TurnStart : BattleMainActionBase
	{
		public FsmInt card_fill_num;
		public override void OnEnter()
		{
			base.OnEnter();
			int hand_card_num = DataManager.Instance.dataCard.list.FindAll(p => p.status == (int)DataCard.STATUS.HAND).Count;
			Debug.Log(hand_card_num);
			if (hand_card_num <= card_fill_num.Value)
			{
				Fsm.Event("card_fill");
			}
			else
			{
				Finish();
			}
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


	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class EnemyCard : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class PlayerCard : BattleMainActionBase
	{
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class IconStandby : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class PhysicsStart : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class PhysicsOffset : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class PhysicsPlayerAttack : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class PhysicsEnemyAttack : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class PhysicsEnd : BattleMainActionBase
	{
	}


	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class MagicStart : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class MagicOffset : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class MagicPlayerAttack : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class MagicEnemyAttack : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class MagicEnd : BattleMainActionBase
	{
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class TurnResult : BattleMainActionBase
	{
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class TrunEnd : BattleMainActionBase
	{
	}


}
