using UnityEngine;
public static class GlobalEnum
{
    public static LayerMask _targetLayerAsteroid = 1 << 17;    // layer Asteroid
    public static LayerMask _targetLayerBot = 1 << 18;    // layer Bot
    public static LayerMask _targetLayerBonus = 1 << 19;    // layer Bonus



    public enum BonusColor
    {
        _blue,// ускорение
        _green,// G gun
        _red,
        _white,
        _yellow,
    }

    public enum ControlType
    {
        botSelf,
        botManager,
        player
    }

    public enum PatrolType
    {
        oneDirection,
        randomDirection
    }
}
