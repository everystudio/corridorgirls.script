using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelGameControlButtons : MonoBehaviour {

	public List<Button> button_list;
	public List<TextMeshProUGUI> label_list;

	public UnityEventInt OnClickButton = new UnityEventInt();

	void Start()
	{
		// 小学生のような実装。なんかいい方法あるの？
		button_list[0].onClick.AddListener(() => { OnClickButton.Invoke(0); });
		button_list[1].onClick.AddListener(() => { OnClickButton.Invoke(1); });
		button_list[2].onClick.AddListener(() => { OnClickButton.Invoke(2); });
	}

	public void SetLabel( int _iIndex , string _strLabel)
	{
		label_list[_iIndex].text = _strLabel;
	}
	public void ShowButtonNum(int _iNum, string[] _label_arr , bool[] _bInteractableArr)
	{
		// iNum == 0で非表示。後ろはnullでもいいよ
		bool bRootShow = 0 < _iNum;
		gameObject.SetActive(bRootShow);
		if (bRootShow == false)
		{
			return;
		}

		if (_label_arr.Length != _bInteractableArr.Length)
		{
			Debug.LogError("mismatch array length");
		}

		int iCount = 0;
		foreach (Button btn in button_list)
		{
			bool bShow = iCount < _iNum;
			btn.gameObject.SetActive(bShow);

			if (bShow)
			{
				SetLabel(iCount, _label_arr[iCount]);
				btn.interactable = _bInteractableArr[iCount];
			}
			iCount += 1;
		}
	}

	public void ShowButtonNum( int _iNum , string[] _label_arr )
	{
		bool[] bArr = null;
		if (_label_arr != null)
		{
			bArr = new bool[_label_arr.Length];
			for (int i = 0; i < _label_arr.Length; i++)
			{
				bArr[i] = true;
			}
		}
		ShowButtonNum(_iNum, _label_arr, bArr);
	}



}
