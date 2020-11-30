using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using VRM;

namespace VRM
{
    public static class URPVRoidChangeMaterialToLit
    {
        [MenuItem("URP/VRoid/Materials Initialize To Lit Shader")]
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
                    .ForEach(ChangeVrmToLit);
                URPVRoidChangeUtility.SaveChangedPrefab(path);
            }
        }

        private static void ChangeVrmToLit(Material material)
        {
            Texture baseMap = default, normal = default, emittive = default;
            Color color = default;
            //各テクスチャを取得する
            baseMap = material.GetTexture("_MainTex");
            normal = material.GetTexture("_BumpMap");
            emittive = material.GetTexture("_EmissionMap");
            //各パラメータを取得する
            color = material.GetColor("_Color");
            //マテリアルに設定する
            URPVRoidMaterilUtility.SaveLitMaterial(material, baseMap, normal, emittive, color);
        }
    }
}