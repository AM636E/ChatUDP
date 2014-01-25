using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common;

namespace DataTest
{
    [TestClass]
    public class DataTest
    {
        [TestMethod]
        public void StatusOK()
        {
            Command c = Command.Login;
            Status s = Status.OK;
            string login = "wowa";
            string message = "message";

            Data data = new Data() { ClientLogin = login, Message = message, Command = c, Status = s };
            byte[] bytes = data.ToBytes();

            Data test = new Data(bytes);

            Assert.AreEqual<Status>(s, test.Status, "Status is not decoded");
        }
        [TestMethod]
        public void StatusLoginTaken()
        {
            Command c = Command.Login;
            Status s = Status.LOGIN_TAKEN;
            string login = "wowa";
            string message = "message";

            Data data = new Data() { ClientLogin = login, Message = message, Command = c, Status = s };
            byte[] bytes = data.ToBytes();

            Data test = new Data(bytes);

            Assert.AreEqual<Status>(s, test.Status, "Status is not decoded");
        }
        [TestMethod]
        public void StatusRandomServerError()
        {
            Command c = Command.Login;
            Status s = Status.RANDOM_SERVER_ERROR;
            string login = "wowa";
            string message = "message";

            Data data = new Data() { ClientLogin = login, Message = message, Command = c, Status = s };
            byte[] bytes = data.ToBytes();

            Data test = new Data(bytes);

            Assert.AreEqual<Status>(s, test.Status, "Status is not decoded");
        }
        [TestMethod]
        public void LoginTest()
        {
            Command c = Command.Login;
            Status s = Status.OK;
            string login = "wowa";
            string message = "message";

            Data data = new Data() { ClientLogin = login, Message = message, Command = c, Status = s };
            byte[] bytes = data.ToBytes();

            Data test = new Data(bytes);

            Assert.AreEqual<String>(login, test.ClientLogin, "Login is not decoded");
        }
        [TestMethod]
        public void LongMessageTest()
        {
            Command c = Command.Login;
            Status s = Status.OK;
            string login = "wowa";
            string message = "messasdfasdfasdfasdage";

            Data data = new Data() { ClientLogin = login, Message = message, Command = c, Status = s };
            byte[] bytes = data.ToBytes();

            Data test = new Data(bytes);

            Assert.AreEqual<String>(login, test.ClientLogin, "Login is not decoded");
        }

        [TestMethod]
        public void LongLoginTest()
        {
            Command c = Command.Login;
            Status s = Status.OK;
            string login = "woasdfafasdfasdwa";
            string message = "message";

            Data data = new Data() { ClientLogin = login, Message = message, Command = c, Status = s };
            byte[] bytes = data.ToBytes();

            Data test = new Data(bytes);

            Assert.AreEqual<String>(login, test.ClientLogin, "Login is not decoded");
        }
        [TestMethod]
        public void MessageTest()
        {
            Command c = Command.Login;
            Status s = Status.OK;
            string login = "wowa";
            string message = "message";

            Data data = new Data() { ClientLogin = login, Message = message, Command = c, Status = s };
            byte[] bytes = data.ToBytes();

            Data test = new Data(bytes);

            Assert.AreEqual<String>(message, test.Message, "Message is not decoded");
        }
        [TestMethod]
        public void WithServerTest()
        {
            Command c = Command.Login;
            Status s = Status.OK;
            string login = "wowa";
            string message = "message";

            Data data = new Data() { ClientLogin = login, Message = message, Command = c, Status = s };
            byte[] bytes = data.ToBytes();

            Data test = new Data(bytes);

            ChatClient client = new ChatClient(login);

            client.Received += (oe, e) =>
            {
                test = new Data(client.Data);
                Assert.AreEqual<String>(login, test.ClientLogin, "Message is not decoded");
            };            
        }
    }
}
