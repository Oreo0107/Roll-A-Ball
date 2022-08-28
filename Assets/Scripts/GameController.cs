using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameType { CP, NoCP }

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameType gameType;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Sets thhe game type from our selections
    public void SetGameType(GameType _gameType)
    {
        gameType = _gameType;
    }

    //toggle between gametypes
    public void ToggleGameType(bool _isCP)
    {
        if (_isCP)
        {
            SetGameType(GameType.NoCP);
            print(gameType);
        }
        else
        {
            SetGameType(GameType.CP);
            print(gameType);
        }
    }

}