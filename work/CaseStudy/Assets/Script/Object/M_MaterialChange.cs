using System.Collections.Generic;
using UnityEngine;

public class M_MaterialChange : MonoBehaviour
{
    [Header("適用するマテリアル"), SerializeField]
    Material m_Material;

    // 元のマテリアルを保存するためのディクショナリー
    Dictionary<SpriteRenderer, Material> originalMaterials = new Dictionary<SpriteRenderer, Material>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpriteRenderer[] childRenderers = collision.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer childRenderer in childRenderers)
        {
            // ヒットしたオブジェクトのマテリアルを保存する
            if (!originalMaterials.ContainsKey(childRenderer))
            {
                originalMaterials.Add(childRenderer, childRenderer.material);
            }

            // マテリアル変更
            childRenderer.material = m_Material;
        }

        Debug.Log("マテリアル適用");
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
