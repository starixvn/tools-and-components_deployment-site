using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deployment.Helper
{
    public class ConnectionFlag
    {
        public const int CONNECT_UPDATE_PROFILE = 0x00000001;
        public const int CONNECT_UPDATE_RECENT = 0x00000002;
        public const int CONNECT_TEMPORARY = 0x00000004;
        public const int CONNECT_INTERACTIVE = 0x00000008;
        public const int CONNECT_PROMPT = 0x00000010;
        public const int CONNECT_REDIRECT = 0x00000080;
        public const int CONNECT_CURRENT_MEDIA = 0x00000200;
        public const int CONNECT_COMMANDLINE = 0x00000800;
        public const int CONNECT_CMD_SAVECRED = 0x00001000;
        public const int CONNECT_CRED_RESET = 0x00002000;
    }
}
