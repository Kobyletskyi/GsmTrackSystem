using Sim900.DTO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim900
{
    public interface ISim900
    {
        AtResult runCmd(string cmdStr, bool handleResponse = true);
        #region File System

        IList<FileInfo> GetFilesList();
        bool WriteFile(string name, byte[] data);

        byte[] ReadFile(string name);

        long? GetFileSize(string name);

        bool RenameFile(string name, string newName);
        bool DeleteFile(string name);
        bool PlayFile(string name);

        #endregion

        #region Network

        List<string> GetProviders();

        void StartGPRS();

        void StopGPRS();

        GeoCoordinate GetLocation();

        string USSD(string ussd, long timeout = 60000, bool ignoreResponse = false);

        ICallResponse Call(string number);

        bool AnswerIncCall();
        void SendSMS(string number, string text);

        void ScanNetwork();

        #endregion

        #region Internet
        string SendGetRequest(string host, string uri);

        string SendPostRequest(string host, string uri, NameValueCollection data);

        #endregion

        string GetIMEI();
    }
}
