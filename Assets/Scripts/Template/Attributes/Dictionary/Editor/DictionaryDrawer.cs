using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using DrawedDictionary;


public abstract class PropertyDrawerTweaks:PropertyDrawer
{
    public object instance;



    /// <summary>
    /// Проверка кастомный ли класс по имени.
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public bool IsUnityClassObject(string typeName)
    {
        return typeName.Contains("UnityEngine.") || typeName.Contains("UnityEditor.") || typeName.Contains("System.");
    }


    #region Reflection

    /// <summary>
    /// Получение экземпляра обьекта по названию его типа.
    /// </summary>
    public object GetInstanceFromTypeName(Rect position, string keyType, object value)
    {
        var Type = (string)GetVarValue(instance, keyType);
        object Object = value;
        if (!IsUnityClassObject(Type))
        {
            Assembly asm = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(s => s.GetType(Type) != null);
            var inst = Activator.CreateInstance(asm.FullName, Type);
            return Object = inst.Unwrap();
        }
        return value;
    }

    /// <summary>
    /// Проверка на Сериализацию класса.
    /// </summary>
    public bool IsTypeSerilizable(string keyType)
    {
        var Type = (string)GetVarValue(instance, keyType);
        if (!IsUnityClassObject(Type))
        {
            Assembly asm = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(s => s.GetType(Type) != null);
            var inst = Activator.CreateInstance(asm.FullName, Type);
            var obj = inst.Unwrap();
            return obj.GetType().IsSerializable;
        }
        return true;
    }

    

    /// <summary>
    /// Вызов метода из System.object через Reflection
    /// </summary>
    public void CallMethod(object src, string methodName, params object[] args)
    {
        src.GetType().GetMethod(methodName).Invoke(src, args);
    }

    /// <summary>
    /// Получение поля или своства из System.object через Reflection
    /// </summary>
    public object GetVarValue(object src, string propName)
    {
        var prop = src.GetType().GetProperty(propName, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
        if (prop != null)
        {
            return prop.GetValue(src, null);
        }
        else
        {
            return src.GetType().GetField(propName, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).GetValue(src);
        }
    }


    /// <summary>
    /// Получение ссылки на обьект к которому првязан PropertyDrawer (Stackoverflow)
    /// </summary>
    public static object GetTargetObjectOfProperty(SerializedProperty prop)
    {
        if (prop == null) return null;

        var path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        var elements = path.Split('.');
        foreach (var element in elements)
        {
            if (element.Contains("["))
            {
                var elementName = element.Substring(0, element.IndexOf("["));
                var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue_Imp(obj, elementName, index);
            }
            else
            {
                obj = GetValue_Imp(obj, element);
            }
        }
        return obj;
    }

    /// <summary>
    /// Stackoverflow GetTargetObjectOfProperty
    /// </summary>
    private static object GetValue_Imp(object source, string name, int index)
    {
        var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
        if (enumerable == null) return null;
        var enm = enumerable.GetEnumerator();
        //while (index-- >= 0)
        //    enm.MoveNext();
        //return enm.Current;

        for (int i = 0; i <= index; i++)
        {
            if (!enm.MoveNext()) return null;
        }
        return enm.Current;
    }


    /// <summary>
    /// Stackoverflow GetTargetObjectOfProperty
    /// </summary>
    private static object GetValue_Imp(object source, string name)
    {
        if (source == null)
            return null;
        var type = source.GetType();

        while (type != null)
        {
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (f != null)
                return f.GetValue(source);

            var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p != null)
                return p.GetValue(source, null);

            type = type.BaseType;
        }
        return null;
    }

    #endregion
}


[CustomPropertyDrawer(typeof(PDictionary), true)]
public class DictionaryDrawer : PropertyDrawerTweaks
{
    
    float y = 0; //Высота
    float startY = 20; //Высота линиии
    object newK, newV; //Данные нового обьекта

    Rect keyRect = new Rect();
    Rect valueRect = new Rect();
    Rect btnRect = new Rect();

    float fieldWidth = 0; //По названиям понятно что это ширина поля, разделителя между полями и кнопки
    float separatorWidth = 0;
    float btnWidth = 0;

    static bool drawAll = true, drawList = true; //Открыты ли списки.

    bool customClass = false;


    /// <summary>
    /// Метод отображения интерфейса.
    /// </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        startY = EditorGUIUtility.singleLineHeight; //Высота одной строки
        y = position.y; //Позиция по Y 

        instance = GetTargetObjectOfProperty(property); //Получение инстанса обьекта

        CallMethod(instance, "UpdateTypes"); //Запрос на вызов метода UpdateTypes в Dictionary из instance

        EditorGUI.BeginProperty(position, label, property);
        {
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            DefineSizes(position); //Вычисление размеров полей относительно position

            drawAll = DrawFoldoutLabel(position, drawAll, label.text); //Открыт ли лейбл
            if (drawAll)
            {

                position = AddOffcet(position); //Добавления смещения

                DefineSizes(position);

                ////Проверка но кастомный класс
                customClass = false;
                if (!IsTypeSerilizable("KType") || !IsTypeSerilizable("VType"))
                {
                    if (!IsTypeSerilizable("KType"))
                    {
                        NotSerializbleError(position, "KType");
                    }
                    if (!IsTypeSerilizable("VType"))
                    {
                        NotSerializbleError(position, "VType");
                    }
                    AddHeight();
                    EditorGUI.indentLevel = indent;
                    EditorGUI.EndProperty();
                    AddHeight();
                    return;
                }
                ///


                //Отрисовка полей для ввода и добавления
                #region AddFields
                if (DrawField(valueRect, (string)GetVarValue(instance, "VType")) == -1) // -1 если нельзя отобразить поле. Иначе отображается поле.
                    {
                        DrawCustomClassMessage(valueRect, (string)GetVarValue(instance, "VType"));  //Если класс в VType кастомный то метод
                    }
                    DrawSeparator(position);
                    if (DrawField(keyRect, (string)GetVarValue(instance, "KType"), true) == -1)
                    {
                        DrawCustomClassMessage(keyRect, (string)GetVarValue(instance, "KType")); //Если класс в VType кастомный то метод
                    }
                    if (GUI.Button(btnRect, "+")) //Кнопка добавления в список
                    {
                        if (!customClass)
                        {
                            if (newK != null && newV != null)
                            {
                                CallMethod(instance, "Add", newK, newV); //Добавление жлемента
                            }
                            else
                            {
                                Debug.LogError("Dictionary key null error");
                            }
                        }
                        else
                        {
                            CallMethod(instance, "AddNull", GetInstanceFromTypeName(keyRect, "KType", newK), GetInstanceFromTypeName(valueRect, "VType", newV)); //Добавление пустого элемента. 
                        }
                    }

                    AddHeight();
                    AddHeight(5);
                #endregion

                //Поиск ключей и значений в SerializedProperty из OnGUI 
                #region FindValues
                SerializedProperty keys = null;
                SerializedProperty value = null;
                while (property.Next(true))
                {
                    if (property.isArray)
                    {
                        if (property.name == "_keys")
                        {
                            keys = property.Copy();
                        }
                        if (property.name == "_values")
                        {
                            value = property.Copy();
                        }
                    }
                }

                #endregion


                //Отрисовка значений
                #region DrawValues
                drawList = DrawFoldoutLabel(position, drawList, "List"); //Открытие списка значений
                if (drawList)
                {
                    position = AddOffcet(position); //Смещение
                    DefineSizes(position);

                    for (int i = 0; i < keys.arraySize; i++) //Иду по каждому элементу
                    {
                        if (GUI.Button(btnRect, "-"))
                        {
                            CallMethod(instance, "RemoveID", i); //Удаление
                        }

                        var currentV = new Rect(valueRect);
                        var currentK = new Rect(keyRect);

                        float offcetK = 0;
                        float offcetV = 0;
                        offcetV = DrawField(currentV, value.GetArrayElementAtIndex(i)); //Отрисовка поля и получение его высоты
                        offcetK = DrawField(currentK, keys.GetArrayElementAtIndex(i), true);
                        DrawSeparator(position, Mathf.Max(offcetK, offcetV));
                        AddHeightC(Mathf.Max(offcetK, offcetV) + 5); //Вычисляю самоу большую высоту. Так как у ключа и у значения высота поля может быть разная я ищу максимальную.
                    }
                    if (keys.arraySize == 0) //Если обьектов нет.
                    {
                        AddHeight();
                        EditorGUI.LabelField(new Rect(new Vector2(position.x, y), new Vector2(position.width, startY)), "List is empty..."); //Пишу лейбл что список пуст.
                    }
                }

                #endregion
            }

            AddHeight();
            EditorGUI.indentLevel = indent;
        }
        EditorGUI.EndProperty();
    }

    #region DrawUtility

    /// <summary>
    /// Если класс нельзя отобразить в Unity без Property, пишу уведоление.
    /// </summary>
    public void DrawCustomClassMessage(Rect rect, string keyType)
    {
        GUI.enabled = false;
        EditorGUI.TextField(rect, "Type \"" + keyType + "\" mapped only in list");
        GUI.enabled = true;
        customClass = true;
    }

    /// <summary>
    /// Получение общей величины Drawer`a. 
    /// </summary>
    public float GetPropertyHeight(SerializedProperty property, GUIContent label, bool get)
    {
        return base.GetPropertyHeight(property, label);
    }

    /// <summary>
    /// Каждое поле при отрисовке обращается к GetPropertyHeight чтобы узнать на какой высоте рисоваться. Я же возвращаю своё значение Y которое моя высота. 
    /// </summary>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return y;
    }

    /// <summary>
    /// Добавить высоту.
    /// </summary>
    /// <param name="add">Добавочная величена к величене строки</param>
    public void AddHeight(float add = 0)
    {
        y += startY + add;
        keyRect.y += startY + add;
        valueRect.y += startY + add;
        btnRect.y += startY + add;
    }

    /// <summary>
    /// Добавить высоту.
    /// </summary>
    /// <param name="add">Высота которую надо добавить</param>
    public void AddHeightC(float add = 0)
    {
        y += add;
        keyRect.y += add;
        valueRect.y += add;
        btnRect.y += add;
    }

    /// <summary>
    /// Написовать разделитель.
    /// </summary>
    public void DrawSeparator(Rect position)
    {
        DrawSeparator(position, startY);
    }

    /// <summary>
    /// Написовать разделитель.
    /// </summary>
    public void DrawSeparator(Rect position, float height)
    {
        EditorGUI.DrawRect(new Rect(new Vector2(position.x + fieldWidth + (separatorWidth / 2f) - 1.5f, startY + y), new Vector2(3, height - 1)), Color.gray);
    }
    
    /// <summary>
    /// Определить размеры полей.
    /// </summary>
    public void DefineSizes(Rect position)
    {
        fieldWidth = (position.width * 0.45f);
        separatorWidth = (position.width * 0.04f);
        btnWidth = (position.width * 0.06f);


        keyRect = new Rect(new Vector2(position.x, startY + y), new Vector2(fieldWidth, startY));
        valueRect = new Rect(new Vector2(position.x + fieldWidth + separatorWidth, startY + y), new Vector2(fieldWidth, startY));
        btnRect = new Rect(new Vector2(position.x + fieldWidth + separatorWidth + fieldWidth, startY + y), new Vector2(btnWidth, startY));
    }
    #endregion

    #region DrawFields

    /// <summary>
    /// Открытие закрытие списков. (Стрелочка как в массивах или обьектах)
    /// </summary>
    public bool DrawFoldoutLabel(Rect position, bool toggle, string text)
    {
        if (GUI.Button(new Rect(new Vector2(position.x - 20, y), new Vector2(position.width + 20, startY)), "", GUIStyle.none))
        {
            toggle = !toggle;
        }
        EditorGUI.Foldout(new Rect(new Vector2(position.x, y), new Vector2(fieldWidth, startY)), toggle, text);
        return toggle;
    }

    /// <summary>
    /// Отображение поля через SerializedProperty
    /// </summary>
    /// <returns>Высота объекта</returns>
    public float DrawField(Rect position, SerializedProperty item, bool isKey = false)
    {
        float yAdd = startY;
        switch (item.propertyType)
        {
            case SerializedPropertyType.Generic:
                EditorGUI.PropertyField(position, item, new GUIContent("Serialized: " + item.type), true);
                if (item.isExpanded)
                {
                    yAdd = EditorGUI.GetPropertyHeight(item, true) + 10;
                }
                break;
            case SerializedPropertyType.Integer:
                item.intValue = EditorGUI.IntField(position, item.intValue);
                break;
            case SerializedPropertyType.Boolean:
                item.boolValue = EditorGUI.Toggle(position, item.boolValue);
                break;
            case SerializedPropertyType.Float:
                item.floatValue = EditorGUI.FloatField(position, item.floatValue);
                break;
            case SerializedPropertyType.String:
                item.stringValue = EditorGUI.TextField(position, item.stringValue);
                break;
            case SerializedPropertyType.Color:
                item.colorValue = EditorGUI.ColorField(position, item.colorValue);
                break;
            case SerializedPropertyType.ObjectReference:
                if (item.objectReferenceValue != null)
                {
                    GUI.enabled = true;
                }
                item.objectReferenceValue = EditorGUI.ObjectField(position, item.objectReferenceValue, typeof(object), true);
                GUI.enabled = true;
                break;
            case SerializedPropertyType.Vector2:
                item.vector2Value = EditorGUI.Vector2Field(position, "", item.vector2Value);
                break;
            case SerializedPropertyType.Vector3:
                item.vector3Value = EditorGUI.Vector3Field(position, "", item.vector3Value);
                break;
            case SerializedPropertyType.Vector4:
                item.vector4Value = EditorGUI.Vector4Field(position, "", item.vector4Value);
                break;
            case SerializedPropertyType.Rect:
                item.rectValue = EditorGUI.RectField(position, "", item.rectValue);
                AddHeight();
                AddHeight();
                break;
            case SerializedPropertyType.Quaternion:
                item.quaternionValue = Quaternion.Euler(EditorGUI.Vector3Field(position, "", item.quaternionValue.eulerAngles));
                break;
            case SerializedPropertyType.Vector2Int:
                item.vector2IntValue = EditorGUI.Vector2IntField(position, "", item.vector2IntValue);
                break;
            case SerializedPropertyType.Vector3Int:
                item.vector3IntValue = EditorGUI.Vector3IntField(position, "", item.vector3IntValue);
                break;
            case SerializedPropertyType.RectInt:
                item.rectIntValue = EditorGUI.RectIntField(position, "", item.rectIntValue);
                AddHeight();
                AddHeight();
                break;
            default:
                EditorGUI.LabelField(position, "Type not supported");
                break;
        }
        CallMethod(instance, "UpdateDictionary");

        return yAdd;

    }

    /// <summary>
    /// Отображение поля через Тип
    /// </summary>
    /// <returns>Высота объекта (Если -1 то это кстомный класс)</returns>
    public float DrawField(Rect position, string typeName, bool isKey = false)
    {
        object value = new object();
        float yAdd = 0;
        if (isKey)
        {
            value = newK;
        }
        else
        {
            value = newV;
        }
        if (IsUnityClassObject(typeName))
        {
            if (typeName.Contains("Int"))
            {
                value = EditorGUI.IntField(position, Convert.ToInt32(value));
            }
            else if (typeName.Contains("Boolean"))
            {
                value = EditorGUI.Toggle(position, Convert.ToBoolean(value));
            }
            else if (typeName.Contains("Single")) //Float
            {
                value = EditorGUI.FloatField(position, Convert.ToSingle(value));
            }
            else if (typeName.Contains("String"))
            {
                value = EditorGUI.TextField(position, Convert.ToString(value));
            }
            else if (typeName.Contains("Color"))
            {
                value = EditorGUI.ColorField(position, value != null ? Converter<Color>(value) : new Color());
            }
            else if (typeName.Contains("Object"))
            {
                value = EditorGUI.ObjectField(position, (value is UnityEngine.Object ? (UnityEngine.Object)value : null), typeof(object), true);
            }
            else if (typeName.Contains("Vector2"))
            {
                value = EditorGUI.Vector2Field(position, "", value != null ? Converter<Vector2>(value) : new Vector2());
            }
            else if (typeName.Contains("Vector3"))
            {
                value = EditorGUI.Vector3Field(position, "", value != null ? Converter<Vector3>(value) : new Vector3());
            }
            else if (typeName.Contains("Vector4"))
            {
                value = EditorGUI.Vector4Field(position, "", value != null ? Converter<Vector4>(value) : new Vector4());
            }
            else if (typeName.Contains("Rect"))
            {
                value = EditorGUI.RectField(position, "", value != null ? Converter<Rect>(value) : new Rect());
                AddHeight();
                AddHeight();
            }
            else if (typeName.Contains("Quaternion"))
            {
                var quaternion = value != null ? Converter<Quaternion>(value) : new Quaternion();
                value = Quaternion.Euler(EditorGUI.Vector3Field(position, "", quaternion.eulerAngles));
            }
            else if (typeName.Contains("Vector2Int"))
            {
                value = EditorGUI.Vector2IntField(position, "", value != null ? Converter<Vector2Int>(value) : new Vector2Int());
            }
            else if (typeName.Contains("Vector3Int"))
            {
                value = EditorGUI.Vector3Field(position, "", value != null ? Converter<Vector3Int>(value) : new Vector3());
            }
            else if (typeName.Contains("RectInt"))
            {
                value = EditorGUI.RectIntField(position, value != null ? Converter<RectInt>(value) : new RectInt());
                AddHeight();
                AddHeight();
            }
        }
        else
        {
            return -1;
        }


        if (isKey)
        {
            newK = value;
        }
        else
        {
            newV = value;
        }

        return yAdd;
    }
    #endregion

    #region Other

    /// <summary>
    /// Довить смещение вправо на 10.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Rect AddOffcet(Rect position)
    {
        position.x += 10;
        position.width -= 10;

        return position;
    }

    

    /// <summary>
    /// Конвертирует обьект в класс.
    /// </summary>
    public T Converter<T>(object value)
    {
        return (T)Convert.ChangeType(value, typeof(T));
    }
    /// <summary>
    /// Уведомление о том что класс не сериализуемый.
    /// </summary>
    public void NotSerializbleError(Rect position, string keyType)
    {
        EditorGUI.LabelField(new Rect(position.x, y + startY, position.width, startY), "Add [System.Serializable] to class \"" + GetVarValue(instance, keyType).ToString() + "\"");
    }
    #endregion


}


