using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SkillMainAction {

	[ActionCategory("SkillMainAction")]
	[HutongGames.PlayMaker.Tooltip("SkillMainAction")]
	public class SkillMainActionBase : FsmStateAction
	{
		protected SkillMain skllMain;
		public override void OnEnter()
		{
			base.OnEnter();
			skllMain = Owner.GetComponent<SkillMain>();
		}
	}

	[ActionCategory("SkillMainAction")]
	[HutongGames.PlayMaker.Tooltip("SkillMainAction")]
	public class standby : SkillMainActionBase
	{
		public FsmInt skill_id;

		public override void OnEnter()
		{
			base.OnEnter();
			SkillMain.Instance.master_skill_param = null;
			SkillMain.Instance.SkillRequest.AddListener(OnSkillRequest);

			SkillMain.Instance.move_num = 0;
		}

		private void OnSkillRequest(int arg0)
		{
			skill_id.Value = arg0;
			SkillMain.Instance.master_skill_param = DataManagerGame.Instance.masterSkill.list.Find(p => p.skill_id == arg0);

			Fsm.Event("request");
		}

		public override void OnExit()
		{
			base.OnExit();
			if (SkillMain.Instance != null)
			{
				SkillMain.Instance.SkillRequest.RemoveListener(OnSkillRequest);
			}
		}
	}

	[ActionCategory("SkillMainAction")]
	[HutongGames.PlayMaker.Tooltip("SkillMainAction")]
	public class skill_order : SkillMainActionBase
	{

	}

	[ActionCategory("SkillMainAction")]
	[HutongGames.PlayMaker.Tooltip("SkillMainAction")]
	public class skill_analyze_effect : SkillMainActionBase
	{
		public FsmInt skill_id;
		public FsmInt effect_num;
		public override void OnEnter()
		{
			base.OnEnter();
			SkillMain.Instance.master_skill_effect_param_list = DataManagerGame.Instance.masterSkillEffect.list.FindAll(p => p.skill_id == skill_id.Value);

			effect_num.Value = SkillMain.Instance.master_skill_effect_param_list.Count;
			Debug.Log(skill_id.Value);
			Debug.Log(effect_num.Value);
			Finish();
		}
	}

	[ActionCategory("SkillMainAction")]
	[HutongGames.PlayMaker.Tooltip("SkillMainAction")]
	public class skill_effect_start : SkillMainActionBase
	{
		public FsmInt skill_index;
		public FsmString skill_type;

		public override void OnEnter()
		{
			base.OnEnter();
			skill_type.Value = SkillMain.Instance.master_skill_effect_param_list[skill_index.Value].skill_type;

			Fsm.Event(skill_type.Value);
		}

	}
	[ActionCategory("SkillMainAction")]
	[HutongGames.PlayMaker.Tooltip("SkillMainAction")]
	public class skill_effect_end : SkillMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Finish();
		}
	}


	[ActionCategory("SkillMainAction")]
	[HutongGames.PlayMaker.Tooltip("SkillMainAction")]
	public class skill_heal : SkillMainActionBase
	{
		public FsmInt skill_index;
		private MasterSkillEffectParam effect;
		public override void OnEnter()
		{
			base.OnEnter();
			effect = SkillMain.Instance.master_skill_effect_param_list[skill_index.Value];

			foreach (DataUnitParam unit in DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara")){
				unit.HpHeal(effect.param);
			}
			BattleMain.Instance.HpRefresh();
			GameMain.Instance.CharaRefresh();

			Finish();
		}
	}

	[ActionCategory("SkillMainAction")]
	[HutongGames.PlayMaker.Tooltip("SkillMainAction")]
	public class skill_buff : SkillMainActionBase
	{
		public FsmInt skill_index;
		private MasterSkillEffectParam effect;
		public override void OnEnter()
		{
			base.OnEnter();
			effect = SkillMain.Instance.master_skill_effect_param_list[skill_index.Value];

			List<DataUnitParam> chara_list = DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara");

			foreach (DataUnitParam unit in chara_list)
			{
				DataUnitParam new_assist = new DataUnitParam();

				new_assist.chara_id = unit.chara_id;
				new_assist.unit = "assist";
				switch(effect.field)
				{
					case "hp_max":
						new_assist.hp_max = effect.param;
						break;
					case "str":
						new_assist.str = effect.param;
						break;
					case "magic":
						new_assist.magic = effect.param;
						break;
					case "heal":
						new_assist.heal = effect.param;
						break;
					default:
						break;
				}

				DataManagerGame.Instance.dataUnit.list.Add(new_assist);
			}

			BattleMain.Instance.HpRefresh();
			GameMain.Instance.CharaRefresh();

			Finish();
		}
	}



	[ActionCategory("SkillMainAction")]
	[HutongGames.PlayMaker.Tooltip("SkillMainAction")]
	public class skill_card_fill : SkillMainActionBase
	{
		public FsmInt skill_index;
		public FsmInt card_fill_num;
		private MasterSkillEffectParam effect;
		public override void OnEnter()
		{
			base.OnEnter();
			effect = SkillMain.Instance.master_skill_effect_param_list[skill_index.Value];

			card_fill_num.Value = effect.param;

			Finish();
		}
	}



	[ActionCategory("SkillMainAction")]
	[HutongGames.PlayMaker.Tooltip("SkillMainAction")]
	public class skill_dice : SkillMainActionBase
	{
		public FsmInt skill_index;
		private MasterSkillEffectParam effect;
		public override void OnEnter()
		{
			base.OnEnter();
			effect = SkillMain.Instance.master_skill_effect_param_list[skill_index.Value];
			SkillMain.Instance.move_num += effect.param;
			Finish();
		}
	}


	[ActionCategory("SkillMainAction")]
	[HutongGames.PlayMaker.Tooltip("SkillMainAction")]
	public class SkillFinished : SkillMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			SkillMain.Instance.SkillFinishHandler.Invoke(true);
			Finish();
		}
	}

}
