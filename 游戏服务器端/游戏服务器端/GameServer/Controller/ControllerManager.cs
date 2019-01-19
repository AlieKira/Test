using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;

namespace GameServer.Controller
{
    class ControllerManager
    {
        private Server server;
        private Dictionary<RequestCode, BaseController> controllerList = new Dictionary<RequestCode, BaseController>();

        public ControllerManager(Server server)
        {
            this.server = server;
            Init();
        }

        private void Init()
        {
            DefaultController defaultController=new DefaultController();
            controllerList.Add(RequestCode.Game,new GameController());
            controllerList.Add(RequestCode.Room,new RoomController());
            controllerList.Add(RequestCode.User,new UserController());
        }

        public void HandleRespond(RequestCode requestCode,ActionCode actionCode,string data,Client client)
        {
            BaseController controller;
            bool isGet=controllerList.TryGetValue(requestCode, out controller);
            if (isGet==false)
            {
                Console.WriteLine("{0}无法中取得对应的controller",requestCode);
                return;
            }

            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            MethodInfo method=controller.GetType().GetMethod(methodName);
            if (method==null)
            {
                Console.WriteLine("在{0}中无法取得对应的{1}方法",controller,methodName);
            }
            object[] newdata=new object[]{data,client,server};
            object ob=method.Invoke(controller, newdata);
            if (ob==null||string.IsNullOrEmpty(ob.ToString()))
            {
                return;
            }
            server.SendRespond(client,actionCode,ob.ToString());
        }
}
}
