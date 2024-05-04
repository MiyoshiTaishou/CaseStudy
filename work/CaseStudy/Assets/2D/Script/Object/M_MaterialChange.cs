using System.Collections.Generic;
using UnityEngine;

public class M_MaterialChange : MonoBehaviour
{
    [Header("�K�p����}�e���A��"), SerializeField]
    Material m_Material;

    /// <summary>
    /// �v���J�n���邩
    /// </summary>
    private bool isStart = false;

    // ���̃}�e���A����ۑ����邽�߂̃f�B�N�V���i���[
    Dictionary<SpriteRenderer, Material> originalMaterials = new Dictionary<SpriteRenderer, Material>();

    private void Update()
    {       
        if (isStart)
        {
            // �}�e���A���� float �̒l�� 0 �܂Ői�߂�
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
        //�v���W�F�N�^�[�N�����̂�
        if(GetComponent<N_ProjectHologram>().GetProjection())
        {
            SpriteRenderer[] childRenderers = collision.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer childRenderer in childRenderers)
            {
                // �q�b�g�����I�u�W�F�N�g�̃}�e���A����ۑ�����
                if (!originalMaterials.ContainsKey(childRenderer))
                {
                    originalMaterials.Add(childRenderer, childRenderer.material);

                    // �}�e���A���ύX
                    childRenderer.material = m_Material;

                    // �}�e���A���� float �̒l�����Z�b�g����
                    childRenderer.material.SetFloat("_Fader", 1.0f);
                    childRenderer.material.SetFloat("_Effect", 0.0f);

                    Debug.Log("�}�e���A���K�p");
                    isStart = true;
                }
            }
        }   
        else
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
