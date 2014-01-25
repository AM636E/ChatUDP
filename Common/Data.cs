using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum Command
    {
        Login = 0,  //login into chat
        Logout = 1, //logout from chat
        List = 2,   //list messages
        Message = 3, //send message
        None = 4        //no command
    }
    public enum Status
    {
        OK,
        LOGIN_TAKEN,
        RANDOM_SERVER_ERROR
    }
    public class Data
    {
        private string _clientLogin = null;
        private string _message = null;
        private Command _command = Command.None;
        private Status _status = Status.OK;

        public String ClientLogin
        {
            get { return _clientLogin; }
            set { _clientLogin = value; }
        }

        public String Message
        {
            get { return _message; }
            set { _message = value; }
        }
        public Command Command
        {
            get { return _command; }
            set { _command = value; }
        }

        public Status Status
        {
            get
            {
                return _status;
            }

            set { _status = value; }
        }

        public Data()
        {
            _clientLogin = null;
            _message = null;
            _command = Command.None;
        }

        public Data(byte[] data)
        {
            this._command = (Command)BitConverter.ToInt32(data, 0);//get command
            this._status = (Status)BitConverter.ToInt32(data, 4);
            int loginLen = BitConverter.ToInt32(data, 8);//get length of the login        
            if (loginLen > 0)
            {
                this._clientLogin = Encoding.UTF8.GetString(data, 12, loginLen);
            }
            int msgLen = BitConverter.ToInt32(data, 12 + loginLen);//get length of the login;//get length of message


            if (msgLen > 0)
            {
                this._message = Encoding.UTF8.GetString(data, 12 + loginLen + 4, msgLen);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Command, ClientLogin, Message, Status);
        }

        public byte[] ToBytes()
        {
            List<byte> result = new List<byte>();

            result.AddRange(BitConverter.GetBytes((int)Command));
            result.AddRange(BitConverter.GetBytes((int)_status));

            if (this._clientLogin != null)
            {
                result.AddRange(BitConverter.GetBytes(Encoding.UTF8.GetByteCount(_clientLogin)));
                result.AddRange(Encoding.UTF8.GetBytes(_clientLogin));
            }
            else
            {
                result.AddRange(BitConverter.GetBytes(0));
            }

            if (_message != null)
            {
                result.AddRange(BitConverter.GetBytes(_message.Length));
                result.AddRange(Encoding.UTF8.GetBytes(_message));
            }
            else
            {
                result.AddRange(BitConverter.GetBytes(0));
            }

            return result.ToArray();
        }
    }
}
