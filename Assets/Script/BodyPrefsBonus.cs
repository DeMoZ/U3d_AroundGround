using UnityEngine;
using System.Collections;

public class BodyPrefsBonus : BodyPrefsAll
{
    public GlobalEnum.BonusColor _typeColor = GlobalEnum.BonusColor._white;

    public BodyPrefsAll _target;        // цель за которой летает бонус, если она есть
    public BodyPrefsBonus _follower;      // преследующий бонус, если он есть
    public BodyPrefsAll Target
    {
        set { _target = value; }
        get { return _target; }
    }
    public int _followers;              // число последователей, подается первым последователем
                                        //public int Followers
                                        //{
                                        //    set { _followers = value; }
                                        //    get { return _followers; }
                                        //}
    public override void VirtualStart()
    {
        // SetColorByType(GlobalEnum.BonusColor._green);
        SetColorByType();
    }
    public void SetColorByType()// меняю отображение бонуса по его типу цвета
    {
        if (_core)
        {
            switch (_typeColor)
            {
                case GlobalEnum.BonusColor._blue:
                    _core.startColor = Color.blue;
                    break;
                case GlobalEnum.BonusColor._green:
                    _core.startColor = Color.green;
                    break;
                case GlobalEnum.BonusColor._red:
                    _core.startColor = Color.red;
                    break;
                case GlobalEnum.BonusColor._white:
                    _core.startColor = Color.white;
                    break;
                case GlobalEnum.BonusColor._yellow:
                    _core.startColor = Color.yellow;
                    break;
            }
        }

        //Debug.Log("применен материал на бонусе");
    }
    public void SetColorByType(GlobalEnum.BonusColor type)
    {
        _typeColor = type;

        SetColorByType();// крашу
    }

    public void RemoveFollowersTarget()//(BodyPrefsBonus bpBonus)
    {

        //if (bpBonus._follower)
        //{
        //    bpBonus._follower._target = null;
        //    bpBonus._follower = null;
        //}
        if (_follower)
        {
            _follower._target = null;
            _follower = null;
        }
    }
    public override void RecursionToTailHead(int count)
    {
        if (_target)// надо узнать что это за тип таргета
        {
            if (_target.gameObject.layer == gameObject.layer)       // таргетом является бонус
            {
                _target.RecursionToTailHead(count + 1);
            }
            else                                                // таргетом является обьект с хвостом
            {
                //BodyPrefsPlayer player = _target.GetComponent<BodyPrefsPlayer>();
                //player._followe
                BodyTail bT = _target.GetComponent<BodyTail>();
                bT.TailCut(count);
                //bT._followers = count;
                //bT.
            }
        }
        else // нет таргета, ничего не делаем?
        {
            // можно задать рандомное движение на этом этапе
        }
    }
    public override BodyPrefsBonus GetLeadBonus()
    {// получаем последний бонус из цепочки. Проверки на что у него за таргет, если он есть, будем делать снаружи
        if (_target && _target.gameObject.layer == gameObject.layer) //  цель - тоже бонус
        {
            return _target.GetLeadBonus();
        }
        else // Значит бонус без цели, или цель - чей то хвост
        {
            return this;
        }
    }
    //public void RecursionToTailHead(int count)
    //{
    //    if (_target)
    //    {
    //        if (_target.gameObject.layer == gameObject.layer)       // таргетом является бонус
    //        {
    //            _target.RecursionToTailHead(count + 1);
    //        }
    //        else                                                // таргетом является обьект с хвостом
    //        {

    //        }

    //    }
    //}
}
