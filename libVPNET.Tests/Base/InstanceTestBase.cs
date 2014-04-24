using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VP.Extensions;

namespace VP.Tests
{
    /// <summary>
    /// Basis of all Instance test classes, which auto-connects a watchdog bot that
    /// records the server-provided session ID of testing instances. Also auto-disposes
    /// of created instances and auto-deletes created objects.
    /// </summary>
    public abstract class InstanceTestBase
    {
        List<TestInstance> instances = new List<TestInstance>();

        List<int> objects = new List<int>();

        object mutex = new object();

        Instance watchdog;
        Task     watchtask;
        bool     ending;

        [TestInitialize]
        public void BaseInit()
        {
            instances.Clear();
            watchdog = new Instance();
            ending   = false;

            watchtask = Task.Factory.StartNew(() =>
            {
                watchdog.Avatars.Enter += (i, a) =>
                {
                    var testInst = getTestInstance(a.Name);
                    
                    if (testInst == null)
                        return;
                    
                    testInst.Session = a.Session;
                    testInst.UserId  = a.Id;
                };

                watchdog.Property.ObjectCreate += (i, s, o) =>
                {
                    var testInst = getTestInstance(s);

                    if (testInst == null)
                        return;

                    Console.WriteLine("Test Watchdog: Tracked instance created object id {0}", o.Id);
                    lock (mutex)
                        objects.Add(o.Id);
                };

                watchdog.Property.ObjectDelete += (i, s, id) =>
                {
                    var testInst = getTestInstance(s);

                    if (testInst == null)
                        return;

                     lock (mutex)
                        if ( objects.Remove(id) )
                            Console.WriteLine("Test Watchdog: Untracked deleted object id {0}", id);
                };

                watchdog.Property.CallbackObjectDelete += (i, rc, id) =>
                {
                    Console.WriteLine("Test Watchdog: Callback received for deletion of tracked object {0}", id);

                    if (rc != ReasonCode.Success)
                        throw new Exception( "Test Watchdog could not delete object: " + rc.ToString() );

                    lock (mutex)
                        objects.Remove(id);
                };

                watchdog.TestLogin(Names.Watch).EnterTestWorld();

                while (!ending || objects.Count > 0)
                    watchdog.Pump();
            });
        }

        [TestCleanup]
        public void BaseCleanup()
        {
            ending = true;

            lock (mutex)
                foreach (var obj in objects)
                    watchdog.Property.DeleteObject(obj);

            watchtask.Wait();
            watchdog.Dispose();

            foreach (var instance in instances)
                instance.Instance.Dispose();
        }

        public Instance NewPunch(bool enter = true)
        {
            return NewBot(Names.Punch, enter);
        }

        public Instance NewJudy(bool enter = true)
        {
            return NewBot(Names.Judy, enter);
        }

        public Instance NewCmdrData(bool enter = true)
        {
            return NewBot(Names.Data, enter);
        }

        public Instance NewBot(string name, bool enter = true)
        {
            var inst = new Instance();

            lock (mutex)
                instances.Add( new TestInstance(inst) );

            inst.TestLogin(name);

            if (enter)
                inst.EnterTestWorld();

            return inst;
        }

        public int SessionOf(Instance target)
        {
            var testInst = getTestInstance(target.Name);

            if (testInst == null)
                throw new ArgumentException("Test watchdog is not tracking that bot instance");

            // For test bots that the watchdog has already recorded the session of
            if (testInst.Session != -1)
                return testInst.Session;

            // Else, we may need to wait for session number to be assigned as watchdog
            // may not have seen the bot yet
            var task = Task.Factory.StartNew( () =>
            {
                while (testInst.Session == -1)
                    Thread.Sleep(25);

                return testInst.Session;
            });

            if ( !task.Wait(5000) )
                throw new TimeoutException("SessionOf took too long");
            
            return task.Result;
        }

        public int UserIdOf(Instance target)
        {
            var testInst = getTestInstance(target.Name);

            if (testInst == null)
                throw new ArgumentException("Test watchdog is not tracking that bot instance");

            // For test bots that the watchdog has already recorded the user id of
            if (testInst.UserId != -1)
                return testInst.UserId;

            // Else, we may need to wait for user ID number to be assigned as watchdog
            // may not have seen the bot yet
            var task = Task.Factory.StartNew( () =>
            {
                while (testInst.UserId == -1)
                    Thread.Sleep(25);

                return testInst.UserId;
            });

            if ( !task.Wait(5000) )
                throw new TimeoutException("UserIdOf took too long");
            
            return task.Result;
        }

        TestInstance getTestInstance(string target)
        {
            lock (mutex)
                foreach (var inst in instances)
                    if ( inst.Instance.Name.AsBotName() == target.AsBotName() )
                        return inst;

            return null;
        }

        TestInstance getTestInstance(int session)
        {
            lock (mutex)
                foreach (var inst in instances)
                    if (inst.Session == session)
                        return inst;

            return null;
        }

        TestInstance getTestInstance(Instance target)
        {
            return getTestInstance(target.Name);
        }
    }
}
