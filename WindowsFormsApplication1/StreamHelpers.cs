/* Copyright (c) 2012 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Gibbed.IO
{
    public static partial class StreamHelpers
    {
        internal static bool ShouldSwap(Endian endian)
        {
            switch (endian)
            {
                case Endian.Little: return BitConverter.IsLittleEndian == false;
                case Endian.Big: return BitConverter.IsLittleEndian == true;
                default: throw new ArgumentException("unsupported endianness", "endian");
            }
        }

        public static MemoryStream ReadToMemoryStream(this Stream stream, long size, int buffer)
        {
            var memory = new MemoryStream();

            long left = size;
            var data = new byte[buffer];
            while (left > 0)
            {
                var block = (int)(Math.Min(left, data.Length));
                if (stream.Read(data, 0, block) != block)
                {
                    throw new EndOfStreamException();
                }
                memory.Write(data, 0, block);
                left -= block;
            }

            memory.Seek(0, SeekOrigin.Begin);
            return memory;
        }

        public static MemoryStream ReadToMemoryStream(this Stream stream, long size)
        {
            return stream.ReadToMemoryStream(size, 0x40000);
        }

        public static void WriteFromStream(this Stream stream, Stream input, long size, int buffer)
        {
            long left = size;
            var data = new byte[buffer];
            while (left > 0)
            {
                var block = (int)(Math.Min(left, data.Length));
                if (input.Read(data, 0, block) != block)
                {
                    throw new EndOfStreamException();
                }
                stream.Write(data, 0, block);
                left -= block;
            }
        }

        public static void WriteFromStream(this Stream stream, Stream input, long size)
        {
            stream.WriteFromStream(input, size, 0x40000);
        }

        public static byte[] ReadBytes(this Stream stream, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            var data = new byte[length];
            var read = stream.Read(data, 0, length);
            if (read != length)
            {
                throw new EndOfStreamException();
            }

            return data;
        }

        public static byte[] ReadBytes(this Stream stream, uint length)
        {
            return stream.ReadBytes((int)length);
        }

        public static void WriteBytes(this Stream stream, byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }

        #region ReadValueU16
        public static UInt16 ReadValueU16(this Stream stream)
        {
            return stream.ReadValueU16(Endian.Little);
        }

        public static UInt16 ReadValueU16(this Stream stream, Endian endian)
        {
            var data = stream.ReadBytes(2);
            var value = BitConverter.ToUInt16(data, 0);

            if (ShouldSwap(endian) == true)
            {
                value = value.Swap();
            }

            return value;
        }
        #endregion

        #region WriteValueU16
        public static void WriteValueU16(this Stream stream, UInt16 value)
        {
            stream.WriteValueU16(value, Endian.Little);
        }

        public static void WriteValueU16(this Stream stream, UInt16 value, Endian endian)
        {
            if (ShouldSwap(endian) == true)
            {
                value = value.Swap();
            }

            var data = BitConverter.GetBytes(value);
            Debug.Assert(data.Length == 2);
            stream.WriteBytes(data);
        }
        #endregion

        #region ReadValueS16
        public static Int16 ReadValueS16(this Stream stream)
        {
            return stream.ReadValueS16(Endian.Little);
        }

        public static Int16 ReadValueS16(this Stream stream, Endian endian)
        {
            var data = stream.ReadBytes(2);
            var value = BitConverter.ToInt16(data, 0);

            if (ShouldSwap(endian) == true)
            {
                value = value.Swap();
            }

            return value;
        }
        #endregion

        #region WriteValueS16
        public static void WriteValueS16(this Stream stream, Int16 value)
        {
            stream.WriteValueS16(value, Endian.Little);
        }

        public static void WriteValueS16(this Stream stream, Int16 value, Endian endian)
        {
            if (ShouldSwap(endian) == true)
            {
                value = value.Swap();
            }

            var data = BitConverter.GetBytes(value);
            Debug.Assert(data.Length == 2);
            stream.WriteBytes(data);
        }
        #endregion



        #region ReadValueS32
        public static Int32 ReadValueS32(this Stream stream)
        {
            return stream.ReadValueS32(Endian.Little);
        }

        public static Int32 ReadValueS32(this Stream stream, Endian endian)
        {
            var data = stream.ReadBytes(4);
            var value = BitConverter.ToInt32(data, 0);

            if (ShouldSwap(endian) == true)
            {
                value = value.Swap();
            }

            return value;
        }
        #endregion

        #region WriteValueS32
        public static void WriteValueS32(this Stream stream, Int32 value)
        {
            stream.WriteValueS32(value, Endian.Little);
        }

        public static void WriteValueS32(this Stream stream, Int32 value, Endian endian)
        {
            if (ShouldSwap(endian) == true)
            {
                value = value.Swap();
            }

            var data = BitConverter.GetBytes(value);
            Debug.Assert(data.Length == 4);
            stream.WriteBytes(data);
        }
        #endregion

        #region ReadValueU32
        public static UInt32 ReadValueU32(this Stream stream)
        {
            return stream.ReadValueU32(Endian.Little);
        }

        public static UInt32 ReadValueU32(this Stream stream, Endian endian)
        {
            var data = stream.ReadBytes(4);
            var value = BitConverter.ToUInt32(data, 0);

            if (ShouldSwap(endian) == true)
            {
                value = value.Swap();
            }

            return value;
        }
        #endregion

        #region WriteValueU32
        public static void WriteValueU32(this Stream stream, UInt32 value)
        {
            stream.WriteValueU32(value, Endian.Little);
        }

        public static void WriteValueU32(this Stream stream, UInt32 value, Endian endian)
        {
            if (ShouldSwap(endian) == true)
            {
                value = value.Swap();
            }

            var data = BitConverter.GetBytes(value);
            Debug.Assert(data.Length == 4);
            stream.WriteBytes(data);
        }
        #endregion

        public static byte ReadValueU8(this Stream stream)
        {
            return (byte)stream.ReadByte();
        }

        public static void WriteValueU8(this Stream stream, byte value)
        {
            stream.WriteByte(value);
        }
        #region ReadValueF32
        public static Single ReadValueF32(this Stream stream)
        {
            return stream.ReadValueF32(Endian.Little);
        }

        public static Single ReadValueF32(this Stream stream, Endian endian)
        {
            var data = stream.ReadBytes(4);

            if (ShouldSwap(endian) == true)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(BitConverter.ToInt32(data, 0).Swap()), 0);
            }

            return BitConverter.ToSingle(data, 0);
        }
        #endregion

        #region WriteValueF32
        public static void WriteValueF32(this Stream stream, Single value)
        {
            stream.WriteValueF32(value, Endian.Little);
        }

        public static void WriteValueF32(this Stream stream, Single value, Endian endian)
        {
            byte[] data = ShouldSwap(endian) == true
                              ? BitConverter.GetBytes(BitConverter.ToInt32(BitConverter.GetBytes(value), 0).Swap())
                              : BitConverter.GetBytes(value);
            Debug.Assert(data.Length == 4);
            stream.WriteBytes(data);
        }
        #endregion
        public static Encoding DefaultEncoding = Encoding.ASCII;
        public static string ReadString(this Stream stream, Endian endian)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            var length = stream.ReadValueU16(endian);
            return stream.ReadString(length, true, Encoding.ASCII);
        }

        public static void WriteString(this Stream stream, string value, Endian endian)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var length = value.Length;
            if (length > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException("value", "string is too long");
            }

            stream.WriteValueU16((ushort)length, endian);
            stream.WriteString(value, (uint)length, Encoding.ASCII);
        }
        public static string ReadString(this Stream stream, uint size)
        {
            return stream.ReadStringInternalStatic(DefaultEncoding, size, false);
        }

        public static string ReadString(this Stream stream, uint size, bool trailingNull)
        {
            return stream.ReadStringInternalStatic(DefaultEncoding, size, trailingNull);
        }

        public static string ReadStringZ(this Stream stream)
        {
            return stream.ReadStringInternalDynamic(DefaultEncoding, '\0');
        }

        public static void WriteString(this Stream stream, string value)
        {
            stream.WriteStringInternalStatic(DefaultEncoding, value);
        }

        public static void WriteString(this Stream stream, string value, uint size)
        {
            stream.WriteStringInternalStatic(DefaultEncoding, value, size);
        }

        public static void WriteStringZ(this Stream stream, string value)
        {
            stream.WriteStringInternalDynamic(DefaultEncoding, value, '\0');
        }

        public static string ReadString(this Stream stream, uint size, Encoding encoding)
        {
            return stream.ReadStringInternalStatic(encoding, size, false);
        }

        public static string ReadString(this Stream stream, int size, Encoding encoding)
        {
            return stream.ReadStringInternalStatic(encoding, (uint)size, false);
        }

        public static string ReadString(this Stream stream, uint size, bool trailingNull, Encoding encoding)
        {
            return stream.ReadStringInternalStatic(encoding, size, trailingNull);
        }

        public static string ReadString(this Stream stream, int size, bool trailingNull, Encoding encoding)
        {
            return stream.ReadStringInternalStatic(encoding, (uint)size, trailingNull);
        }

        public static string ReadStringZ(this Stream stream, Encoding encoding)
        {
            return stream.ReadStringInternalDynamic(encoding, '\0');
        }

        public static void WriteString(this Stream stream, string value, Encoding encoding)
        {
            stream.WriteStringInternalStatic(encoding, value);
        }

        public static void WriteString(this Stream stream, string value, uint size, Encoding encoding)
        {
            stream.WriteStringInternalStatic(encoding, value, size);
        }

        public static void WriteStringZ(this Stream stream, string value, Encoding encoding)
        {
            stream.WriteStringInternalDynamic(encoding, value, '\0');
        }

        internal static string ReadStringInternalStatic(this Stream stream,
                                                        Encoding encoding,
                                                        uint size,
                                                        bool trailingNull)
        {
            var data = new byte[size];
            stream.Read(data, 0, data.Length);

            string value = encoding.GetString(data, 0, data.Length);

            if (trailingNull == true)
            {
                var position = value.IndexOf('\0');
                if (position >= 0)
                {
                    value = value.Substring(0, position);
                }
            }

            return value;
        }

        internal static void WriteStringInternalStatic(this Stream stream, Encoding encoding, string value)
        {
            byte[] data = encoding.GetBytes(value);
            stream.Write(data, 0, data.Length);
        }

        internal static void WriteStringInternalStatic(this Stream stream, Encoding encoding, string value, uint size)
        {
            byte[] data = encoding.GetBytes(value);
            Array.Resize(ref data, (int)size);
            stream.Write(data, 0, (int)size);
        }

        internal static string ReadStringInternalDynamic(this Stream stream, Encoding encoding, char end)
        {
            int characterSize = encoding.GetByteCount("e");
            Debug.Assert(characterSize == 1 || characterSize == 2 || characterSize == 4);
            string characterEnd = end.ToString(CultureInfo.InvariantCulture);

            int i = 0;
            var data = new byte[128 * characterSize];

            while (true)
            {
                if (i + characterSize > data.Length)
                {
                    Array.Resize(ref data, data.Length + (128 * characterSize));
                }

                int read = stream.Read(data, i, characterSize);
                Debug.Assert(read == characterSize);

                if (encoding.GetString(data, i, characterSize) == characterEnd)
                {
                    break;
                }

                i += characterSize;
            }

            if (i == 0)
            {
                return "";
            }

            return encoding.GetString(data, 0, i);
        }

        internal static void WriteStringInternalDynamic(this Stream stream, Encoding encoding, string value, char end)
        {
            byte[] data = encoding.GetBytes(value);
            stream.Write(data, 0, data.Length);

            data = encoding.GetBytes(end.ToString(CultureInfo.InvariantCulture));
            stream.Write(data, 0, data.Length);
        }
        public static bool ReadValueBoolean(this Stream stream)
        {
            return stream.ReadValueB8();
        }

        public static void WriteValueBoolean(this Stream stream, bool value)
        {
            stream.WriteValueB8(value);
        }

        public static bool ReadValueB8(this Stream stream)
        {
            return stream.ReadValueU8() > 0;
        }

        public static void WriteValueB8(this Stream stream, bool value)
        {
            stream.WriteValueU8((byte)(value == true ? 1 : 0));
        }

        public static bool ReadValueB32(this Stream stream, Endian endian)
        {
            return stream.ReadValueU32(endian) != 0;
        }

        public static bool ReadValueB32(this Stream stream)
        {
            return stream.ReadValueB32(Endian.Little);
        }

        public static void WriteValueB32(this Stream stream, bool value, Endian endian)
        {
            stream.WriteValueU32((byte)(value == true ? 1 : 0), endian);
        }

        public static void WriteValueB32(this Stream stream, bool value)
        {
            stream.WriteValueB32(value, Endian.Little);
        }
    }
}