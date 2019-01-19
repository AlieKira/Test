using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;

public class BaseRequest:MonoBehaviour
    {
        protected RequestCode requestCode = RequestCode.None;
        protected ActionCode actionCode = ActionCode.none;
        protected GameFacade _gameFacade;
        protected GameFacade gameFacade
        {
            get
            {
                if (_gameFacade==null)
                {
                    _gameFacade = GameFacade.Instance;
                }

                return _gameFacade;
            }
        }

        public virtual void Awake()
        {
            gameFacade.AddRequest(actionCode,this);
        } 
        public virtual void SendRequest(string data)
        {
            gameFacade.SendRequest(requestCode,actionCode,data);
        }

        public virtual void OnResponse(string data)
        {

        }
    }
