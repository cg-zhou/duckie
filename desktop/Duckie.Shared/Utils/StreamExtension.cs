using System.IO;
using System.Runtime.InteropServices;

namespace Duckie.Shared.Utils;

public static class StreamExtension
{
    public static MemoryStream ToStream(this byte[] bytes)
    {
        return new MemoryStream(bytes);
    }

    public static void WriteFile(this byte[] bytes, string path)
    {
        File.WriteAllBytes(path, bytes);
    }

    public static void WriteStruct<TStruct>(this MemoryStream stream, TStruct value)
        where TStruct : struct
    {
        var bytes = value.StructToBytes();
        stream.Write(bytes, 0, bytes.Length);
    }

    public static byte[] StructToBytes<TStruct>(this TStruct value)
        where TStruct : struct
    {
        var size = Marshal.SizeOf(value);
        var bytes = new byte[size];
        var ptr = Marshal.AllocHGlobal(size);

        try
        {
            Marshal.StructureToPtr(value, ptr, false);
            Marshal.Copy(ptr, bytes, 0, size);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
        return bytes;
    }

    public static TStruct BytesToStruct<TStruct>(this byte[] value)
        where TStruct : struct
    {
        var size = Marshal.SizeOf(typeof(TStruct));
        var ptr = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.Copy(value, 0, ptr, size);
            return (TStruct)Marshal.PtrToStructure(ptr, typeof(TStruct));
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static TStruct ReadStruct<TStruct>(this Stream stream) where TStruct : struct
    {
        var size = Marshal.SizeOf(typeof(TStruct));
        var buffer = new byte[size];
        stream.Read(buffer, 0, size);
        return buffer.BytesToStruct<TStruct>();
    }
}
