using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;


public class ShowCareerTimerRequest:BaseRequest
{
    private GamePanel gamePanel;

    public override void Awake()
    {
        requestCode= RequestCode.Game;
        actionCode = ActionCode.ShowCareerTimer;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        gamePanel.ShowCareerTimeSync(int.Parse(data));
    }
}

