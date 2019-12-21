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
			if (battleMain.player_card != null)
			{
				GameObject.Destroy(battleMain.player_card.gameObject);
				battleMain.player_card = null;
			}
			if (battleMain.enemy_card != null)
			{
				GameObject.Destroy(battleMain.enemy_card.gameObject);
				battleMain.enemy_card = null;
			}
			battleMain.OnOpeningEnd.AddListener(() =>
			{
				battleMain.OnOpeningEnd.RemoveAllListeners();
				Finish();
			});
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
				Fsm.Event("touch");
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
		public FsmInt enemy_card_id;
		public override void OnEnter()
		{
			base.OnEnter();

			if(battleMain.enemy_card!= null)
			{
				GameObject.Destroy(battleMain.enemy_card.gameObject);
				battleMain.enemy_card = null;
			}

			battleMain.enemy_card = PrefabManager.Instance.MakeScript<Card>(
				battleMain.m_prefCard, battleMain.m_goEnemyCardRoot);

			MasterCardParam master_enemy_card = DataManager.Instance.masterCard.list.Find(p => p.card_id == enemy_card_id.Value);

			battleMain.enemy_card.Initialize(master_enemy_card);

			Finish();
		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class PlayerCard : BattleMainActionBase
	{
		public FsmInt select_card_serial;
		public override void OnEnter()
		{
			base.OnEnter();
			if (battleMain.player_card != null)
			{
				GameObject.Destroy(battleMain.player_card.gameObject);
				battleMain.player_card = null;
			}

			battleMain.player_card = PrefabManager.Instance.MakeScript<Card>(
				battleMain.m_prefCard, battleMain.m_goPlayerCardRoot);

			DataCardParam data_card = DataManager.Instance.dataCard.list.Find(p => p.card_serial == select_card_serial.Value);

			battleMain.player_card.Initialize(data_card);

			Finish();
		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class IconStandby : BattleMainActionBase
	{
		//public FsmInt select_card_serial;
		//public FsmInt enemy_card_id;

		public override void OnEnter()
		{
			base.OnEnter();

			battleMain.player_icon_list.Clear();
			battleMain.enemy_icon_list.Clear();

			for ( int i = 1; i <= 4; i++)
			{
				int iIndex = 0;
				foreach (MasterCardSymbolParam sym in battleMain.player_card.card_symbol_list.FindAll(p=>p.line == i)){
					BattleIcon icon = PrefabManager.Instance.MakeScript<BattleIcon>(battleMain.m_prefBattleIcon, battleMain.m_goBattleChara);
					icon.is_left = true;
					icon.Initialize(sym, iIndex);
					battleMain.player_icon_list.Add(icon);
					iIndex += 1;
				}
				iIndex = 0;
				foreach (MasterCardSymbolParam sym in battleMain.enemy_card.card_symbol_list.FindAll(p => p.line == i))
				{
					BattleIcon icon = PrefabManager.Instance.MakeScript<BattleIcon>(battleMain.m_prefBattleIcon, battleMain.m_goBattleEnemy);
					icon.Initialize(sym, iIndex);
					battleMain.enemy_icon_list.Add(icon);
					iIndex += 1;
				}
			}
		}
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
