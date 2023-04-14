namespace DefinedEnums
{
    public static class DefinedGameParam
    {
        public const int _recoveryTime = 4;
    }

#region character
    public enum eCharacterAnimState
    {
        IDLE = 0,
        RUN,
        ATTACK,
        BATTLE_IDLE,
        HIT,
        DEAD = 99
    }

    public enum eDirectional
    {
        Left        = 0,
        Right
    }

    public enum eCharIconKind
    {
        Knight          = 0,
        Bandit1,
        bandit2
    }

    #endregion

#region manager
    public enum eGameState
    {
        none = 0,
        Init,
        Ready,
        Start,
        Play,
        Result,
        End
    }

    #endregion

    #region UI

    public enum eMiniMessageBoxType
    {
        Small         = 0,
        Big
    }

    public enum eMessageBoxKind
    {
        Normal          = 0,
        Timer,
        HorizAct,
        VertzAct
    }


    #endregion
}
