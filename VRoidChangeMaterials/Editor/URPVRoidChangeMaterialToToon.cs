using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace VRM
{
    public static class URPVRoidChangeMaterialToToon
    {
        [MenuItem("URP/VRoid/Materials Initialize To Toon Shader")]
        private static void ChangeVrmModelMaterialChange()
        {
            var paths = URPVRoidChangeUtility.GetSelectPaths();
            //変更するシェーダーを取得する
            var vrm = Shader.Find("VRM/MToon");
            foreach (var path in paths)
            {
                //マテリアルを取得する
                var materials = URPVRoidChangeUtility.GetAllAsset<Material>(path + ".Materials");
                //VRM→Toon
                materials
                    .Where(v => v.shader == vrm)
                    .ToList()
                    .ForEach(ChangeVrmToToon);
                //Prefabを保存する
                URPVRoidChangeUtility.SaveChangedPrefab(path);
            }
        }

        private static void ChangeVrmToToon(Material material)
        {
            Texture baseMap = default, shadeTex = default, normal = default, emittive = default;
            Color color = default, shade = default;
            //各テクスチャを取得する
            baseMap = material.GetTexture("_MainTex");
            shadeTex = material.GetTexture("_ShadeTexture");
            normal = material.GetTexture("_BumpMap");
            emittive = material.GetTexture("_EmissionMap");
            //各パラメータを取得する
            color = material.GetColor("_Color");
            shade = material.GetColor("_ShadeColor");
            //マテリアルに設定する
            URPVRoidMaterilUtility.SaveToonMaterial(material, baseMap, shadeTex, normal, emittive, color, shade);
        }
    }
}