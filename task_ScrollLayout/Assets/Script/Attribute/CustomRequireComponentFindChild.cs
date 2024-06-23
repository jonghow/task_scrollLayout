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

        // 현재 오브젝트의 자식들에 대해 컴포넌트가 있는지 확인하고 없으면 추가
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
                        // 자식 오브젝트가 없으면 새로 생성
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