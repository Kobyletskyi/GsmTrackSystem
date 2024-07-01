using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim900.DTO
{
    public interface ICallResponse : IDisposable
    {
        event Action<string> OnFinish;
        event Action<char, TimeSpan> OnDTMF;
        event Action OnAudioStop;
    }
}
