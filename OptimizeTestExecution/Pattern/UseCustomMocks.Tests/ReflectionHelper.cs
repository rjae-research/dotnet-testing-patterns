using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UseCustomMocks.Tests
{
    public class ReflectionHelper
    {
        public static T GetField<T>(object source, string fieldName)
        {
            FieldInfo info = GetFieldInfo(source, fieldName);
            if (ReferenceEquals(info, null))
                throw new Exception($"Can't find field '{fieldName}' in {source}");
            return (T) info.GetValue(source);
        }

        public static T GetField<T>(Type type, object source, string fieldName)
        {
            FieldInfo info = GetFieldInfo(type, fieldName);
            if (ReferenceEquals(info, null))
                throw new Exception($"Can't find field '{fieldName}' in {type}");
            return (T) info.GetValue(source);
        }

        public static T GetProperty<T>(object source, string propertyName)
        {
            PropertyInfo info = GetPropertyInfo(source, propertyName);
            if (ReferenceEquals(info, null))
                throw new Exception($"Can't find property '{propertyName}' in {source}");
            return (T) info.GetValue(source, null);
        }

        public static T InvokeConstructor<T>(params object[] arguments)
        {
            List<Type> types = arguments.Select(o => o.GetType()).ToList();
            return (T) InvokeConstructor(typeof(T), types.Count > 0 ? types.ToArray() : null, arguments);
        }

        public static object InvokeConstructor(Type type, Type[] constructorTypesInOrder, object[] constructorArguments)
        {
            return type.GetConstructor(BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, constructorTypesInOrder ?? Type.EmptyTypes, null)?.Invoke(constructorArguments);
        }

        public static object InvokeMethod(object source, string methodName, params object[] arguments)
        {
            return InvokeMethod(source.GetType(), source, methodName, arguments);
        }

        public static object InvokeMethod(Type type, object source, string methodName, object[] arguments)
        {
            return type.InvokeMember(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.InvokeMethod, null, source, arguments);
        }

        public static void SetField(object source, string fieldName, object fieldValue)
        {
            FieldInfo info = GetFieldInfo(source, fieldName);
            if (ReferenceEquals(info, null))
                throw new Exception($"Can't find field '{fieldName}' in {source}");
            info.SetValue(source, fieldValue);
        }

        public static void SetField(Type type, object source, string fieldName, object fieldValue)
        {
            FieldInfo info = GetFieldInfo(type, fieldName);
            if (ReferenceEquals(info, null))
                throw new Exception($"Can't find field '{fieldName}' in {type}");
            info.SetValue(source, fieldValue);
        }

        public static void SetProperty(object source, string propertyName, object propertyValue)
        {
            PropertyInfo info = GetPropertyInfo(source, propertyName);
            info.SetValue(source, propertyValue, null);
        }

        protected static FieldInfo GetFieldInfo(object source, string fieldName)
        {
            return GetFieldInfo(source.GetType(), fieldName);
        }

        protected static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            return type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }

        protected static PropertyInfo GetPropertyInfo(object source, string propertyName)
        {
            return source.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }
    }
}