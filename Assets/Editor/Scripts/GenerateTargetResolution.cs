using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts
{
    [InitializeOnLoad]
    public static class GenerateTargetResolution
    {
        private static readonly (string name, int width, int height) ResolutionData = ("Game Grumps - Joint Justice", 1280, 768);
        private static object _gameViewSizesInstance;
        private static MethodInfo _getGroupMethodInfo;

        static GenerateTargetResolution()
        {
            EditorApplication.delayCall += DelayCall; 
        }

        private static void DelayCall()
        {
            var sizesType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameViewSizes");
            var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
            var instanceProp = singleType.GetProperty("instance");
            _getGroupMethodInfo = sizesType.GetMethod("GetGroup");
            _gameViewSizesInstance = instanceProp.GetValue(null, null);

            if (FindSize(GameViewSizeGroupType.Standalone, ResolutionData.width, ResolutionData.height) != -1)
            {
                return;
            }

            AddCustomSize(GameViewSizeType.FixedResolution, GameViewSizeGroupType.Standalone, ResolutionData.width, ResolutionData.height, ResolutionData.name);
            SetSize(FindSize(GameViewSizeGroupType.Standalone, ResolutionData.name));
            Debug.Log($"Added and selected missing '{ResolutionData.name} ({ResolutionData.width}x{ResolutionData.height})' editor resolution");
        }

        public enum GameViewSizeType
        {
            AspectRatio, FixedResolution
        }

        private static void SetSize(int index)
        {
            var gameViewType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameView");
            var selectedSizeIndexProp = gameViewType.GetProperty("selectedSizeIndex", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var gameViewEditorWindow = EditorWindow.GetWindow(gameViewType);
            selectedSizeIndexProp.SetValue(gameViewEditorWindow, index, null);
        }

        private static void AddCustomSize(GameViewSizeType viewSizeType, GameViewSizeGroupType sizeGroupType, int width, int height, string text)
        {
            var group = GetGroup(sizeGroupType);
            var addCustomSize = _getGroupMethodInfo.ReturnType.GetMethod("AddCustomSize");
            var gameViewSizeType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameViewSize");
            var constructor = gameViewSizeType.GetConstructor(new Type[] { typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameViewSizeType"), typeof(int), typeof(int), typeof(string) });
            var newSize = constructor.Invoke(new object[] { (int)viewSizeType, width, height, text });
            addCustomSize.Invoke(group, new object[] {newSize});
        }

        private static int FindSize(GameViewSizeGroupType sizeGroupType, string text)
        {
            var group = GetGroup(sizeGroupType);
            var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
            var displayTexts = getDisplayTexts.Invoke(group, null) as string[];
            for (var i = 0; i < displayTexts.Length; i++)
            {
                var display = displayTexts[i];
                var name = display.IndexOf('(');
                if (name != -1)
                    display = display.Substring(0, name - 1);
                if (display == text)
                    return i;
            }
            return -1;
        }

        public static int FindSize(GameViewSizeGroupType sizeGroupType, int width, int height)
        {
            var group = GetGroup(sizeGroupType);
            var groupType = group.GetType();
            var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
            var getCustomCount = groupType.GetMethod("GetCustomCount");
            var sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
            var getGameViewSize = groupType.GetMethod("GetGameViewSize");
            var gameViewSizeType = getGameViewSize.ReturnType;
            var widthProp = gameViewSizeType.GetProperty("width");
            var heightProp = gameViewSizeType.GetProperty("height");
            var indexValue = new object[1];
            for (var i = 0; i < sizesCount; i++)
            { 
                indexValue[0] = i;
                var size = getGameViewSize.Invoke(group, indexValue);
                var sizeWidth = (int)widthProp.GetValue(size, null);
                var sizeHeight = (int)heightProp.GetValue(size, null);
                if (sizeWidth == width && sizeHeight == height)
                    return i;
            }
            return -1;
        }

        private static object GetGroup(GameViewSizeGroupType type)
        {
            return _getGroupMethodInfo.Invoke(_gameViewSizesInstance, new object[] { (int)type });
        }
    }
}
