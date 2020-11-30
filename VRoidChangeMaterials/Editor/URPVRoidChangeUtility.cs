using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace VRM
{
    public static class URPVRoidChangeUtility
    {
        /// <summary>
        /// 選択しているオブジェクトのパスを取得する
        /// </summary>
        public static IEnumerable<string> GetSelectPaths()
        {
            List<string> paths = new List<string>();
            var selects = Selection.gameObjects;
            if (Selection.gameObjects.Length == 0)
            {
                Debug.LogError("モデルを選択してください。");
                return paths;
            }
            foreach (var select in selects)
            {
                //Prefabのパスを取得する
                string path = AssetDatabase.GetAssetPath(select);
                if (path.Length == 0)
                {
                    //Sceneビューで選択している場合、プロジェクト内にあるPrefabを取得する
                    GameObject source = PrefabUtility.GetCorrespondingObjectFromSource(select);
                    //Prefabのパスを取得する
                    path = AssetDatabase.GetAssetPath(source);
                    if (path.Length == 0)
                    {
                        Debug.LogError("モデルのPrefabが選択されていません。");
                        return paths;
                    }
                }
                //Prefabがあるフォルダを取得し、その名前を利用するために拡張子を削除する
                paths.Add(path.Replace(".prefab", ""));
            }
            return paths;
        }

        /// <summary>
        /// 変更したPrefabを保存する
        /// </summary>
        public static void SaveChangedPrefab(string path)
        {
            //Prefabにアクセスするためにパスを戻す
            path += ".prefab";
            //Prefabにマテリアルを適用したかチェックするためのコンポーネントを追加する
            GameObject prefab = PrefabUtility.LoadPrefabContents(path);
            //適当な大きさに広げておく（小さすぎる値が入っていた場合の対処）
            System.Array.ForEach(prefab.GetComponentsInChildren<SkinnedMeshRenderer>(),
                s => s.localBounds = new Bounds(Vector3.zero, Vector3.one * 10.0f));
            PrefabUtility.SaveAsPrefabAsset(prefab, path);
            PrefabUtility.UnloadPrefabContents(prefab);
            //保存する
            AssetDatabase.SaveAssets();
            //リフレッシュする
            AssetDatabase.Refresh();
            EditorApplication.QueuePlayerLoopUpdate();
        }

        /// <summary>
        /// そのパスに存在するアセットを全て取得する
        /// </summary>
        public static IEnumerable<T> GetAllAsset<T>(string path) where T : Object
        {
            //指定したディレクトリに入っている全ファイルを取得(子ディレクトリも含む)
            string[] filePathArray = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

            List<T> assetList = new List<T>();

            //取得したファイルの中からアセットだけリストに追加する
            foreach (string filePath in filePathArray)
            {
                T asset = AssetDatabase.LoadAssetAtPath<T>(filePath);
                if (asset != null) assetList.Add(asset);
            }
            return assetList;
        }
    }
}