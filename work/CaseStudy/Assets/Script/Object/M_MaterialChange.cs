using System.Collections.Generic;
using UnityEngine;

public class M_MaterialChange : MonoBehaviour
{
    [Header("�K�p����}�e���A��"), SerializeField]
    Material m_Material;

    // ���̃}�e���A����ۑ����邽�߂̃f�B�N�V���i���[
    Dictionary<SpriteRenderer, Material> originalMaterials = new Dictionary<SpriteRenderer, Material>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpriteRenderer[] childRenderers = collision.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer childRenderer in childRenderers)
        {
            // �q�b�g�����I�u�W�F�N�g�̃}�e���A����ۑ�����
            if (!originalMaterials.ContainsKey(childRenderer))
            {
                originalMaterials.Add(childRenderer, childRenderer.material);
            }

            // �}�e���A���ύX
            childRenderer.material = m_Material;
        }

        Debug.Log("�}�e���A���K�p");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SpriteRenderer[] childRenderers = collision.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer childRenderer in childRenderers)
        {
            // ���̃}�e���A���ɖ߂�
            if (originalMaterials.ContainsKey(childRenderer))
            {
                childRenderer.material = originalMaterials[childRenderer];
                originalMaterials.Remove(childRenderer);
            }
        }
    }
}
