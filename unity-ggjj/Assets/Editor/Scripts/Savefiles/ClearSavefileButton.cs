using System.Collections;
using System.Collections.Generic;
using Savefiles;
using UnityEngine;

using UnityEditor;
using UnityEngine;
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
        Proxy.DeleteSaveData();
    }
}