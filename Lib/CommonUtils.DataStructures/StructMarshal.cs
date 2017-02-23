///-------------------------------------------------------------------------------------------------
// <date>20160302</date>
// <summary>Functionality for storing in DB as blobs for uniform object arrays</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DataStructures
{
    public class StructMarshal
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct DoubleStruct
        {
            [FieldOffset(0)]
            public Double value;
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        public struct CharStruct
        {
            [FieldOffset(0)]
            public Char value;
        }

        public static T ByteToStruct<T>(byte[] rawData, int position)
        {
            int rawsize = Marshal.SizeOf(typeof(T));
            if (rawsize > rawData.Length - position)
                throw new ArgumentException("Not enough data to fill struct. Array length from position: " + (rawData.Length - position) + ", Struct length: " + rawsize);
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.Copy(rawData, position, buffer, rawsize);
            T retobj = (T)Marshal.PtrToStructure(buffer, typeof(T));
            Marshal.FreeHGlobal(buffer);
            return retobj;
        }
        public static byte[] StructToByteArray<T>(T[] array)
        {
            int structSize = Marshal.SizeOf(typeof(T));
            int size = array.Length * structSize;
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            for (int i = 0; i < array.Length; i++)
                Marshal.StructureToPtr(array[i], ptr + i * structSize, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

    }
}
