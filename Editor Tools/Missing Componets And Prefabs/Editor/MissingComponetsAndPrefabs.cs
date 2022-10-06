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
            //Clear the previous missing data
            missingDataResult.Clear();

            //Start From getting all the root objects in Scene
            GameObject[] rootGO = SceneManager.GetActiveScene().GetRootGameObjects();

            //Find Missing Refence in Root Objects and their Childrens
            foreach (GameObject go in rootGO)
            {
                FindMissingOne(go);
            }

            //Total Missing Refence Found
            Debug.Log("Total Missing: " + missingDataResult.Count);

            //Log All Missing Prefabs & Components 
            for (int i = 0; i < missingDataResult.Count; i++)
            {
                Debug.Log(missingDataResult[i].path, missingDataResult[i].missingGO);
            }

        }

        //Stores Missing Data
        public static List<MissingData> missingDataResult = new List<MissingData>();

        /// <summary>
        /// Find Missing Reference
        /// </summary>
        /// <param name="go">GameObject to check Missing Refence</param>
        /// <param name="path">path of this GameObject</param>
        public static void FindMissingOne(GameObject go, string path = "")
        {
            //Set Default path for Root Object and Path derive from parent for children object
            string currentPath = path + " / " + go.name;
            
            Component[] components = go.GetComponents<Component>();

            //Take gameObject as Prefab
            PrefabAssetType pt = PrefabUtility.GetPrefabAssetType(go);

            //Check if prefab is Missing
            if (pt == PrefabAssetType.MissingAsset)
            {
                StorePrefabData(currentPath + " / " + go.name, go);
            }
            //Check for its Components
            else
            {
                for (int i = 0; i < components.Length; i++)
                {
                    if (components[i] == null) StoreComponentData(currentPath, go);
                    continue;
                }
            }

            //Check For missing reference in Child
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
                    //Again Check for all missing refence in child taking current child as main Object
                    FindMissingOne(childObject, currentPath);
                }
            }


        }

        /// <summary>
        /// Store Missing Component Data
        /// </summary>
        /// <param name="path">Component's GameObject Path</param>
        /// <param name="go">Component's GameObject</param>
        public static void StoreComponentData(string path, GameObject go)
        {
            MissingData result;
            result.path = "[Missng Component] " + path;
            result.missingGO = go;

            missingDataResult.Add(result);
        }

        /// <summary>
        /// Store Missing Prefab Data
        /// </summary>
        /// <param name="path">Prefab Gameobject Path</param>
        /// <param name="go">Prefab GameObject</param>
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