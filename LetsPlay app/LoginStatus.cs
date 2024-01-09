using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsPlay_app
{
    public class LoginStatus
    {
        /*
        private static UserSettings _userStatus;

        public static UserSettings UserStatus
        {
            get
            {
                if (_userStatus == null)
                {
                    _userStatus = new UserSettings();
                }

                return _userStatus;
            }
        }
        */

        public static bool IsUserLoggedIn { get; set; }
    }
}
