using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMCamp : DataManagerBase<DMCamp> {

	public static readonly string SS_ID = "1NVMQClQVzejSE3nE3qsgNA2wMSEdM2l892afiGYj83c";
	public static readonly string SS_TEST = "1bPRYINRf64UOumP6ExJGBb4pHN4qPlM-fUJk7VGAEpg";

	public TextAssetHolder masterTextAssets;

	public CsvKvs gameData = new CsvKvs();

	public DataCard dataCard = new DataCard();
	public DataUnit dataUnitCamp = new DataUnit();
	public DataUnit dataUnitGame = new DataUnit();
	public DataItem dataItem = new DataItem();
	public DataSkill dataSkill = new DataSkill();
	public DataStage dataStage = new DataStage();
	public DataCampItem dataCampItem = new DataCampItem();

	public MasterStage masterStage = new MasterStage();

	public MasterChara masterChara = new MasterChara();
	public MasterCharaCard masterCharaCard = new MasterCharaCard();

	public MasterCard masterCard = new MasterCard();
	public MasterCardSymbol masterCardSymbol = new MasterCardSymbol();

	public MasterItem masterItem = new MasterItem();
	public MasterStageItem masterStageItem = new MasterStageItem();
	public MasterStageCard masterStageCard = new MasterStageCard();

	public MasterSkill masterSkill = new MasterSkill();

	public MasterCampItem masterCampItem = new MasterCampItem();
	public MasterCampShop masterCampShop = new MasterCampShop();
	public MasterLevelup masterLevelup = new MasterLevelup();

	public MasterHelp masterHelp = new MasterHelp();

	[SerializeField]
	private UnityEngine.Audio.AudioMixer mixer;

	[HideInInspector]
	public bool Initialized = false;

	public override void Initialize()
	{
		base.Initialize();
		StartCoroutine(init_network());
	}

	public IEnumerator OverrideMasterData()
	{
		yield return StartCoroutine(masterChara.SpreadSheet(SS_ID, "chara", () => { }));
		yield return StartCoroutine(masterCharaCard.SpreadSheet(SS_ID, "chara_card", () => { }));

		yield return StartCoroutine(masterStage.SpreadSheet(SS_ID, "stage", () => { }));
		yield return StartCoroutine(masterCard.SpreadSheet(SS_ID, "card", () => { }));
		yield return StartCoroutine(masterCardSymbol.SpreadSheet(SS_ID, "card_symbol", () => { }));

		yield return StartCoroutine(masterItem.SpreadSheet(SS_ID, "item", () => { }));
		yield return StartCoroutine(masterStageItem.SpreadSheet(SS_ID, "stage_item", () => { }));
		yield return StartCoroutine(masterStageCard.SpreadSheet(SS_ID, "stage_card", () => { }));

		yield return StartCoroutine(masterSkill.SpreadSheet(SS_ID, "skill", () => { }));
		yield return StartCoroutine(masterCampItem.SpreadSheet(SS_ID, "campitem", () => { }));
		yield return StartCoroutine(masterCampShop.SpreadSheet(SS_ID, "campshop", () => { }));
		yield return StartCoroutine(masterLevelup.SpreadSheet(SS_ID, "levelup", () => { }));
		
		yield return StartCoroutine(masterHelp.SpreadSheet(SS_ID, "help", () => { }));
	}

	void Start()
	{
		Debug.Log("start");
		// こっちで間に合うというかこのタイミングじゃないとボリューム調整が効かない
		//mixer.SetFloat("BGM", user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_BGM));
		//mixer.SetFloat("SE", user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_SE));

		Debug.Log(user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_SE));
		// スタートより早くセットすると何者かに上書きされます。死ね
		mixer.SetFloat("BGM", Mathf.Lerp(Defines.SOUND_VOLME_MIN, Defines.SOUND_VOLUME_MAX, user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_BGM)));
		mixer.SetFloat("SE", Mathf.Lerp(Defines.SOUND_VOLME_MIN, Defines.SOUND_VOLUME_MAX, user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_SE)));


		BGMControl.Instance.Play();
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

	private IEnumerator init_network()
	{
		GetAgingState();

		gameData.SetSaveFilename(Defines.FILENAME_GAMEDATA);
		if( false == gameData.Load())
		{
			// なんか初期化する必要あるなら
		}
		if (!user_data.HasKey(Defines.KEY_SOUNDVOLUME_BGM))
		{
			Debug.Log("not haskey:Defines.KEY_SOUNDVOLUME_BGM");
			user_data.WriteFloat(Defines.KEY_SOUNDVOLUME_BGM, 0.8f);
		}
		if (!user_data.HasKey(Defines.KEY_SOUNDVOLUME_SE))
		{
			Debug.Log("not haskey:Defines.KEY_SOUNDVOLUME_SE");
			user_data.WriteFloat(Defines.KEY_SOUNDVOLUME_SE, 0.8f);
		}

		// master
		masterChara.Load(masterTextAssets.Get("master_chara"));
		masterCharaCard.Load(masterTextAssets.Get("master_chara_card"));

		masterStage.Load(masterTextAssets.Get("master_stage"));
		masterCard.Load(masterTextAssets.Get("master_card"));
		masterCardSymbol.Load(masterTextAssets.Get("master_card_symbol"));

		masterItem.Load(masterTextAssets.Get("master_item"));
		masterStageItem.Load(masterTextAssets.Get("master_stage_item"));
		masterStageCard.Load(masterTextAssets.Get("master_card"));

		masterSkill.Load(masterTextAssets.Get("master_skill"));
		masterCampItem.Load(masterTextAssets.Get("master_campitem"));
		masterCampShop.Load(masterTextAssets.Get("master_campshop"));
		masterLevelup.Load(masterTextAssets.Get("master_levelup"));
		masterHelp.Load(masterTextAssets.Get("master_help"));
		// data
		dataStage.SetSaveFilename(Defines.FILENAME_DATA_STAGE);
		dataUnitCamp.SetSaveFilename(Defines.FILENAME_UNIT_CAMP);
		dataUnitGame.SetSaveFilename(Defines.FILENAME_UNIT_GAME);
		dataSkill.SetSaveFilename(Defines.FILENAME_SKILL_CAMP);
		dataItem.SetSaveFilename(Defines.FILENAME_ITEM_CAMP);
		dataCard.SetSaveFilename(Defines.FILENAME_CARD_CAMP);

		dataCampItem.SetSaveFilename(Defines.FILENAME_ITEM_CAMP);

		if ( false == dataUnitCamp.LoadMulti())
		{
			dataUnitCamp.MakeInitialData(masterChara.list);
			dataUnitCamp.Save();
			//yield return StartCoroutine(dataUnitCamp.SpreadSheet(SS_TEST, "unit", () => { }));
		}
		if( false == dataSkill.LoadMulti())
		{
			dataSkill.MakeInitialData();
			dataSkill.Save();
		}
		//yield return StartCoroutine(dataItem.SpreadSheet(SS_TEST, "item", () => { }));

		if( false == dataCampItem.LoadMulti())
		{
			dataCampItem.list.Clear();
			dataCampItem.Save();
			//yield return StartCoroutine(dataCampItem.SpreadSheet(SS_TEST, "campitem", () => { }));
		}
		if( false == dataItem.LoadMulti())
		{
			dataItem.list.Clear();
			dataItem.Save();
		}

		if( false == dataStage.LoadMulti())
		{
			// まだデータなし
			dataStage.Save();
		}

		if(false == dataCard.LoadMulti())
		{
			dataCard.Reset(dataUnitCamp.list, masterCharaCard.list);
			dataCard.Save();


		}

		yield return null;

		Initialized = true;

	}
}
