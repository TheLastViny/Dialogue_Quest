using NarrativeSystem.Dictionnaries;
using NarrativeSystem.Variables;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class SeriazableDictionaryConverter<TKey, TValue> : UxmlAttributeConverter<SerializableDictionary<TKey, TValue>>
{
    static string ValueToString(object value) => System.Convert.ToString(value, CultureInfo.InvariantCulture);

    public override string ToString(SerializableDictionary<TKey, TValue> dictionary)
    {
        var dataBuilder = new StringBuilder();

        foreach(var kvp in dictionary)
        {
            dataBuilder.Append($"{ValueToString(kvp.Key)}|{ValueToString(kvp.Value)},");
        }

        return dataBuilder.ToString();
    }

    public override SerializableDictionary<TKey, TValue> FromString(string source)
    {
        var ouputDictionary = new SerializableDictionary<TKey, TValue>();

        var keyValuePairs = source.Split(',');
        foreach(var kvp in keyValuePairs)
        {
            var Fields = kvp.Split('|');   
            TKey key = (TKey)System.Convert.ChangeType(Fields[0], typeof(TKey));
            TValue value = (TValue)System.Convert.ChangeType(Fields[1], typeof(TValue));

            ouputDictionary.Add(key, value);
        }

        return ouputDictionary;
    }
}

[CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
public class SerializableDictionaryDrawerIMGUI : PropertyDrawer
{
    SerializedProperty Property;
    SerializedProperty Keys;
    SerializedProperty Values;

    public override VisualElement CreatePropertyGUI(SerializedProperty inProperty)
    {
        Property = inProperty;
        Keys = inProperty.FindPropertyRelative("keys");
        Values = inProperty.FindPropertyRelative("values");

        var root = new Foldout()
        {
            text = Property.displayName,
            viewDataKey = $"{Property.serializedObject.targetObject.GetInstanceID()}.{Property.name}"
        };

        var listView = new ListView()
        {
            showAddRemoveFooter = true,
            showBorder = true,
            showAlternatingRowBackgrounds = AlternatingRowBackground.All,
            showFoldoutHeader = false,
            showBoundCollectionSize = true,
            reorderable = false,
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
            headerTitle = Property.displayName,
            bindingPath = Keys.propertyPath,
            bindItem = BindListItem,
            onRemove = OnRemove,
            overridingAddButtonBehavior = OnAdd
        };

        root.Add(listView);

        return root;
    }

    void BindListItem(VisualElement element, int itemIndex)
    {
        element.Clear();

        var keyProperty = Keys.GetArrayElementAtIndex(itemIndex);
        var valueProperty = Values.GetArrayElementAtIndex(itemIndex);

        var KeyUI = new PropertyField(keyProperty);
        var ValueUI = new PropertyField(valueProperty);

        element.Add(KeyUI);
        element.Add(ValueUI);

        element.Bind(Property.serializedObject);
    }

    void OnRemove(BaseListView listView)
    {
        if(Keys.arraySize > 0 && listView.selectedIndex >= 0 && listView.selectedIndex < Keys.arraySize)
        {
            int indexToRemove = listView.selectedIndex;
            Keys.DeleteArrayElementAtIndex(indexToRemove);
            Values.DeleteArrayElementAtIndex(indexToRemove);
            Property.serializedObject.ApplyModifiedProperties();
        } 
    }

    void OnAdd(BaseListView listView, Button button)
    {
        var dict = fieldInfo.GetValue(Property.serializedObject.targetObject) as IDictionary;
        GenericMenu menu = new GenericMenu();

        if(dict is VariableRefDictionary)
        {
            menu.AddItem(new GUIContent("String"), false, () => AddElement("String"));
            menu.AddItem(new GUIContent("Int"), false, () => AddElement("Int"));
            menu.AddItem(new GUIContent("Float"), false, () => AddElement("Float"));
            menu.AddItem(new GUIContent("Bool"), false, () => AddElement("Bool"));
        }

        menu.ShowAsContext();
    }

    void AddElement(string type)
    {
        var dict = fieldInfo.GetValue(Property.serializedObject.targetObject) as IDictionary;
        if (dict == null)
        {
            return;
        }

        object value = type switch
        {
            "String" => new StringVariable("", ""),
            "Int" => new IntVariable(0, ""),
            "Float" => new FloatVariable(0f, ""),
            "Bool" => new BoolVariable(false, ""),
            _ => null
        };

        string key = GenerateUniqueKey(dict, type);
        dict.Add(key, value);

        EditorUtility.SetDirty(Property.serializedObject.targetObject);
    }

    string GenerateUniqueKey(IDictionary dict, string baseName)
    {
        int i = 0;
        while (dict.Contains($"{baseName}_{i}"))
        {
            i++;
        }

        return $"{baseName}_{i}";
    }
}
