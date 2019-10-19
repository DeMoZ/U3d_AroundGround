using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GMHud : MonoBehaviour
{
    public Text _tailScoresUI;

    public Button _bBoost;
    public Button _bGGun;

    public bool _boolBoost = false;
    public bool _boolGGun = false;

    int _tail = 0;

    public int TailLenght
    {
        set { _tail = value; }
        get { return _tail; }
    }

    //public void ChangeScoresAndTail(int score)
    //{
    //    TailLenght = score;
    //    if (_tailScoresUI)
    //    {
    //        _tailScoresUI.text = _tail.ToString();
    //    }
    //    if (TailLenght >= 5)
    //    {
    //        Debug.Log("набраны очки " + TailLenght);
    //        if (_tailScoresUI)
    //        {
    //            _tailScoresUI.text = TailLenght + " победа";
    //        }
    //    }
    //}
    public void ChangeScoresAndTail(List<int> score)
    {
        TailLenght = score[0];
        if (_tailScoresUI)
        {
            _tailScoresUI.text = _tail.ToString();
        }
        if (TailLenght >= 5)
        {
            Debug.Log("набраны очки " + TailLenght);
            if (_tailScoresUI)
            {
                _tailScoresUI.text = TailLenght + " победа";
            }
        }
        Buttons(score);
    }

    void Buttons(List<int> score)
    {
        ButtonEnabler(score[1], _bBoost, _boolBoost);
        ButtonEnabler(score[2], _bGGun, _boolGGun);
        //ButtonEnabler(score[3], _b, _bool);
        //ButtonEnabler(score[4], _b, _bool);
        //ButtonEnabler(score[5], _b, _bool);
    }
    void ButtonEnabler(int score, Button btn, bool boolBtn)
    {
        if (btn)
        {
            if (boolBtn && score > 0)////_blue,// ускорение
            {
                btn.gameObject.SetActive(true);
            }
            else
            {
                btn.gameObject.SetActive(false);
            }
        }
    }

    public void ResetTailScores()
    {
        if (_tailScoresUI)
        {
            _tailScoresUI.text = "0";
            TailLenght = 0;
        }
    }
    void Start()
    {
        ResetTailScores();
    }


}
