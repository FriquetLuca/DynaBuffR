using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace DynaBuffR;
public static class Extractor {
    delegate void InjectDelegate(List<byte> buffer, object currentValue);
    // Todo: System.Int32[] and others
    private static Dictionary<string, InjectDelegate> fieldExtractor = new Dictionary<string, InjectDelegate>() {
        { "System.SByte", Extractor.InjectByte },
        { "System.Byte", Extractor.InjectByte },
        { "System.Int16", Extractor.InjectInt16 },
        { "System.UInt16", Extractor.InjectUInt16 },
        { "System.Int32", Extractor.InjectInt32 },
        { "System.UInt32", Extractor.InjectUInt32 },
        { "System.Int64", Extractor.InjectInt64 },
        { "System.UInt64", Extractor.InjectUInt64 },
        { "System.Single", Extractor.InjectSingle },
        { "System.Double", Extractor.InjectDouble },
        { "System.Decimal", Extractor.InjectDecimal },
        { "System.Char", Extractor.InjectChar },
        { "System.String", Extractor.InjectString }
    };
    private static void InjectString(List<byte> buffer, object currentValue) {
        byte[] strVal = Encoding.UTF8.GetBytes((string)currentValue);
        buffer.AddRange(BitConverter.GetBytes(strVal.Length));
        buffer.AddRange(strVal);
    }
    private static void InjectByte(List<byte> buffer, object currentValue) {
        buffer.Add((byte)currentValue);
    }
    private static void InjectInt16(List<byte> buffer, object currentValue) {
        buffer.AddRange(BitConverter.GetBytes((short)currentValue));
    }
    private static void InjectUInt16(List<byte> buffer, object currentValue) {
        buffer.AddRange(BitConverter.GetBytes((ushort)currentValue));
    }
    private static void InjectInt32(List<byte> buffer, object currentValue) {
        buffer.AddRange(BitConverter.GetBytes((int)currentValue));
    }
    private static void InjectUInt32(List<byte> buffer, object currentValue) {
        buffer.AddRange(BitConverter.GetBytes((uint)currentValue));
    }
    private static void InjectInt64(List<byte> buffer, object currentValue) {
        buffer.AddRange(BitConverter.GetBytes((long)currentValue));
    }
    private static void InjectUInt64(List<byte> buffer, object currentValue) {
        buffer.AddRange(BitConverter.GetBytes((ulong)currentValue));
    }
    private static void InjectSingle(List<byte> buffer, object currentValue) {
        buffer.AddRange(BitConverter.GetBytes((float)currentValue));
    }
    private static void InjectDouble(List<byte> buffer, object currentValue) {
        buffer.AddRange(BitConverter.GetBytes((double)currentValue));
    }
    private static void InjectDecimal(List<byte> buffer, object currentValue) {
        foreach(var p in decimal.GetBits((decimal)currentValue)) {
            buffer.AddRange(BitConverter.GetBytes(p));
        }
    }
    private static void InjectChar(List<byte> buffer, object currentValue) {
        buffer.AddRange(BitConverter.GetBytes((char)currentValue));
    }
    public static void WriteBufferField(List<byte> buffer, string fieldType, object fieldVal) {
        if(fieldExtractor.ContainsKey(fieldType)) {
            var value = fieldExtractor.GetValueOrDefault(fieldType);
            if(value is not null) {
                value(buffer, fieldVal);
            }
        }
    }
    public static byte[] ExtractBufferObject<T>(object obj) {
        List<byte> buffer = new List<byte>();
        List<byte> bufferType = new List<byte>();
        bufferType.AddRange(BitConverter.GetBytes(obj.GetType().ToString().GetHashCode()));
        foreach(var field in ClassManipulator.GetAllFields(obj, (FieldInfo info) => true)) {
            buffer.AddRange(BitConverter.GetBytes(field.Name.GetHashCode()));
            string fieldType = field.FieldType.ToString();
            byte[] fieldTypeBuffer = Encoding.UTF8.GetBytes(fieldType);
            buffer.AddRange(BitConverter.GetBytes(fieldTypeBuffer.Length));
            buffer.AddRange(fieldTypeBuffer);
            object? fieldVal = field.GetValue(obj);
            if(fieldVal is null) {
                continue;
            }
            Extractor.WriteBufferField(buffer, fieldType, fieldVal);
        }
        int objectLength = buffer.Count;
        bufferType.AddRange(BitConverter.GetBytes(objectLength));
        buffer.InsertRange(0, bufferType);
        return buffer.ToArray();
    }
}