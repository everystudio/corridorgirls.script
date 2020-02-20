using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defines{

	#region USER_DATA
	public const string KEY_GAMEMODE = "gamemode";

	public const string KeyGem = "gem";
	public const string KeyCoin = "coin";
	public const string KeyFood = "food";
	public const string KeyMana = "mana";

	public const string KEY_AGING = "aging";
	#endregion


	public const int BUY_FOOD_FROM_GEM = 100;
	public const int BUY_FOOD_COST_GEM = 10;

	public const int BUY_MANA_FROM_GEM = 100;
	public const int BUY_MANA_COST_GEM = 10;

	public const int ADD_GEM_FROM_UNITYADS = 10;

	public const string FILENAME_GAMEDATA = "game_data";
	public const string FILENAME_DATA_STAGE = "data_stage";

	public const string FILENAME_DATA_CORRIDOR = "data_corridor";

	public const string FILENAME_UNIT_CAMP = "unit_camp";
	public const string FILENAME_UNIT_GAME = "unit_game";

	public const string FILENAME_SKILL_CAMP = "skill_camp";
	public const string FILENAME_SKILL_GAME = "skill_game";

	public const string FILENAME_CARD_CAMP = "card_camp";
	public const string FILENAME_CARD_GAME = "card_game";

	public const string FILENAME_ITEM_CAMP = "item_camp";
	public const string FILENAME_ITEM_GAME = "item_game";

	public const string FILENAME_CAMPITEM = "campitem";

	public const float ICON_MOVE_TIME = 0.1f;

	public const string STR_FORMAT_CHARA_TEXTURE = "chara{0:D3}01_mini";
	public const string STR_FORMAT_FACE = "face_{0:D3}01";
	public const string STR_FORMAT_FACEICON = "chara{0:000}01_00_faceicon";

	public const string KEY_MP = "quest_mp";
	public const string KEY_MP_MAX = "quest_mp_max";

	public const int CARD_SYMBOL_ATTACK = 1001;
	public const int CARD_SYMBOL_MAGIC = 3001;
	public const int CARD_SYMBOL_HEAL = 5001;

	public const float SOUND_VOLUME_MAX = 0.0f;
	public const float SOUND_VOLME_MIN = -80.0f;

	public const string BGM_NAME_CAMP = "anime_10_loop";
	public const string BGM_NAME_FIELD = "anime_04_loop";
	public const string BGM_NAME_BATTLE = "boss_battle_01_loop";
	public const string BGM_NAME_BOSS = "battle_03_loop";

	public const string SE_CARDPLAY = "cursor_09";

	public const string KEY_SOUNDVOLUME_BGM = "volume_bgm";
	public const string KEY_SOUNDVOLUME_SE = "volume_se";

	public const string KEY_SOUNDSE_DECIDE = "cursor_01";
	public const string KEY_SOUNDSE_CANCEL = "cancel_01";

	public const string KEY_SOUNDSE_PLUS = "cursor_04";
	public const string KEY_SOUNDSE_MINUS = "cancel_02";

	public const string KEY_SOUNDSE_CASH = "se_cash";


	public static string GetHelpKey( int _iHelpId)
	{
		return string.Format("checked_help_id_{0}", _iHelpId);
	}


}
