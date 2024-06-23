using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RequireComponentInChildren : PropertyAttribute
{
    public System.Type requiredComponent;

    public RequireComponentInChildren(System.Type componentType)
    {
        requiredComponent = componentType;
    }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(RequireComponentInChildren))]
public class RequireComponentInChildrenDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (EditorApplication.isPlaying == true) return;

        RequireComponentInChildren requireAttribute = (RequireComponentInChildren)attribute;

        // ���� ������Ʈ�� �ڽĵ鿡 ���� ������Ʈ�� �ִ��� Ȯ���ϰ� ������ �߰�
        MonoBehaviour monoBehaviour = property.serializedObject.targetObject as MonoBehaviour;

        if (monoBehaviour != null)
        {
            foreach (Transform child in monoBehaviour.transform)
            {
                if (child.GetComponent(requireAttribute.requiredComponent) == null)
                {
                    string childName = requireAttribute.requiredComponent.Name;
                    Transform childTransform = child.transform.Find(childName);

                    if (childTransform == null)
                    {
                        // �ڽ� ������Ʈ�� ������ ���� ����
                        GameObject newChild = new GameObject(childName);
                        newChild.transform.SetParent(child.transform);

                        newChild.gameObject.AddComponent(new RectTransform().GetType());
                        newChild.gameObject.AddComponent(requireAttribute.requiredComponent);

                        ClientUtility.OnResetLocalPos(ref newChild);

                        newChild.gameObject.SetActive(false);
                    }
                }
            }
        }

        EditorGUI.PropertyField(position, property, label);
    }

    
}
#endif