using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Tools
{
    public class TestTools
    {
        private const double DEFAULT_TIMEOUT = 10;

        public static IEnumerator WaitForState(Func<bool> hasReachedState, double timeoutInSeconds = DEFAULT_TIMEOUT)
        {
            yield return DoUntilStateIsReached(null, hasReachedState, timeoutInSeconds);
        }

        public static IEnumerator DoUntilStateIsReached(Func<IEnumerator> action, Func<bool> hasReachedState, double timeoutInSeconds = DEFAULT_TIMEOUT)
        {
            DateTime timeoutAt = DateTime.Now.AddSeconds(timeoutInSeconds); // we cannot rely on UnityEngine.Time inside tests
            while (DateTime.Now < timeoutAt)
            {
                yield return action?.Invoke();
                if (hasReachedState())
                {
                    break;
                }
            }

            if (DateTime.Now >= timeoutAt)
            {
                Assert.Fail($"Waiting for an expected state timed out (took longer than {timeoutInSeconds} seconds){Environment.NewLine}If increasing the timeout does not resolve this and the associated expression worked before making any changes to this project, a precondition required for this test has changed and needs to be updated accordingly");
            }        
        }

        public static T[] FindInactiveInScene<T>() where T : Object
        {
            return Resources.FindObjectsOfTypeAll<T>().Where(o => {
                if (o.hideFlags != HideFlags.None)
                {
                    return false;
                }

                return PrefabUtility.GetPrefabAssetType(o) != PrefabAssetType.Regular;
            }).ToArray();
        }

        /// <summary>
        /// Gets an inactive object from the scene using its name in the hierarchy
        /// </summary>
        /// <param name="name">The name of the game object the object is attached to</param>
        /// <typeparam name="T">The type of object to search for.</typeparam>
        /// <returns>The object found, or null if none are found.</returns>
        public static T FindInactiveInSceneByName<T>(string name) where T : Object
        {
            return FindInactiveInScene<T>().SingleOrDefault(obj => obj.name == name);
        }
    }
}
