using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
		private float time;

		public override void OnEnter()
		{
			base.OnEnter();
			time = 0.0f;
			battleMain.player_icon_list.Clear();
			battleMain.enemy_icon_list.Clear();

			for ( int i = 1; i <= 4; i++)
			{
				int iIndex = 0;
				foreach (MasterCardSymbolParam sym in battleMain.player_card.card_symbol_list.FindAll(p=>p.line == i)){
					BattleIcon icon = PrefabManager.Instance.MakeScript<BattleIcon>(battleMain.m_prefBattleIcon, battleMain.m_goBattleChara);
					icon.Initialize(sym, iIndex,true);
					battleMain.player_icon_list.Add(icon);
					iIndex += 1;
				}
				iIndex = 0;
				foreach (MasterCardSymbolParam sym in battleMain.enemy_card.card_symbol_list.FindAll(p => p.line == i))
				{
					BattleIcon icon = PrefabManager.Instance.MakeScript<BattleIcon>(battleMain.m_prefBattleIcon, battleMain.m_goBattleEnemy);
					icon.Initialize(sym, iIndex,false);
					battleMain.enemy_icon_list.Add(icon);
					iIndex += 1;
				}
			}
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			time += Time.deltaTime;
			if( 2.0f < time)
			{
				Finish();
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
	public class SymbolOffset : BattleMainActionBase
	{
		public FsmInt symbol_id_player;
		public FsmInt symbol_id_enemy;

		public FsmInt symbol_id_player_canceler;
		public FsmInt symbol_id_enemy_canceler;

		private bool m_bMove;
		private float m_fTime;
		private float move_time;

		public override void OnEnter()
		{
			base.OnEnter();
			move_time = 0.5f;
			m_fTime = 0.0f;
			Debug.Log("SymbolOffset.OnEnter");

			if( symbol_id_player_canceler.Value != 0)
			{
				BattleIcon player_icon_canceler = battleMain.player_icon_list.Find(p => p.master_symbol.card_symbol_id == symbol_id_player_canceler.Value);
				if(player_icon_canceler != null)
				{
					Finish();
					return;
				}
			}
			if (symbol_id_enemy_canceler.Value != 0)
			{
				BattleIcon enemy_icon_canceler = battleMain.enemy_icon_list.Find(p => p.master_symbol.card_symbol_id == symbol_id_enemy_canceler.Value);
				if (enemy_icon_canceler != null)
				{
					Finish();
					return;
				}
			}

			BattleIcon player_icon = battleMain.player_icon_list.Find(p => p.master_symbol.card_symbol_id == symbol_id_player.Value);
			BattleIcon enemy_icon = battleMain.enemy_icon_list.Find(p => p.master_symbol.card_symbol_id == symbol_id_enemy.Value);

			if( player_icon != null && enemy_icon != null)
			{
				m_bMove = true;

				player_icon.m_animator.SetTrigger("break");
				enemy_icon.m_animator.SetTrigger("break");

				List<BattleIcon> player_icon_list = battleMain.player_icon_list.FindAll(p => p.master_symbol.line == player_icon.master_symbol.line && p != player_icon);
				List<BattleIcon> enemy_icon_list = battleMain.enemy_icon_list.FindAll(p => p.master_symbol.line == enemy_icon.master_symbol.line && p != enemy_icon);

				foreach( BattleIcon icon in player_icon_list)
				{
					icon.move(move_time, icon.index - 1, icon.master_symbol.line, icon.is_left);
					icon.index -= 1;
				}
				foreach (BattleIcon icon in enemy_icon_list)
				{
					icon.move(move_time, icon.index - 1, icon.master_symbol.line, icon.is_left);
					icon.index -= 1;
				}

				battleMain.player_icon_list.Remove(player_icon);
				battleMain.enemy_icon_list.Remove(enemy_icon);

			}
			else
			{
				m_bMove = false;
				Finish();
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			// 多分いらないと思うけど
			if (m_bMove)
			{
				m_fTime += Time.deltaTime;
				if(move_time < m_fTime)
				{
					Fsm.Event("continue");
				}
			}
		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class Attack : BattleMainActionBase
	{
		public FsmBool is_player;
		public FsmInt symbol_id;

		private bool m_bNext;
		private float m_fTime;

		private int attack_count = 0;
		private int result_count = 0;

		public override void OnEnter()
		{
			base.OnEnter();

			attack_count = 0;
			result_count = 0;

			StartCoroutine(exe_attack());
			/*
			m_bNext = false;
			m_fTime = 0.0f;

			BattleIcon target_icon = null;
			List<BattleIcon> target_list = null;
			if (is_player.Value)
			{
				target_icon = battleMain.player_icon_list.Find(p => p.master_symbol.card_symbol_id == symbol_id.Value);
				if (target_icon != null)
				{
					target_list = battleMain.player_icon_list.FindAll(p => p.master_symbol.line == target_icon.master_symbol.line && p != target_icon);
				}
			}
			else
			{
				target_icon = battleMain.enemy_icon_list.Find(p => p.master_symbol.card_symbol_id == symbol_id.Value);
				if (target_icon != null)
				{
					target_list = battleMain.enemy_icon_list.FindAll(p => p.master_symbol.line == target_icon.master_symbol.line && p != target_icon);
				}
			}

			if( target_icon != null)
			{
				target_icon.m_animator.SetTrigger("attack");
				if(is_player.Value)
				{
					battleMain.player_icon_list.Remove(target_icon);
				}
				else
				{
					battleMain.enemy_icon_list.Remove(target_icon);
				}
			}
			if (target_list != null && 0 < target_list.Count)
			{
				m_bNext = true;
				foreach (BattleIcon icon in target_list)
				{
					icon.move(Defines.ICON_MOVE_TIME, icon.index - 1, icon.master_symbol.line, icon.is_left);
					icon.index -= 1;
				}
			}
			else
			{
				Finish();
			}
			*/
		}

		private bool act_attack()
		{
			bool bRet = false;
			BattleIcon target_icon = null;
			List<BattleIcon> target_list = null;
			if (is_player.Value)
			{
				target_icon = battleMain.player_icon_list.Find(p => p.master_symbol.card_symbol_id == symbol_id.Value);
				if (target_icon != null)
				{
					target_list = battleMain.player_icon_list.FindAll(p => p.master_symbol.line == target_icon.master_symbol.line && p != target_icon);
				}
			}
			else
			{
				target_icon = battleMain.enemy_icon_list.Find(p => p.master_symbol.card_symbol_id == symbol_id.Value);
				if (target_icon != null)
				{
					target_list = battleMain.enemy_icon_list.FindAll(p => p.master_symbol.line == target_icon.master_symbol.line && p != target_icon);
				}
			}

			if (target_icon != null)
			{
				target_icon.AttackHandler.AddListener(OnAttackIcon);
				target_icon.m_animator.SetTrigger("attack");
				if (is_player.Value)
				{
					battleMain.player_icon_list.Remove(target_icon);
				}
				else
				{
					battleMain.enemy_icon_list.Remove(target_icon);
				}
			}
			if (target_list != null && 0 < target_list.Count)
			{
				m_bNext = true;
				foreach (BattleIcon icon in target_list)
				{
					icon.move(Defines.ICON_MOVE_TIME, icon.index - 1, icon.master_symbol.line, icon.is_left);
					icon.index -= 1;
				}
				bRet = true;
			}
			else
			{
				bRet = false;
			}
			return bRet;
		}

		private void OnAttackIcon(BattleIcon arg0)
		{
			result_count += 1;
			Debug.Log(arg0.gameObject.name);
		}

		private IEnumerator exe_attack()
		{
			while(act_attack())
			{
				attack_count += 1;
				yield return new WaitForSeconds(0.3f);
			}

		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if( attack_count == result_count)
			{
				Finish();
			}
			/*
			if( m_bNext)
			{
				m_fTime += Time.deltaTime;
				if( Defines.ICON_MOVE_TIME < m_fTime)
				{
					Fsm.Event("continue");
				}
			}
			*/
		}
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
