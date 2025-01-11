using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAssetCommon
{
    public interface ILogListener
    {
        void OnError(string message)
        {
    
        }

        void OnWarning(string message)
        {

        }

        void OnTrace(string message)
        {

        }

        void OnInfo(string message)
        {

        }

        void OnMessage(LogLevel level, string message)
        {

        }
    }
}
