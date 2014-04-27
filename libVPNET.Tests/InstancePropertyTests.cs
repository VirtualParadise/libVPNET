using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VP.Extensions;

namespace VP.Tests
{
    [TestClass]
    public class InstancePropertyTests : InstanceTestBase
    {
        [TestMethod]
        public void Disposal()
        {
            var i = new Instance();
            i.Dispose();

            VPNetAssert.ThrowsDisposed(_ => i.Property.AddObject(Samples.VPObject));
            VPNetAssert.ThrowsDisposed(_ => i.Property.ChangeObject(Samples.VPObject));
            VPNetAssert.ThrowsDisposed(_ => i.Property.ClickObject(0));
            VPNetAssert.ThrowsDisposed(_ => i.Property.ClickObject(Samples.VPObject));
            VPNetAssert.ThrowsDisposed(_ => i.Property.ClickObject(0, Vector3.Zero));
            VPNetAssert.ThrowsDisposed(_ => i.Property.ClickObject(Samples.VPObject, Vector3.Zero));
            VPNetAssert.ThrowsDisposed(_ => i.Property.CreateObject("vase1.rwx", Vector3.Zero));
            VPNetAssert.ThrowsDisposed(_ => i.Property.CreateObject("vase1.rwx", Vector3.Zero, Rotation.Zero));
            VPNetAssert.ThrowsDisposed(_ => i.Property.DeleteObject(0));
            VPNetAssert.ThrowsDisposed(_ => i.Property.DeleteObject(Samples.VPObject));
            VPNetAssert.ThrowsDisposed(_ => i.Property.QueryCell(0,0));
        }

        [TestMethod]
        public void QueryCell()
        {
            var cmdrData = NewCmdrData();

            var found = false;
            var done  = false;

            cmdrData.Property.QueryCellResult += (i, o) =>
            {
                Assert.IsTrue(o.Id >= 0);

                Console.WriteLine("Query: {0}, {1}", o.Id, o.Model);
                found = true;
            };

            cmdrData.Property.QueryCellEnd += (i, x, z) =>
            {
                Assert.AreEqual(0, x);
                Assert.AreEqual(0, z);
                done = true;
            };

            cmdrData.Property.QueryCell(0, 0);

            TestPump.AllUntil( () => found && done, cmdrData );

            Assert.IsTrue(done, "QueryCellEnd never fired");

            if (!found)
                Assert.Inconclusive("No objects were found in query; is 0, 0 in test world empty?");
        }

        [TestMethod]
        public void ObjectCreate()
        {
            var punch = NewPunch();
            var judy  = NewJudy();

            var events    = 0;
            var callbacks = 0;

            punch.Property.ObjectCreate += (i, s, o) =>
            {
                if ( s != SessionOf(judy) )
                    return;

                Assert.AreEqual(UserIdOf(judy), o.Owner);
                Assert.AreEqual(Samples.VPObject.Position, o.Position);
                StringAssert.Contains(o.Model, "sign5.rwx");

                if ( o.Model.EndsWith("sample") )
                {
                    Assert.AreEqual(Samples.VPObject.Action, o.Action);
                    Assert.AreEqual(Samples.VPObject.Data.Length, o.Data.Length);
                    Assert.AreEqual(Samples.VPObject.Description, o.Description);
                    Assert.AreEqual(Samples.VPObject.Rotation, o.Rotation);
                    Assert.AreEqual(Samples.VPObject.Type, o.Type);
                }
                else if ( o.Model.EndsWith("sample2") )
                    Assert.AreEqual(Rotation.Zero, o.Rotation);
                else if ( o.Model.EndsWith("sample3") )
                    Assert.AreEqual(Samples.Rotation, o.Rotation);
                else
                    Assert.Inconclusive("Unexpected object with partial name match");
                
                // Inaccurate sanity test
                Assert.IsTrue(o.Time.Year >= 2014);

                events++;
            };

            judy.Property.CallbackObjectCreate += (i, rc, o) =>
            {
                if (rc != ReasonCode.Success)
                    throw new Exception( "Reason code failure: " + rc.ToString() );

                callbacks++;
            };

            judy.Property.AddObject(Samples.VPObject);
            judy.Property.CreateObject("sign5.rwx#sample2", Samples.Vector3);
            judy.Property.CreateObject("sign5.rwx#sample3", Samples.Vector3, Samples.Rotation);

            TestPump.AllUntil( () => events + callbacks >= 6, punch, judy );

            Assert.IsTrue(events == 3, "ObjectCreate event not fired exactly three times");
            Assert.IsTrue(callbacks == 3, "CallbackObjectCreate not fired exactly three times");
        }

        /// <summary>
        /// Failing due to http://virtualparadise.org/bugtracker/view.php?id=40
        /// </summary>
        [TestMethod]
        public void ObjectChange()
        {
            var punch = NewPunch();
            var judy  = NewJudy();

            var objIds    = new HashSet<int>();
            var events    = 0;
            var callbacks = 0;

            punch.Property.ObjectCreate += (i, s, o) =>
            {
                if ( s != SessionOf(judy) )
                    return;

                objIds.Add(o.Id);
                o.Model = "sign1.rwx";
                judy.Property.ChangeObject(o);
            };

            punch.Property.ObjectChange += (i, s, o) =>
            {
                if ( s != SessionOf(judy) )
                    return;

                Assert.IsTrue( objIds.Contains(o.Id) );

                // Inaccurate sanity test
                Assert.IsTrue(o.Time.Year >= 2014);

                Assert.AreEqual("sign1.rwx", o.Model);
                // TODO: reintroduce this check when bug 40 is fixed
                //Assert.AreEqual(UserIdOf(judy), o.Owner);
                Assert.AreEqual(Samples.VPObject.Position, o.Position);
                Assert.AreEqual(Samples.VPObject.Action, o.Action);
                Assert.AreEqual(Samples.VPObject.Data.Length, o.Data.Length);
                Assert.AreEqual(Samples.VPObject.Description, o.Description);
                Assert.AreEqual(Samples.VPObject.Rotation, o.Rotation);
                Assert.AreEqual(Samples.VPObject.Type, o.Type);
                Assert.AreEqual(Samples.Rotation, o.Rotation);

                events++;
            };

            judy.Property.CallbackObjectChange += (i, rc, o) =>
            {
                if (rc != ReasonCode.Success)
                    throw new Exception( "Reason code failure: " + rc.ToString() );

                callbacks++;
            };

            judy.Property.AddObject(Samples.VPObject);
            judy.Property.AddObject(Samples.VPObject);
            judy.Property.AddObject(Samples.VPObject);

            TestPump.AllUntil( () => events + callbacks >= 6, punch, judy );

            Assert.IsTrue(events == 3, "ObjectChange event not fired exactly three times");
            Assert.IsTrue(callbacks == 3, "CallbackObjectChange not fired exactly three times");
        }

        [TestMethod]
        public void ObjectDelete()
        {
            var punch = NewPunch();
            var judy  = NewJudy();

            var objIds    = new HashSet<int>();
            var events    = 0;
            var callbacks = 0;

            punch.Property.ObjectCreate += (i, s, o) =>
            {
                if ( s != SessionOf(judy) )
                    return;

                if (objIds.Count == 0)
                    judy.Property.DeleteObject(o);
                else
                    judy.Property.DeleteObject(o.Id);

                objIds.Add(o.Id);
            };

            punch.Property.ObjectDelete += (i, s, id) =>
            {
                if ( s != SessionOf(judy) )
                    return;

                Assert.IsTrue( objIds.Remove(id) );
                events++;
            };

            judy.Property.CallbackObjectDelete += (i, rc, o) =>
            {
                if (rc != ReasonCode.Success)
                    throw new Exception( "Reason code failure: " + rc.ToString() );

                callbacks++;
            };

            judy.Property.AddObject(Samples.VPObject);
            judy.Property.AddObject(Samples.VPObject);

            TestPump.AllUntil( () => events + callbacks >= 6, punch, judy );

            Assert.IsTrue(events == 2, "ObjectDelete event not fired exactly twice");
            Assert.IsTrue(callbacks == 2, "CallbackObjectDelete not fired exactly twice");
            Assert.IsTrue(objIds.Count == 0);
        }

        [TestMethod]
        public void ObjectGet()
        {
            var punch = NewPunch();
            var judy  = NewJudy();

            var objIds    = new HashSet<int>();
            var callbacks = 0;

            punch.Property.ObjectCreate += (i, s, o) =>
            {
                if ( s != SessionOf(judy) )
                    return;

                objIds.Add(o.Id);
                punch.Property.GetObject(o.Id);
            };

            punch.Property.CallbackObjectGet += (i, rc, o) =>
            {
                if (rc != ReasonCode.Success)
                    throw new Exception( "Reason code failure: " + rc.ToString() );

                Assert.IsTrue( objIds.Contains(o.Id) );

                // Inaccurate sanity test
                Assert.IsTrue(o.Time.Year >= 2014);

                Assert.AreEqual(Samples.VPObject.Model, o.Model);
                Assert.AreEqual(UserIdOf(judy), o.Owner);
                Assert.AreEqual(Samples.VPObject.Position, o.Position);
                Assert.AreEqual(Samples.VPObject.Action, o.Action);
                // TODO: reintroduce this check when unknown VP bug is fixed
                //Assert.AreEqual(Samples.VPObject.Data.Length, o.Data.Length);
                Assert.AreEqual(Samples.VPObject.Description, o.Description);
                Assert.AreEqual(Samples.VPObject.Rotation, o.Rotation);
                Assert.AreEqual(Samples.VPObject.Type, o.Type);
                Assert.AreEqual(Samples.Rotation, o.Rotation);

                callbacks++;
            };

            judy.Property.AddObject(Samples.VPObject);
            judy.Property.AddObject(Samples.VPObject);
            judy.Property.AddObject(Samples.VPObject);

            TestPump.AllUntil( () => callbacks >= 3, punch, judy );

            Assert.IsTrue(callbacks == 3, "CallbackObjectGet not fired exactly three times");
        }

        [TestMethod]
        public void ObjectGet_Exceptions()
        {
            var cmdrData = NewCmdrData();
            var fired    = false;

            cmdrData.Property.CallbackObjectGet += (i, rc, o) =>
            {
                Assert.AreEqual(ReasonCode.ObjectNotFound, rc);
                fired = true;
            };

            cmdrData.Property.GetObject(-1);

            TestPump.AllUntil( () => fired, cmdrData );

            Assert.IsTrue(fired, "CallbackObjectGet never fired");
        }

        [TestMethod]
        public void ObjectClick()
        {
            var cmdrData   = NewCmdrData();
            var punch      = NewPunch();
            var judy       = NewJudy();
            var judyClicks = 0;
            var dataClicks = 0;
            var objId      = -1;

            punch.Property.ObjectCreate += (i, s, o) =>
            {
                if ( s != SessionOf(judy) )
                    return;

                objId = o.Id;

                punch.Property.ClickObject(o.Id);
                punch.Property.ClickObject(o);
                punch.Property.ClickObject(o.Id, SessionOf(judy));
                punch.Property.ClickObject(o, SessionOf(judy));

                punch.Property.ClickObject(o.Id, Samples.Vector3);
                punch.Property.ClickObject(o, Samples.Vector3);
                punch.Property.ClickObject(o.Id, Samples.Vector3, SessionOf(judy));
                punch.Property.ClickObject(o, Samples.Vector3, SessionOf(judy));
            };

            judy.Property.ObjectClick += (i, c) =>
            {
                if ( c.Session != SessionOf(punch) )
                    return;

                Assert.AreEqual(objId, c.Id);
                
                if ( !c.Position.Equals(Vector3.Zero) )
                    Assert.AreEqual(Samples.Vector3, c.Position);

                judyClicks++;
            };

            cmdrData.Property.ObjectClick += (i, c) =>
            {
                if ( c.Session != SessionOf(punch) )
                    return;

                Assert.AreEqual(objId, c.Id);
                
                if ( !c.Position.Equals(Vector3.Zero) )
                    Assert.AreEqual(Samples.Vector3, c.Position);

                dataClicks++;
            };

            judy.Property.AddObject(Samples.VPObject);

            TestPump.AllUntil( () => dataClicks + judyClicks >= 12, punch, judy, cmdrData );

            Assert.IsTrue(dataClicks == 4, "ObjectClick event not fired for Data exactly four times");
            Assert.IsTrue(judyClicks == 8, "ObjectClick event not fired for Judy exactly eight times");
        }
    }
}