using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts
{
    public static class GameViewUtils
    {
        private static readonly object GameViewSizesInstance;
        private static readonly MethodInfo GetGroupInfo;

        static GameViewUtils()
        {
            var sizesType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameViewSizes");
            var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
            var instanceProp = singleType.GetProperty("instance");
            GetGroupInfo = sizesType.GetMethod("GetGroup");
            GameViewSizesInstance = instanceProp.GetValue(null, null);
        }

        [UnityEditor.MenuItem("Test/SetTestSize")]
        public static void SetTestSize()
        {
            int idx = FindSize(GameViewSizeGroupType.Standalone, 1280, 720);
            if (idx != -1)
                SetSize(idx);
        }

        private static void SetSize(int index)
        {
            var gvWndType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameView");
            var selectedSizeIndexProp = gvWndType.GetProperty("selectedSizeIndex",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var gvWnd = EditorWindow.GetWindow(gvWndType);
            selectedSizeIndexProp.SetValue(gvWnd, index, null);
        }

        [UnityEditor.MenuItem("Test/SizeDimensionsQuery")]
        public static void SizeDimensionsQueryTest()
        {
            Debug.Log(SizeExists(GameViewSizeGroupType.Standalone, 123, 456));
        }

        public static int FindSize(GameViewSizeGroupType sizeGroupType, string text)
        {
            var group = GetGroup(sizeGroupType);
            var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
            var displayTexts = getDisplayTexts.Invoke(group, null) as string[];
            for (int i = 0; i < displayTexts.Length; i++)
            {
                string display = displayTexts[i];
                int pren = display.IndexOf('(');
                if (pren != -1)
                    display = display.Substring(0, pren - 1);
                if (display == text)
                    return i;
            }

            return -1;
        }

        private static bool SizeExists(GameViewSizeGroupType sizeGroupType, int width, int height)
        {
            return FindSize(sizeGroupType, width, height) != -1;
        }

        private static int FindSize(GameViewSizeGroupType sizeGroupType, int width, int height)
        {
            var group = GetGroup(sizeGroupType);
            var groupType = group.GetType();
            var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
            var getCustomCount = groupType.GetMethod("GetCustomCount");
            int sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
            var getGameViewSize = groupType.GetMethod("GetGameViewSize");
            var gvsType = getGameViewSize.ReturnType;
            var widthProp = gvsType.GetProperty("width");
            var heightProp = gvsType.GetProperty("height");
            var indexValue = new object[1];
            for (int i = 0; i < sizesCount; i++)
            {
                indexValue[0] = i;
                var size = getGameViewSize.Invoke(group, indexValue);
                int sizeWidth = (int)widthProp.GetValue(size, null);
                int sizeHeight = (int)heightProp.GetValue(size, null);
                if (sizeWidth == width && sizeHeight == height)
                    return i;
            }

            return -1;
        }

        private static object GetGroup(GameViewSizeGroupType type)
        {
            return GetGroupInfo.Invoke(GameViewSizesInstance, new object[] { (int)type });
        }

        private static GameViewSizeGroupType GetCurrentGroupType()
        {
            var getCurrentGroupTypeProp = GameViewSizesInstance.GetType().GetProperty("currentGroupType");
            return (GameViewSizeGroupType)(int)getCurrentGroupTypeProp.GetValue(GameViewSizesInstance, null);
        }
    
        [UnityEditor.MenuItem("Test/LogCurrentGroupType")]
        private static void LogCurrentGroupType()
        {
            Debug.Log(GetCurrentGroupType());
        }
    }

    public class Resolution : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            GameViewUtils.SetTestSize();
        }
    }
}