using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour {

	[SerializeField]
	private Image m_imgFrame;
	[SerializeField]
	private TextMeshProUGUI m_txtType;
	[SerializeField]
	private TextMeshProUGUI m_txtPower;
	[SerializeField]
	private Image m_imgTypeIcon;
	public Animator m_animator;

	public GameObject m_goSymbolLine1;
	public GameObject m_goSymbolLine2;
	public List<Image> symbol_list;

	public Image m_imgFaceIcon;

	public Button m_btn;
	public UnityEventInt OnClickCard = new UnityEventInt();

	public DataCardParam data_card;

	public List<MasterCardSymbolParam> card_symbol_list = new List<MasterCardSymbolParam>();

	public Image m_imgStatusFrame;
	public TextMeshProUGUI m_txtStatus;

	public void Initialize(MasterCardParam _master , List<MasterCardSymbolParam> _master_card_symbol_list)
	{
		m_imgStatusFrame.gameObject.SetActive(false);

		if ( data_card == null)
		{
			data_card = new DataCardParam();
			data_card.master = _master;
		}

		if( data_card.chara_id == 0)
		{
			m_imgFaceIcon.gameObject.SetActive(false);
		}
		else
		{
			Sprite spr = SpriteManager.Instance.Get(string.Format(Defines.STR_FORMAT_FACE, data_card.chara_id));
			if (spr != null) {
				m_imgFaceIcon.sprite = spr;
			}
			else
			{
				m_imgFaceIcon.gameObject.SetActive(false);
			}
		}

		card_symbol_list.Clear();

		for (int i = 0; i < symbol_list.Count; i++)
		{
			//Debug.Log(_card.card_id);
			//Debug.Log(_card.master);
			int symbol_id = _master.GetSymbolId(i);

			if (0 < symbol_id)
			{
				MasterCardSymbolParam symbol = _master_card_symbol_list.Find(p => p.card_symbol_id == symbol_id);
				symbol_list[i].gameObject.SetActive(true);
				//Debug.Log(symbol.sprite_name);
				symbol_list[i].sprite = SpriteManager.Instance.Get(symbol.sprite_name);

				card_symbol_list.Add(symbol);
			}
			else
			{
				symbol_list[i].gameObject.SetActive(false);
				if (i == 3)
				{
					m_goSymbolLine2.SetActive(false);
				}
			}
		}
		m_txtType.text = _master.label;
		m_txtPower.text = _master.power.ToString();
	}

	public void Initialize(DataCardParam _card , List<MasterCardSymbolParam> _master_card_symbol_list)
	{
		data_card = _card;
		Initialize(data_card.master , _master_card_symbol_list );
	}


	public void ShowStatus()
	{
		Color color;
		string strMessage = "";
		switch( data_card.status)
		{
			case (int)DataCard.STATUS.PLAY:
				color = new Color(1.0f, 1.0f, 0.0f, 0.5f);
				strMessage = "プレイ";
				break;

			case (int)DataCard.STATUS.HAND:
				color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
				strMessage = "手札";
				break;
			case (int)DataCard.STATUS.REMOVE:
				color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
				strMessage = "捨て";
				break;

			case (int)DataCard.STATUS.DECK:
				color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
				strMessage = "山札";
				break;
			default:
				color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
				strMessage = "";
				break;
		}
		m_imgStatusFrame.gameObject.SetActive(true);
		m_imgStatusFrame.color = color;
		m_txtStatus.text = strMessage;
	}

	void Start()
	{
		m_btn.onClick.AddListener(() =>
		{
			if (data_card != null)
			{
				OnClickCard.Invoke(data_card.card_serial);
			}
		});
	}


	public void Deleted()
	{
		Destroy(gameObject);
	}




}
