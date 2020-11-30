using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class URPVRoidMaterilUtility
{
    /// <summary>
    /// Toonマテリアルを保存する
    /// </summary>
    public static void SaveToonMaterial(Material material, Texture baseMap, Texture shadeTex, Texture normal, Texture emittive,
        Color color, Color shade)
    {
        //変更するシェーダーを取得する
        var toon = Shader.Find("Universal Render Pipeline/Toon");
        //シェーダーを変更する
        material.shader = toon;
        //新しいマテリアルにテクスチャを適用する
        material.SetTexture("_BaseMap", baseMap);
        material.SetTexture("_1st_ShadeMap", shadeTex);
        material.SetTexture("_NormalMap", normal);
        material.SetTexture("_EmissionMap", emittive);
        //新しいマテリアルに初期の色を適用する
        material.SetColor("_BaseColor", color);
        material.SetColor("_1st_ShadeColor", shade);
        //カリングモードの初期化
        material.SetInt("_CullMode", 2);
        material.SetInt("_ClippingMode", 2);
        //透明処理に関する初期化
        material.SetFloat("_IsBaseMapAlphaAsClippingMask", 1.0f);
        material.SetFloat("Tweak_transparency", 0.0f);
        //アウトラインの初期化
        material.SetFloat("_Is_BlendBaseColor", 1.0f);
    }
    /// <summary>
    /// Litマテリアルを保存する
    /// </summary>
    public static void SaveLitMaterial(Material material, Texture baseMap, Texture normal, Texture emittive, Color color)
    {
        //変更するシェーダーを取得する
        var lit = Shader.Find("Universal Render Pipeline/Lit");
        //シェーダーを変更する
        material.shader = lit;
        //新しいマテリアルにテクスチャを適用する
        material.SetTexture("_BaseMap", baseMap);
        material.SetTexture("_BumpMap", normal);
        material.SetTexture("_EmissionMap", emittive);
        //新しいマテリアルに初期の色を適用する
        material.SetColor("_Color", color);
        material.SetFloat("_AlphaClip", 1.0f);
    }
}
