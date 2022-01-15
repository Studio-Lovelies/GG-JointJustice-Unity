using System;
using System.Collections;
using NUnit.Framework;

namespace Tests.PlayModeTests.Tools
{
    public class TestTools
    {
        public static IEnumerator WaitForState(Func<bool> hasReachedState, double timeoutInSeconds = 10)
        {
            DateTime timeoutAt = DateTime.Now.AddSeconds(timeoutInSeconds); // we cannot rely on UnityEngine.Time inside tests
            while (DateTime.Now < timeoutAt)
            {
                yield return null;
                if (hasReachedState())
                {
                    break;
                }
            }
            if (DateTime.Now >= timeoutAt)
                Assert.Fail($"Waiting for an expected state timed out (took longer than {timeoutInSeconds} seconds){Environment.NewLine}If increasing the timeout does not resolve this and the associated expression worked before making any changes to this project, a precondition required for this test has changed and needs to be updated accordingly");
        }
    }
}
