using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chat;
using Common;
namespace DataTest
{
    [TestClass]
    public class ServerMethodsTest
    {
        [TestMethod]
        public void TestTaken()
        {
            ChatServer s = new ChatServer(1001);

            s.Clients.Add(new ClientInfo(){Login = "wasia", Adress = null});
            s.Clients.Add(new ClientInfo() { Login = "olla", Adress = null });
            s.Clients.Add(new ClientInfo() { Login = "dima", Adress = null });
            s.Clients.Add(new ClientInfo() { Login = "hottabich", Adress = null });
            s.Clients.Add(new ClientInfo() { Login = "wasia", Adress = null });

            Assert.AreEqual<bool>(true, s.IsLoginFree("dido"));
        }
        [TestMethod]
        public void TestNotTaken()
        {
            ChatServer s = new ChatServer(1002);

            s.Clients.Add(new ClientInfo() { Login = "wasia", Adress = null });
            s.Clients.Add(new ClientInfo() { Login = "olla", Adress = null });
            s.Clients.Add(new ClientInfo() { Login = "dima", Adress = null });
            s.Clients.Add(new ClientInfo() { Login = "hottabich", Adress = null });
            s.Clients.Add(new ClientInfo() { Login = "wasia", Adress = null });

            Assert.AreEqual<bool>(false, s.IsLoginFree("wasia"));
        }
    }
}
