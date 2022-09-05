#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace DominoCode
{

    public class MissingComponetsAndPrefabs
    {
        [MenuItem("Tools/Domino Code/Log Missing Components and Prefabs")]
        public static void LogMissingComponentsAndPrefabs()
        {
            missingDataResult.Clear();

            GameObject[] rootGO = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (GameObject go in rootGO) FindMissingOne(go);

            Debug.Log("Total Missing: " + missingDataResult.Count);

            for (int i = 0; i < missingDataResult.Count; i++)
            {
                Debug.Log(missingDataResult[i].path, missingDataResult[i].missingGO);
            }

        }

        public static List<MissingData> missingDataResult = new List<MissingData>();

        public static void FindMissingOne(GameObject go, string path = "")
        {
            string currentPath = path + " / " + go.name;
            Component[] components = go.GetComponents<Component>();

            PrefabAssetType pt = PrefabUtility.GetPrefabAssetType(go);

            if (pt == PrefabAssetType.MissingAsset)
            {
                StorePrefabData(currentPath + " / " + go.name, go);
            }
            else
            {
                for (int i = 0; i < components.Length; i++)
                {
                    if (components[i] == null) StoreComponentData(currentPath, go);
                    continue;
                }
            }

            for (int j = 0; j < go.transform.childCount; j++)
            {
                GameObject childObject = go.transform.GetChild(j).gameObject;
                pt = PrefabUtility.GetPrefabAssetType(childObject);

                if (pt == PrefabAssetType.MissingAsset)
                {
                    StorePrefabData(currentPath + " / " + go.name + " / " + childObject.name, childObject);
                }
                else
                {
                    FindMissingOne(childObject, currentPath);
                }
            }


        }

        public static void StoreComponentData(string path, GameObject go)
        {
            MissingData result;
            result.path = "[Missng Component] " + path;
            result.missingGO = go;

            missingDataResult.Add(result);
        }
        public static void StorePrefabData(string path, GameObject go)
        {
            MissingData result;
            result.path = "[Missng Prefab] " + path;
            result.missingGO = go;

            missingDataResult.Add(result);
        }

    }

    public struct MissingData
    {
        public string path;
        public GameObject missingGO;
    }

}
#endif