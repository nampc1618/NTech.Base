using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using static NTech.Base.Resources.NNetSocket.NClientSocket;
using System.Runtime.Remoting.Messaging;

namespace NTech.Base.Resources.NNetSocket
{
    public class NServerSocket
    {
        //Port: 1,024 to 65,535
        #region Enum
        public enum EConnectionEventServer
        {
            SERVER_LISTEN,
            SERVER_RECEIVEDATA
        }
        #endregion

        #region callback variable
        IAsyncResult m_asynResult;
        public AsyncCallback m_callback;
        #endregion

        #region Variable
        private string m_errorMsg = string.Empty;
        private string m_ReceiveString = string.Empty;
        private bool m_flagWaitForData = false;
        private Socket m_socketWelcome = null;
        private Socket m_socketConnection = null;
        private Thread m_threadWatchPort = null;
        #endregion

        #region Property
        public string Msg
        {
            get { return this.m_errorMsg; }
            set { this.m_errorMsg = value; }
        }
        public string ReceiveString
        {
            get { return this.m_ReceiveString; }
            set { this.m_ReceiveString = value; }
        }
        #endregion

        #region Methods
        public void StartListening(int port)
        {
            try
            {
                bool flag = this.m_socketWelcome == null;
                if (flag)
                {
                    IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
                    this.m_socketWelcome = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this.m_socketWelcome.Bind(localEP);
                    this.m_socketWelcome.Listen(100);
                    this.m_threadWatchPort = new Thread(new ThreadStart(this.WatchPort));
                    this.m_threadWatchPort.IsBackground = true;
                    this.m_threadWatchPort.Start();

                    ConnectionEventCallback?.Invoke(EConnectionEventServer.SERVER_LISTEN, true);
                }
            }
            catch (Exception ex)
            {
                this.m_errorMsg = ex.Message;
            }
        }
        public void WatchPort()
        {
            while (true)
            {
                try
                {
                    this.m_socketConnection = this.m_socketWelcome.Accept();
                    //if (!m_flagWaitForData)
                    //{
                    WaitForData();
                    //    m_flagWaitForData = true;
                    //}
                }
                catch (Exception ex)
                {
                    m_errorMsg = ex.Message;
                    ServerErrorEventCallback?.Invoke(m_errorMsg);
                    break;
                }
            }
        }
        public bool SetReceiveTimeOut(int millisecond)
        {
            bool result;
            try
            {
                this.m_socketConnection.ReceiveTimeout = millisecond;
                result = true;
            }
            catch (Exception ex)
            {
                this.m_errorMsg = ex.Message;
                result = false;
            }
            return result;
        }
        public void WaitForData()
        {
            try
            {
                if (m_callback == null)
                {
                    m_callback = new AsyncCallback(OnReceiveData);
                }
                CSocketPacket theSocketPkt = new CSocketPacket();
                theSocketPkt.thisSocket = this.m_socketConnection;
                m_asynResult = this.m_socketConnection.BeginReceive(theSocketPkt.dataBuffer, 0, theSocketPkt.dataBuffer.Length, SocketFlags.None, m_callback, theSocketPkt);
            }
            catch (Exception ex)
            {
                m_errorMsg = ex.Message;
                ServerErrorEventCallback?.Invoke(m_errorMsg);
            }
        }
        public void OnReceiveData(IAsyncResult asyn)
        {
            try
            {
                CSocketPacket theSocketId = (CSocketPacket)asyn.AsyncState;
                int iRx = 0;
                iRx = theSocketId.thisSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = decoder.GetChars(theSocketId.dataBuffer, 0, iRx, chars, 0);
                System.String szData = new System.String(chars);
                this.m_ReceiveString = szData;

                ConnectionEventCallback?.Invoke(EConnectionEventServer.SERVER_RECEIVEDATA, szData);
                WaitForData();
            }
            catch (Exception ex)
            {
                m_errorMsg = ex.Message;
                ServerErrorEventCallback?.Invoke(m_errorMsg);
            }
        }
        public bool SendMsg(string Msg, string utf8 = null)
        {
            bool result;
            try
            {
                if (this.m_socketConnection != null)
                {
                    byte[] bytes;
                    if (string.IsNullOrEmpty(utf8))
                        bytes = Encoding.ASCII.GetBytes(Msg);
                    else
                        bytes = Encoding.UTF8.GetBytes(Msg);
                    this.m_socketConnection.Send(bytes);
                    result = true;
                }
                else
                {
                    result = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                this.m_errorMsg = ex.Message;
                result = false;
                ServerErrorEventCallback?.Invoke(m_errorMsg);
            }
            return result;
        }
        public bool SendMsg(byte[] Msg)
        {
            bool result;
            try
            {
                bool connected = this.m_socketConnection.Connected;
                if (connected)
                {
                    this.m_socketConnection.Send(Msg);
                    result = true;
                }
                else
                {
                    result = false;
                }
                result = true;
            }
            catch (Exception ex)
            {
                this.m_errorMsg = ex.Message;
                result = false;
                ServerErrorEventCallback?.Invoke(m_errorMsg);
            }

            return result;
        }
        #endregion

        #region Event
        public delegate void ConnectionEventHandler(EConnectionEventServer e, object obj);
        public event ConnectionEventHandler ConnectionEventCallback;

        public delegate void ErrorEventHandler(string errorMsg);
        public event ErrorEventHandler ServerErrorEventCallback;
        #endregion
    }
}
