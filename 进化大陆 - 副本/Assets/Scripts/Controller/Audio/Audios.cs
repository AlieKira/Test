using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Audios
{
    private static Audios _instance;

    public static Audios Instance
    {
        get {
            if (_instance==null)
            {
                _instance=new Audios();
            }

            return _instance;
        }
    }

    public const string Sound_Alert = "Alert";
    public const string Sound_ArrowShoot = "ArrowShoot";
    public const string Sound_Bg_Fast = "Bg(fast)";
    public const string Sound_Bg_Moderate = "Bg(moderate)";
    public const string Sound_ButtonClick = "ButtonClick";
    public const string Sound_Miss = "Miss";
    public const string Sound_ShootPerson = "ShootPerson";
    public const string Sound_Timer = "Timer";
}
