using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Template.Attributes
{
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
}