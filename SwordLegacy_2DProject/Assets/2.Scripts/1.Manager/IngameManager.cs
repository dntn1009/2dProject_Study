using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefinedEnums;

public class IngameManager : MonoBehaviour
{
    static IngameManager _uniqueInstance;

    [SerializeField] Sprite[] _charIcons;

    // Constant 변수
    float _readyTime = 3;
    float _startDely = 1;

    //정보 변수
    eGameState _currentGameState;
    float _stepTime = 0;

    //참조 변수
    Transform _playerSpawnPoint;
    HeroObject _player;
    MiniStatusBox _miniStatusBox;
    MessageBox _bigMsgBox;

    public eGameState _nowGameState
    {
        get { return _currentGameState; }
    }

    public static IngameManager _instance
    {
        get { return _uniqueInstance; }
    }

    void Awake()
    {
        _uniqueInstance = this;
        // 임시
        Initgame(1);
        // 
    }
    void Update()
    {
        switch (_currentGameState)
        {
            case eGameState.Ready:
                _stepTime += Time.deltaTime;
                if (_stepTime >= _readyTime)
                    StartGame();
                break;
            case eGameState.Start:
                _stepTime += Time.deltaTime;
                if (_stepTime >= _startDely)
                    PlayGame();
                break;
        }

    }
    public void Initgame(int stageNum)
    {
        _currentGameState = eGameState.Init;
        GameObject prefab, go;
        //UIFrame상 Box 참조
        go = GameObject.FindGameObjectWithTag("UIMiniStatusBox");
        _miniStatusBox = go.GetComponent<MiniStatusBox>();
        go = GameObject.FindGameObjectWithTag("UIBigMessageBox");
        _bigMsgBox = go.GetComponent<MessageBox>();
        //스테이지생성
        string stage = "Stage" + stageNum.ToString();
        prefab = Resources.Load("Prefabs/Stages/" + stage) as GameObject;
        Instantiate(prefab);

        _miniStatusBox.Enables(false);
        _bigMsgBox.CloseMessageBox();
        //플레이어 생성
        go = GameObject.FindGameObjectWithTag("SpawnPlayerP");
        _playerSpawnPoint = go.transform;
        prefab = Resources.Load("Prefabs/Characters/HeroKnight") as GameObject;
        Instantiate(prefab, _playerSpawnPoint.position, _playerSpawnPoint.rotation);
        _player = prefab.GetComponent<HeroObject>();

        //임시
        _player.InitalizeData("IronWater Kim", string.Empty, 10, 4, 6);

        ReadyGame();
    }

    public void ReadyGame()
    {
        _currentGameState = eGameState.Ready;
        Sprite icon = _charIcons[(int)eCharIconKind.Knight];
        _miniStatusBox.Enables(true);
        _miniStatusBox.InitSetData(icon, _player._myName, _player._designName, _player._finalAttackPower, _player._finalDefencePower);
        _bigMsgBox.OpenMessageBox("Ready~", eMiniMessageBoxType.Big);
        _stepTime = 0;
    }
    public void StartGame()
    {
        _currentGameState = eGameState.Start;
        _bigMsgBox.OpenMessageBox("Game Start!!", eMiniMessageBoxType.Big);
        _stepTime = 0;
    }
    public void PlayGame()
    {
        _currentGameState = eGameState.Play;
        _bigMsgBox.CloseMessageBox();
    }
    public void EndGame()
    {
        _currentGameState = eGameState.End;
    }

    public void ResultGame()
    {
        _currentGameState = eGameState.Result;
    }
}
