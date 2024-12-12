using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvasScript : CanvasScript
{
    [SerializeField]
    protected Button _infoPanelButton;

    [SerializeField]
    protected Text _countText;

    [SerializeField]
    protected Text _percentageText;

    protected GameMBScript _currentGame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameMBScript GetCurrentGame()
    {
        return _currentGame;
    }

    public void SetCurrentGame(GameMBScript _input)
    {
        _currentGame = _input;
    }
}
