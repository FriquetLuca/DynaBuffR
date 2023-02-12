using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace DynaBuffR;
public static class ClassManipulator {
    public static IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo, bool> predicate) {
        List<Type> types = new List<Type>() {
            target.GetType()
        };
        while (types.Last().BaseType != null) {
#pragma warning disable CS8604
            types.Add(types.Last().BaseType);
#pragma warning restore CS8604
        }
        for (int i = types.Count - 1; i >= 0; i--) {
            IEnumerable<FieldInfo> fieldInfos = types[i].GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(predicate);
            foreach (FieldInfo fieldInfo in fieldInfos) {
                yield return fieldInfo;
            }
        }
    }
    
}