using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSimulator : MonoBehaviour {

	public bool start_simulation;

	public List<int> stage_id_list;
	public int chara_level;
	public int simulation_count;


	public int left_chara_id;
	public int right_chara_id;
	public int back_chara_id;

	[HideInInspector]
	public int stage_id_index;
	[HideInInspector]
	public int wave;
	[HideInInspector]
	public int move_delay;

	public DataCorridor dataCorridor = new DataCorridor();
	public DataCard dataCard = new DataCard();
	public DataCard dataCardEnemy = new DataCard();
	public DataUnit dataUnit = new DataUnit();
	public DataItem dataItem = new DataItem();
	public DataSkill dataSkill = new DataSkill();
	public DataStage dataStage = new DataStage();

	public TextAssetHolder data_holder;

	public MasterStage masterStage = new MasterStage();
	public MasterCorridor masterCorridor = new MasterCorridor();
	public MasterCorridorEvent masterCorridorEvent = new MasterCorridorEvent();

	public MasterCard masterCard = new MasterCard();
	public MasterCardSymbol masterCardSymbol = new MasterCardSymbol();

	public MasterChara masterChara = new MasterChara();
	public MasterCharaCard masterCharaCard = new MasterCharaCard();

	public MasterItem masterItem = new MasterItem();
	public MasterStageWave masterStageWave = new MasterStageWave();
	public MasterStageEvent masterStageEvent = new MasterStageEvent();
	public MasterStageItem masterStageItem = new MasterStageItem();
	public MasterStageCard masterStageCard = new MasterStageCard();
	public MasterStageMission masterStageMission = new MasterStageMission();
	public MasterStageEnemy masterStageEnemy = new MasterStageEnemy();
	public MasterStageBattleBonus masterStageBattleBonus = new MasterStageBattleBonus();
	public MasterStageShopItem masterStageShopItem = new MasterStageShopItem();

	public MasterSkill masterSkill = new MasterSkill();
	public MasterSkillEffect masterSkillEffect = new MasterSkillEffect();

	public MasterBattleBonus masterBattleBonus = new MasterBattleBonus();

	public MasterMission masterMission = new MasterMission();
	public MasterMissionDetail masterMissionDetail = new MasterMissionDetail();
	public MasterHelp masterHelp = new MasterHelp();

	public void load_masterdata()
	{
		masterStage.Load(data_holder.Get("master_stage"));
		masterCorridor.Load(data_holder.Get("master_corridor"));
		masterCorridorEvent.Load(data_holder.Get("master_corridor_event"));

		masterCard.Load(data_holder.Get("master_card"));
		masterCardSymbol.Load(data_holder.Get("master_card_symbol"));

		masterChara.Load(data_holder.Get("master_chara"));
		masterCharaCard.Load(data_holder.Get("master_chara_card"));

		masterItem.Load(data_holder.Get("master_item"));
		masterStageWave.Load(data_holder.Get("master_stage_wave"));
		masterStageEvent.Load(data_holder.Get("master_stage_event"));
		masterStageItem.Load(data_holder.Get("master_stage_item"));
		masterStageCard.Load(data_holder.Get("master_stage_card"));
		masterStageMission.Load(data_holder.Get("master_stage_mission"));
		masterStageEnemy.Load(data_holder.Get("master_stage_enemy"));
		masterStageBattleBonus.Load(data_holder.Get("master_stage_bb"));
		masterStageShopItem.Load(data_holder.Get("master_stage_shopitem"));

		masterSkill.Load(data_holder.Get("master_skill"));
		masterSkillEffect.Load(data_holder.Get("master_skill_effect"));

		masterBattleBonus.Load(data_holder.Get("master_bb"));

		masterMission.Load(data_holder.Get("master_mission"));
		masterMissionDetail.Load(data_holder.Get("master_mission_detail"));
		masterHelp.Load(data_holder.Get("master_help"));

	}


	public void create_stage(int _iStage , int _iWave)
	{
		MasterStageParam master_stage = masterStage.list.Find(p => p.stage_id == _iStage);
		dataCorridor.BuildDungeonSim(
			master_stage,
			_iWave,
			masterStageEvent,
			masterStageWave,
			masterCorridorEvent);
	}

	public void party_initialize()
	{
		// 最低レベルは1
		chara_level = Mathf.Max(1, chara_level);

		List<DataUnitParam> party_members = DMCamp.Instance.dataUnitCamp.list.FindAll(p => p.unit == "chara" && p.position != "none");
		dataUnit.list.Clear();

		MasterCharaParam master_left = DMCamp.Instance.masterChara.list.Find(p => p.chara_id == left_chara_id);
		MasterCharaParam master_right = DMCamp.Instance.masterChara.list.Find(p => p.chara_id == right_chara_id);
		MasterCharaParam master_back = DMCamp.Instance.masterChara.list.Find(p => p.chara_id == back_chara_id);

		dataUnit.list.Add(DataUnit.MakeUnit(master_left, chara_level, "left", 60));
		dataUnit.list.Add(DataUnit.MakeUnit(master_right, chara_level, "right", 60));
		dataUnit.list.Add(DataUnit.MakeUnit(master_back, chara_level, "back", 60));
	}

	public void card_initialize()
	{

		// 初期設定
		dataCard.list.Clear();

		int serial = 1;

		List<DataUnitParam> unit_param_list = dataUnit.list.FindAll(p => p.unit == "chara" && (p.position == "left" || p.position == "right" || p.position == "back"));

		foreach (DataUnitParam unit in unit_param_list)
		{
			List<MasterCharaCardParam> card_list = masterCharaCard.list.FindAll(p => p.chara_id == unit.chara_id);
			foreach (MasterCharaCardParam c in card_list)
			{
				DataCardParam dc = new DataCardParam();

				//dc.chara_id = c.chara_id;
				//dc.card_id = c.card_id;
				//dc.card_serial = serial;
				dc.Copy(masterCard.list.Find(p => p.card_id == c.card_id), c.chara_id, serial);

				dc.status = (int)DataCard.STATUS.DECK;
				if (unit.position == "back")
				{
					dc.status = (int)DataCard.STATUS.NOTUSE;
				}
				serial += 1;
				dataCard.list.Add(dc);
			}
		}

		Debug.LogWarning(dataCard.list.Count);

		dataCard.CardFill(5);
	}
}
