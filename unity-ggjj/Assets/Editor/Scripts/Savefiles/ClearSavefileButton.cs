using SaveFiles;
using UnityEngine;
using UnityEditor;

public class ClearSaveDataButton : MonoBehaviour
{
    [UnityEditor.MenuItem("Edit/Delete Save Data %#d")]
    static void DeleteSaveData()
    {
        if (!EditorUtility.DisplayDialog("Delete Save Data", "Are you sure you want to delete your Save Data?\r\n\r\nYou will lose ALL progress in the game!", "Delete", "Cancel"))
        {
            return;
        }

        Debug.LogWarning("Save Data has been successfully deleted!");
        PlayerPrefsProxy.DeleteSaveData();
    }
}