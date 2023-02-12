using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DynaBuffR;
public static class Injector {
    delegate object ExtractDelegate(ref int offset, byte[] byteArray);
    // Todo: System.Int32[] and others
    private static Dictionary<string, ExtractDelegate> fieldInjector = new Dictionary<string, ExtractDelegate>() {
        { "System.SByte", Injector.ExtractSByte },
        { "System.Byte", Injector.ExtractByte },
        { "System.Int16", Injector.ExtractInt16 },
        { "System.UInt16", Injector.ExtractUInt16 },
        { "System.Int32", Injector.ExtractInt32 },
        { "System.UInt32", Injector.ExtractUInt32 },
        { "System.Int64", Injector.ExtractInt64 },
        { "System.UInt64", Injector.ExtractUInt64 },
        { "System.Single", Injector.ExtractSingle },
        { "System.Double", Injector.ExtractDouble },
        { "System.Decimal", Injector.ExtractDecimal },
        { "System.Char", Injector.ExtractChar },
        { "System.String", Injector.ExtractString }
    };
    /// <summary>
    /// Extract a SByte in a byte array from an offset. Once extracted, it increment the offset by 1 since a SByte is 1 bytes.
    /// </summary>
    /// <param name="offset">The offset at which the SByte is written.</param>
    /// <param name="byteArray">The array of bytes containing the SByte to extract.</param>
    /// <returns>The extracted SByte from the array of bytes.</returns>
    public static object ExtractSByte(ref int offset, byte[] byteArray) {
        sbyte currentValue = (sbyte)byteArray[offset];
        offset += sizeof(sbyte);
        return currentValue;
    }
    /// <summary>
    /// Extract a Byte in a byte array from an offset. Once extracted, it increment the offset by 1 since a Byte is 1 bytes.
    /// </summary>
    /// <param name="offset">The offset at which the Byte is written.</param>
    /// <param name="byteArray">The array of bytes containing the Byte to extract.</param>
    /// <returns>The extracted Byte from the array of bytes.</returns>
    public static object ExtractByte(ref int offset, byte[] byteArray) {
        byte currentValue = byteArray[offset];
        offset += sizeof(byte);
        return currentValue;
    }
    /// <summary>
    /// Extract a Int16 in a byte array from an offset. Once extracted, it increment the offset by 2 since a Int16 is 2 bytes.
    /// </summary>
    /// <param name="offset">The offset at which the Int16 is written.</param>
    /// <param name="byteArray">The array of bytes containing the Int16 to extract.</param>
    /// <returns>The extracted Int16 from the array of bytes.</returns>
    public static object ExtractInt16(ref int offset, byte[] byteArray) {
        short currentValue = BitConverter.ToInt16(byteArray, offset);
        offset += sizeof(short);
        return currentValue;
    }
    /// <summary>
    /// Extract a UInt16 in a byte array from an offset. Once extracted, it increment the offset by 2 since a UInt16 is 2 bytes.
    /// </summary>
    /// <param name="offset">The offset at which the UInt16 is written.</param>
    /// <param name="byteArray">The array of bytes containing the UInt16 to extract.</param>
    /// <returns>The extracted UInt16 from the array of bytes.</returns>
    public static object ExtractUInt16(ref int offset, byte[] byteArray) {
        ushort currentValue = BitConverter.ToUInt16(byteArray, offset);
        offset += sizeof(ushort);
        return currentValue;
    }
    /// <summary>
    /// Extract a Int32 in a byte array from an offset. Once extracted, it increment the offset by 4 since a Int32 is 4 bytes.
    /// </summary>
    /// <param name="offset">The offset at which the Int32 is written.</param>
    /// <param name="byteArray">The array of bytes containing the Int32 to extract.</param>
    /// <returns>The extracted Int32 from the array of bytes.</returns>
    public static object ExtractInt32(ref int offset, byte[] byteArray) {
        int currentValue = BitConverter.ToInt32(byteArray, offset);
        offset += sizeof(int);
        return currentValue;
    }
    /// <summary>
    /// Extract a UInt32 in a byte array from an offset. Once extracted, it increment the offset by 4 since a UInt32 is 4 bytes.
    /// </summary>
    /// <param name="offset">The offset at which the UInt32 is written.</param>
    /// <param name="byteArray">The array of bytes containing the UInt32 to extract.</param>
    /// <returns>The extracted UInt32 from the array of bytes.</returns>
    public static object ExtractUInt32(ref int offset, byte[] byteArray) {
        uint currentValue = BitConverter.ToUInt32(byteArray, offset);
        offset += sizeof(uint);
        return currentValue;
    }
    /// <summary>
    /// Extract a Int64 in a byte array from an offset. Once extracted, it increment the offset by 8 since a Int64 is 8 bytes.
    /// </summary>
    /// <param name="offset">The offset at which the Int64 is written.</param>
    /// <param name="byteArray">The array of bytes containing the Int64 to extract.</param>
    /// <returns>The extracted Int64 from the array of bytes.</returns>
    public static object ExtractInt64(ref int offset, byte[] byteArray) {
        long currentValue = BitConverter.ToInt64(byteArray, offset);
        offset += sizeof(long);
        return currentValue;
    }
    /// <summary>
    /// Extract a UInt64 in a byte array from an offset. Once extracted, it increment the offset by 8 since a UInt64 is 8 bytes.
    /// </summary>
    /// <param name="offset">The offset at which the UInt64 is written.</param>
    /// <param name="byteArray">The array of bytes containing the UInt64 to extract.</param>
    /// <returns>The extracted UInt64 from the array of bytes.</returns>
    public static object ExtractUInt64(ref int offset, byte[] byteArray) {
        ulong currentValue = BitConverter.ToUInt64(byteArray, offset);
        offset += sizeof(ulong);
        return currentValue;
    }
    /// <summary>
    /// Extract a Float in a byte array from an offset. Once extracted, it increment the offset by 4 since a Float is 4 bytes.
    /// </summary>
    /// <param name="offset">The offset at which the Float is written.</param>
    /// <param name="byteArray">The array of bytes containing the Float to extract.</param>
    /// <returns>The extracted Float from the array of bytes.</returns>
    public static object ExtractSingle(ref int offset, byte[] byteArray) {
        float currentValue = BitConverter.ToSingle(byteArray, offset);
        offset += sizeof(float);
        return currentValue;
    }
    /// <summary>
    /// Extract a Double in a byte array from an offset. Once extracted, it increment the offset by 8 since a Double is 8 bytes.
    /// </summary>
    /// <param name="offset">The offset at which the Double is written.</param>
    /// <param name="byteArray">The array of bytes containing the Double to extract.</param>
    /// <returns>The extracted Double from the array of bytes.</returns>
    public static object ExtractDouble(ref int offset, byte[] byteArray) {
        double currentValue = BitConverter.ToDouble(byteArray, offset);
        offset += sizeof(double);
        return currentValue;
    }
    /// <summary>
    /// Extract a Decimal in a byte array from an offset. Once extracted, it increment the offset by 16 since a Decimal is 16 bytes.
    /// </summary>
    /// <param name="offset">The offset at which the Decimal is written.</param>
    /// <param name="byteArray">The array of bytes containing the Decimal to extract.</param>
    /// <returns>The extracted Decimal from the array of bytes.</returns>
    public static object ExtractDecimal(ref int offset, byte[] byteArray) {
        return new decimal(
            new int[4] {
                (int)Injector.ExtractInt32(ref offset, byteArray),
                (int)Injector.ExtractInt32(ref offset, byteArray),
                (int)Injector.ExtractInt32(ref offset, byteArray),
                (int)Injector.ExtractInt32(ref offset, byteArray)
            }
        );
    }
    /// <summary>
    /// Extract a Char in a byte array from an offset. Once extracted, it increment the offset by 2 since a Char is 2 bytes.
    /// </summary>
    /// <param name="offset">The offset at which the Char is written.</param>
    /// <param name="byteArray">The array of bytes containing the Char to extract.</param>
    /// <returns>The extracted Char from the array of bytes.</returns>
    public static object ExtractChar(ref int offset, byte[] byteArray) {
        char currentValue = BitConverter.ToChar(byteArray, offset);
        offset += sizeof(char);
        return currentValue;
    }
    /// <summary>
    /// Extract a String in a byte array from an offset. Once extracted, it increment the offset by all bytes that composed the string.
    /// </summary>
    /// <param name="offset">The offset at which the String is written.</param>
    /// <param name="byteArray">The array of bytes containing the String to extract.</param>
    /// <returns>The extracted String from the array of bytes.</returns>
    public static object ExtractString(ref int offset, byte[] byteArray) {
        int stringLength = (int)Injector.ExtractInt32(ref offset, byteArray);
        byte[] stringBuffer = byteArray.Skip(offset).Take(stringLength).ToArray();
        offset += stringLength;
        return Encoding.UTF8.GetString(stringBuffer);
    }
    public static void InjectFieldValue<T>(ref int offset, byte[] obj, string fieldType, FieldInfo field, T objInstance) {
        if(fieldInjector.ContainsKey(fieldType)) {
            ExtractDelegate? value = fieldInjector.GetValueOrDefault(fieldType);
            if(value is not null) {
                field.SetValue(objInstance, value(ref offset, obj));
            }
        }
    }
    public static T CreateInstance<T>(ref int offset, byte[] obj) {
        int bufferTypeHash = (int)Injector.ExtractInt32(ref offset, obj);
        Type[] result = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Name.GetHashCode() == bufferTypeHash).ToArray();
        if(result.Length > 0) {
            Type? objType = Type.GetType(result[0].Name);
#pragma warning disable CS8600
#pragma warning disable CS8604
            T? objInstance = (T)Activator.CreateInstance(objType);
#pragma warning restore CS8604
#pragma warning restore CS8600
#pragma warning disable CS8604
            IEnumerable<FieldInfo>? objInstanceType = ClassManipulator.GetAllFields(objInstance, (FieldInfo info) => true);
#pragma warning restore CS8604
            int objectLength = (int)Injector.ExtractInt32(ref offset, obj);
            int objectMaxOffset = offset + objectLength;
            // Check the max offset of the object and the buffer bounds
            while(offset < objectMaxOffset && offset < obj.Length) {
                var fieldHash = (int)Injector.ExtractInt32(ref offset, obj);
                var fieldTypeLength = (int)Injector.ExtractInt32(ref offset, obj);
                var fieldTypeBuffer = obj.Skip(offset).Take(fieldTypeLength).ToArray();
                var fieldType = Encoding.UTF8.GetString(fieldTypeBuffer);
                offset += fieldTypeLength;
                var field = objInstanceType.FirstOrDefault(f => f.Name.GetHashCode() == fieldHash);
                if(field == null) {
                    continue;
                }
                Injector.InjectFieldValue<T>(ref offset, obj, fieldType, field, objInstance);
            }
            return objInstance;
        }
#pragma warning disable CS8600
#pragma warning disable CS8603
#pragma warning disable CS8604
        return (T)Activator.CreateInstance(Type.GetType(typeof(T).ToString()));
#pragma warning restore CS8604
#pragma warning restore CS8603
#pragma warning restore CS8600
    }
}