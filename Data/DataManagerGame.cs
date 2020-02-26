using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagerGame : DataManagerBase<DataManagerGame> {

	public static readonly string CONFIG_SS = "18bT3K48n7wOjPwQP-zn1QdUs_JoNKVKZpE6pb-I6PUQ";
	public static readonly string SS_QUEST = "1ls0u6TFbOisVM5nOw6cIP1Y4nK0mDtdgRPaOKmlzGvU";

	public static readonly string SS_ID = "1NVMQClQVzejSE3nE3qsgNA2wMSEdM2l892afiGYj83c";
	public static readonly string SS_TEST = "1bPRYINRf64UOumP6ExJGBb4pHN4qPlM-fUJk7VGAEpg";

	public TextAssetHolder data_holder;

	public CsvKvs gameData = new CsvKvs();
	//public DataKvs dataQuest = new DataKvs();
	public DataCorridor dataCorridor = new DataCorridor();
	public DataCard dataCard = new DataCard();
	public DataUnit dataUnit = new DataUnit();
	public DataItem dataItem = new DataItem();
	public DataSkill dataSkill = new DataSkill();
	public DataStage dataStage = new DataStage();


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

	[SerializeField]
	private UnityEngine.Audio.AudioMixer mixer;

	public bool Initialized = false;

	public override void Initialize()
	{
		base.Initialize();
		StartCoroutine(init_network());
	}

	private bool m_bIsAging;
	public bool IsAging
	{
		get
		{
			return m_bIsAging;
		}
	}

	public bool GetAgingState()
	{
		m_bIsAging = config.HasKey(Defines.KEY_AGING);
		return IsAging;
	}

	public IEnumerator OverrideMasterData()
	{

		yield return StartCoroutine(masterStage.SpreadSheet(SS_ID, "stage", () => { }));
		yield return StartCoroutine(masterCorridor.SpreadSheet(SS_ID, "corridor", () => { }));
		yield return StartCoroutine(masterCorridorEvent.SpreadSheet(SS_ID, "corridor_event", () => { }));

		yield return StartCoroutine(masterCard.SpreadSheet(SS_ID, "card", () => { }));
		yield return StartCoroutine(masterCardSymbol.SpreadSheet(SS_ID, "card_symbol", () => { }));

		yield return StartCoroutine(masterChara.SpreadSheet(SS_ID, "chara", () => { }));
		yield return StartCoroutine(masterCharaCard.SpreadSheet(SS_ID, "chara_card", () => { }));

		yield return StartCoroutine(masterItem.SpreadSheet(SS_ID, "item", () => { }));
		yield return StartCoroutine(masterStageWave.SpreadSheet(SS_ID, "stage_wave", () => { }));
		yield return StartCoroutine(masterStageEvent.SpreadSheet(SS_ID, "stage_event", () => { }));
		yield return StartCoroutine(masterStageItem.SpreadSheet(SS_ID, "stage_item", () => { }));
		yield return StartCoroutine(masterStageCard.SpreadSheet(SS_ID, "stage_card", () => { }));
		yield return StartCoroutine(masterStageMission.SpreadSheet(SS_ID, "stage_mission", () => { }));
		yield return StartCoroutine(masterStageEnemy.SpreadSheet(SS_ID, "stage_enemy", () => { }));
		yield return StartCoroutine(masterStageBattleBonus.SpreadSheet(SS_ID, "stage_bb", () => { }));
		yield return StartCoroutine(masterStageShopItem.SpreadSheet(SS_ID, "stage_shopitem", () => { }));
		
		yield return StartCoroutine(masterSkill.SpreadSheet(SS_ID, "skill", () => { }));
		yield return StartCoroutine(masterSkillEffect.SpreadSheet(SS_ID, "skill_effect", () => { }));

		yield return StartCoroutine(masterBattleBonus.SpreadSheet(SS_ID, "bb", () => { }));

		yield return StartCoroutine(masterMission.SpreadSheet(SS_ID, "mission", () => { }));
		yield return StartCoroutine(masterMissionDetail.SpreadSheet(SS_ID, "mission_detail", () => { }));

	}

	void Start()
	{
		//Debug.Log("start");
		// こっちで間に合うというかこのタイミングじゃないとボリューム調整が効かない
		//mixer.SetFloat("BGM", user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_BGM));
		//mixer.SetFloat("SE", user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_SE));

		// スタートより早くセットすると何者かに上書きされます。死ね
		mixer.SetFloat("BGM", Mathf.Lerp(Defines.SOUND_VOLME_MIN, Defines.SOUND_VOLUME_MAX, user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_BGM)));
		mixer.SetFloat("SE", Mathf.Lerp(Defines.SOUND_VOLME_MIN, Defines.SOUND_VOLUME_MAX, user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_SE)));


		BGMControl.Instance.Play();
	}

	private IEnumerator init_network()
	{
		//Debug.Log(config.ReadInt("stage_id"));
		if (!user_data.HasKey(Defines.KEY_SOUNDVOLUME_BGM))
		{
			Debug.Log("not haskey:Defines.KEY_SOUNDVOLUME_BGM");
			user_data.WriteFloat(Defines.KEY_SOUNDVOLUME_BGM , 0.8f);
		}
		if (!user_data.HasKey(Defines.KEY_SOUNDVOLUME_SE))
		{
			Debug.Log("not haskey:Defines.KEY_SOUNDVOLUME_SE");
			user_data.WriteFloat(Defines.KEY_SOUNDVOLUME_SE, 0.8f);
		}
		//Debug.Log(user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_BGM));
		//Debug.Log(user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_SE));


		gameData.SetSaveFilename(Defines.FILENAME_GAMEDATA);
		if( false == gameData.LoadMulti())
		{
			gameData.AddInt(Defines.KEY_MP, 0);
			gameData.AddInt(Defines.KEY_MP_MAX, 30);
		}
		GetAgingState();

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

		yield return null;

		dataCorridor.SetSaveFilename(Defines.FILENAME_DATA_CORRIDOR);
		dataUnit.SetSaveFilename(Defines.FILENAME_UNIT_GAME);
		dataSkill.SetSaveFilename(Defines.FILENAME_SKILL_GAME);
		dataItem.SetSaveFilename(Defines.FILENAME_ITEM_GAME);
		dataCard.SetSaveFilename(Defines.FILENAME_CARD_GAME);

		dataStage.SetSaveFilename(Defines.FILENAME_DATA_STAGE);

		if (false == dataUnit.LoadMulti())
		{
			dataUnit.MakeInitialData(masterChara.list);
		}
		if (false == dataSkill.LoadMulti())
		{
			//yield return StartCoroutine(dataSkill.SpreadSheet(SS_TEST, "skill", () => { }));
		}
		if (false == dataItem.LoadMulti())
		{
			//yield return StartCoroutine(dataItem.SpreadSheet(SS_TEST, "item", () => { }));
		}
		if( false == dataStage.LoadMulti())
		{
			Debug.LogError("no data stage");
		}
		if( false == dataCard.LoadMulti())
		{

		}
		if( false == dataCorridor.LoadMulti())
		{

		}

		Initialized = true;
	}

	public void SaveTurn()
	{
		gameData.Save();
		dataCorridor.Save();
		dataCard.Save();
		dataUnit.Save();
		dataItem.Save();
		dataSkill.Save();
		dataStage.Save();
		GameMain.Instance.battleMain.dataCardEnemy.Save(Defines.FILENAME_CARD_ENEMY);
	}

private void OnRecievedMasterDungeonStep(List<MasterGimicParam> arg0)
	{
	}

	private void OnRecievedMasterCard(List<MasterCardParam> arg0)
	{
		foreach(MasterCardParam param in arg0)
		{
			//Debug.Log(param.card_id);
		}
	}

	public int GetMp()
	{
		return DataManagerGame.Instance.gameData.ReadInt(Defines.KEY_MP);
	}

	public void MpHeal(int _iHeal)
	{
		int mp_max = DataManagerGame.Instance.gameData.ReadInt(Defines.KEY_MP_MAX);
		int mp_current = DataManagerGame.Instance.gameData.ReadInt(Defines.KEY_MP);

		if (mp_max < mp_current + _iHeal)
		{
			DataManagerGame.Instance.gameData.WriteInt(Defines.KEY_MP, mp_max);
		}
		else {
			DataManagerGame.Instance.gameData.AddInt(Defines.KEY_MP, _iHeal);
		}

	}


}
