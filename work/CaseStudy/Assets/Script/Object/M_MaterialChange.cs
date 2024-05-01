using System.Collections.Generic;
using UnityEngine;

public class M_MaterialChange : MonoBehaviour
{
    [Header("適用するマテリアル"), SerializeField]
    Material m_Material;

    /// <summary>
    /// 計測開始するか
    /// </summary>
    private bool isStart = false;

    // 元のマテリアルを保存するためのディクショナリー
    Dictionary<SpriteRenderer, Material> originalMaterials = new Dictionary<SpriteRenderer, Material>();

    private void Update()
    {       
        if (isStart)
        {
            // マテリアルの float の値を 0 まで進める
            foreach (var mt in originalMaterials)
            {
                float currentValue = 0f;
                float currentValue2 = 0f;

                if (mt.Key.material.HasProperty("_Fader"))
                {
                    currentValue = mt.Key.material.GetFloat("_Fader");
                }

                if (mt.Key.material.HasProperty("_Effect"))
                {
                    currentValue2 = mt.Key.material.GetFloat("_Effect");
                }

                float newValue = currentValue - Time.deltaTime;
                 float newValue2 = currentValue2 + Time.deltaTime;

                 mt.Key.material.SetFloat("_Fader", newValue);                
                 mt.Key.material.SetFloat("_Effect", newValue2);                
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        //プロジェクター起動中のみ
        if(GetComponent<N_ProjectHologram>().GetProjection())
        {
            SpriteRenderer[] childRenderers = collision.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer childRenderer in childRenderers)
            {
                // ヒットしたオブジェクトのマテリアルを保存する
                if (!originalMaterials.ContainsKey(childRenderer))
                {
                    originalMaterials.Add(childRenderer, childRenderer.material);

                    // マテリアル変更
                    childRenderer.material = m_Material;

                    // マテリアルの float の値をリセットする
                    childRenderer.material.SetFloat("_Fader", 1.0f);
                    childRenderer.material.SetFloat("_Effect", 0.0f);

                    Debug.Log("マテリアル適用");
                    isStart = true;
                }
            }
        }   
        else
        {
            SpriteRenderer[] childRenderers = collision.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer childRenderer in childRenderers)
            {
                // 元のマテリアルに戻す
                if (originalMaterials.ContainsKey(childRenderer))
                {
                    childRenderer.material = originalMaterials[childRenderer];
                    originalMaterials.Remove(childRenderer);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SpriteRenderer[] childRenderers = collision.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer childRenderer in childRenderers)
        {
            // 元のマテリアルに戻す
            if (originalMaterials.ContainsKey(childRenderer))
            {
                childRenderer.material = originalMaterials[childRenderer];
                originalMaterials.Remove(childRenderer);
            }
        }
    }
}
