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
		public override void OnEnter()
		{
			base.OnEnter();
		}
	}

	[ActionCategory("SkillMainAction")]
	[HutongGames.PlayMaker.Tooltip("SkillMainAction")]
	public class standby : SkillMainActionBase
	{
		public FsmInt skill_id;
		public FsmString skill_type;

		public override void OnEnter()
		{
			base.OnEnter();
			SkillMain.Instance.master_skill_param = null;
			SkillMain.Instance.SkillRequest.AddListener(OnSkillRequest);
		}

		private void OnSkillRequest(int arg0)
		{
			skill_id.Value = arg0;
			SkillMain.Instance.master_skill_param = DataManager.Instance.masterSkill.list.Find(p => p.skill_id == arg0);

			skill_type.Value = SkillMain.Instance.master_skill_param.skill_type;
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
	public class skill_heal : SkillMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			foreach (DataUnitParam unit in DataManager.Instance.dataUnit.list.FindAll(p => p.unit == "chara")){
				unit.hp += 30;
			}

			BattleMain.Instance.HpRefresh();
			GameMain.Instance.CharaRefresh();

			Finish();
		}
	}

	[ActionCategory("SkillMainAction")]
	[HutongGames.PlayMaker.Tooltip("SkillMainAction")]
	public class skill_add_str : SkillMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			List<DataUnitParam> chara_list = DataManager.Instance.dataUnit.list.FindAll(p => p.unit == "chara");

			foreach (DataUnitParam unit in chara_list)
			{
				DataUnitParam new_assist = new DataUnitParam();

				new_assist.chara_id = unit.chara_id;
				new_assist.unit = "assist";
				new_assist.str = 10;

				DataManager.Instance.dataUnit.list.Add(new_assist);
			}

			BattleMain.Instance.HpRefresh();
			GameMain.Instance.CharaRefresh();

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
