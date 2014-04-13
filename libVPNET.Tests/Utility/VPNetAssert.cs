using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VP.Tests
{
    static class VPNetAssert
    {
        public static void ThrowsDisposed(Action<object> act)
        {
            bool threw = false;

            try
            {
                act( new object() );
            }
            catch (ObjectDisposedException ex)
            {
                threw = true;
            }

            Assert.IsTrue(threw);
        }

        public static void ThrowsReasonCode(ReasonCode rc, Action<object> act)
        {
            ReasonCode? thrown = null;

            try
            {
                act( new object() );
            }
            catch (VPException ex)
            {
                thrown = ex.Reason;
            }

            if (!thrown.HasValue)
                Assert.Fail("Delegate did not throw a VPException. Expected: {0}", rc);

            Assert.AreEqual(rc, thrown.Value);
        }
    }
}
