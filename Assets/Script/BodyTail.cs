using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BodyTail : MonoBehaviour
{
    public int _blueBon = 0, _greenBon = 0, _redBon = 0, _whiteBon = 0, _yelowBon = 0;

    BodyPrefsAll _thisPrefs;
    //public int _followers = 0;
    public List<BodyPrefsBonus> _tail = new List<BodyPrefsBonus>();

    void Start()
    {
        _thisPrefs = GetComponent<BodyPrefsAll>();
        SendScoresAndTail();// (_tail.Count);
        //TailTypes();
    }
    // нулевой бонус будет цепляться за _bp._transform;
    public void AddFollower(BodyPrefsBonus bpBonus)
    {
        // _followers++;
        if (_tail.Count == 0) // если первый сбор
        {
        }
        else// if (bpBonus._transform != _tail[0]._transform)
        {
            _tail.Remove(bpBonus);
        }
        _tail.Insert(0, bpBonus);
        bpBonus._target = _thisPrefs;

        //***********
        if (_tail.Count > 1)// если есть второй бонус, то первый бонус перемещаем на тоже место
        {
            _tail[1]._target = _tail[0];
            _tail[0]._follower = _tail[1];
            _tail[0]._transform.position = _tail[1]._transform.position;
        }
        //***********





        //_tail[0]._target = thisPrefs;// GetComponent<BodyPrefsAll>();

        //    int count = 0;//_tail.Count;

        //    for (int i = 0; i < _tail.Count; i++)
        //    {
        //        count = i + 1;
        //        if (!_tail[i]._follower)
        //        {
        //            break;
        //        }
        //        else
        //        {
        //            if (_tail.Count < (i + 1))
        //            {
        //                if (_tail[i]._follower != _tail[i + 1] || _tail[i + 1]._target != _tail[i + 1]._follower)
        //                {
        //                    break;

        //                }
        //            }
        //        }
        //    }

        SendScoresAndTail();//(_tail.Count);
                            // TailTypes();
    }

    public void TailCut(int cut)
    {
        int tailCount = _tail.Count;
        Debug.Log("(0,1,2,3...)  хвост будет подрезан , текущаю длинна= " + tailCount + "   отрезать начиная с элемента№ = " + cut + "   , и того отрежем " + (tailCount - cut));
        _tail.RemoveRange(cut, tailCount - cut);
        Debug.Log("длинна после пореза = " + _tail.Count);
        SendScoresAndTail();//(_tail.Count);
                            // TailTypes();
    }

    //void RecheckTail()// не дает результата. 
    //{
    //    // иду по хвосту от головы до низу и проверяю преслодователя с целью. если расходятся, то откидываю часть
    //    for (int i = 0; i < _tail.Count; i++)
    //    {
    //        if (_tail[i]._follower)
    //        {
    //            if (!_tail[i]._follower._target)
    //            {
    //                _tail.RemoveRange(i, _tail.Count - i);
    //                break;
    //            }
    //        }
    //        else
    //        {
    //            _tail.RemoveRange(i, _tail.Count - i);
    //            break;
    //        }
    //    }
    //    SendTailScores(_tail.Count);
    //}

    void SendScoresAndTail()//(int scores)
    {
        int b = 0, g = 0, r = 0, w = 0, y = 0;
        for (int i = 0; i < _tail.Count; i++)
        {
            switch (_tail[i]._typeColor)
            {
                case GlobalEnum.BonusColor._blue:
                    b++;
                    break;
                case GlobalEnum.BonusColor._green:
                    g++;
                    break;
                case GlobalEnum.BonusColor._red:
                    r++;
                    break;
                case GlobalEnum.BonusColor._white:
                    w++;
                    break;
                case GlobalEnum.BonusColor._yellow:
                    y++;
                    break;
            }
        }
        _blueBon = b;
        _greenBon = g;
        _redBon = r;
        _whiteBon = w;
        _yelowBon = y;

        if (_thisPrefs._controlType == GlobalEnum.ControlType.player)
        {
            // _thisPrefs._gm.SendMessage("ChangeTailScores", scores, SendMessageOptions.DontRequireReceiver);
            List<int> scoresAndTail = new List<int> { _tail.Count, b, g, r, w, y };
            _thisPrefs._gm.SendMessage("ChangeScoresAndTail", scoresAndTail, SendMessageOptions.DontRequireReceiver);



        }
    }

    // void TailTypes()
    //{
    //float b = 0, g = 0, r = 0, w = 0, y = 0;
    //for (int i = 0; i < _tail.Count; i++)
    //{
    //    switch (_tail[i]._typeColor)
    //    {
    //        case GlobalEnum.BonusColor._blue:
    //            b++;
    //            break;
    //        case GlobalEnum.BonusColor._green:
    //            g++;
    //            break;
    //        case GlobalEnum.BonusColor._red:
    //            r++;
    //            break;
    //        case GlobalEnum.BonusColor._white:
    //            w++;
    //            break;
    //        case GlobalEnum.BonusColor._yellow:
    //            y++;
    //            break;
    //    }
    //}
    //_blueBon = b;
    //_greenBon = g;
    //_redBon = r;
    //_whiteBon = w;
    //_yelowBon = y;

    // _thisPrefs.TailTypes(b, g, r, w, y);

    //  _thisPrefs._gm.SendMessage("TailTypes", new List<float> { 1 }, SendMessageOptions.DontRequireReceiver);
    // }

}
