using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Xb
{
    /// <summary>
    /// Byte, Bit functions
    /// バイト／ビット関数群
    /// </summary>
    /// <remarks></remarks>
    public class Byte
    {
        /// <summary>
        /// Get Base64-String from Byte Array
        /// バイト配列からBase64文字列を生成する。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetBase64String(byte[] bytes)
        {
            return System.Convert.ToBase64String(bytes);
        }


        /// <summary>
        /// Get Base64-String from Stream
        /// StreamからBase64文字列を生成する。
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetBase64String(System.IO.Stream stream)
        {
            return Xb.Byte.GetBase64String(Xb.Byte.GetBytes(stream));
        }


        /// <summary>
        /// Get Byte Array from Stream
        /// Streamからバイト配列を取得する。
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <remarks>
        /// thanks:
        /// http://www.atmarkit.co.jp/fdotnet/dotnettips/985memstream/memstream.html
        /// http://stackoverflow.com/questions/221925/creating-a-byte-array-from-a-stream
        /// </remarks>
        public static byte[] GetBytes(System.IO.Stream stream)
        {
            using (var memStream = new System.IO.MemoryStream())
            {
                //アホほど速い...くっそーしらんやった...。
                stream.CopyTo(memStream);
                return memStream.ToArray();
            }
        }


        /// <summary>
        /// Get Stream from Byte Array
        /// バイト配列からStreamを取得する。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static System.IO.Stream GetStream(byte[] bytes)
        {
            return (System.IO.Stream)(new System.IO.MemoryStream(bytes));
        }


        /// <summary>
        /// Get Stream from Base64-String
        /// Base64文字列からStreamを取得する。
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static System.IO.Stream GetStream(string base64String)
        {
            return Xb.Byte.GetStream(System.Convert.FromBase64String(base64String));
        }


        /// <summary>
        /// Get Bit-String from 1byte
        /// バイト値から二進数文字列を取得する。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetBitString(byte value)
        {
            return Convert.ToString(value, 2).PadLeft(8, '0');
        }


        /// <summary>
        /// Get Bit-String from integer(regarded as unsigned)
        /// 符号無し整数値から二進数文字列を取得する。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetBitString(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("for only unsigned integer");

            return Convert.ToString((uint)value, 2);
        }


        /// <summary>
        /// Byte Array Operator Class
        /// バイト配列操作クラス
        /// </summary>
        public class ByteArray
        {
            /// <summary>
            /// Byte Array
            /// バイト配列
            /// </summary>
            private List<byte> _bytes;

            /// <summary>
            /// Byte Array
            /// バイト配列
            /// </summary>
            public List<byte> Bytes => this._bytes;

            /// <summary>
            /// Joined Bit String
            /// 全バイト配列を連結したビット文字列
            /// </summary>
            public string BitString => string.Join("", this._bytes.Select(Xb.Byte.GetBitString));

            /// <summary>
            /// Constructor
            /// コンストラクタ
            /// </summary>
            /// <param name="bytes"></param>
            public ByteArray(byte[] bytes)
            {
                this._bytes = new List<byte>();
                this._bytes.AddRange(bytes);
            }

            /// <summary>
            /// Get Bit value
            /// 指定位置のビット値をboolで取得する。
            /// </summary>
            /// <param name="byteIndex"></param>
            /// <param name="bitIndex"></param>
            /// <returns></returns>
            public bool GetBit(int byteIndex, int bitIndex)
            {
                if (bitIndex < 0 || 7 < bitIndex)
                    throw new ArgumentOutOfRangeException("bitIndex out of range");

                if (byteIndex < 0 || this._bytes.Count - 1 < byteIndex)
                    throw new ArgumentOutOfRangeException("byteIndex out of range");
                
                var bits = Convert.ToString(this._bytes[byteIndex], 2).PadLeft(8, '0');
                return (bits.Substring(7 - bitIndex, 1) == "1");
            }


            /// <summary>
            /// Write Bit value
            /// 指定位置のビット値をセットする。
            /// </summary>
            /// <param name="byteIndex"></param>
            /// <param name="bitIndex"></param>
            /// <param name="value"></param>
            public void SetBit(int byteIndex, int bitIndex, bool value)
            {
                if (bitIndex < 0 || 7 < bitIndex)
                    throw new ArgumentOutOfRangeException("bitIndex out of range");

                if (byteIndex < 0 || this._bytes.Count - 1 < byteIndex)
                    throw new ArgumentOutOfRangeException("byteIndex out of range");

                var toBits = "";
                var fromBits = Convert.ToString(this._bytes[byteIndex], 2).PadLeft(8, '0');

                for (var i = 0; i <= 7; i++)
                {
                    if ((7 - bitIndex) == i)
                    {
                        toBits += value ? "1" : "0";
                    }
                    else
                    {
                        toBits += fromBits.Substring(i, 1);
                    }
                }

                this._bytes[byteIndex] = Convert.ToByte(Convert.ToInt16(toBits, 2));
            }


            /// <summary>
            /// Get Integer from byte range
            /// 指定バイト範囲から符号なし整数を取得する。
            /// </summary>
            /// <param name="byteIndex"></param>
            /// <param name="length"></param>
            /// <returns></returns>
            public int GetInteger(int byteIndex, int length)
            {
                if (byteIndex < 0 || this._bytes.Count - 1 < byteIndex)
                    throw new ArgumentOutOfRangeException("byteIndex out of range");

                if (length < 1 || 4 < length)
                    throw new ArgumentOutOfRangeException("length out of range");

                if (this._bytes.Count - 1 < byteIndex + length)
                    throw new ArgumentOutOfRangeException("byteIndex + length out of array index");

                var bytes = new byte[4];
                Array.Copy(this._bytes.ToArray(), byteIndex, bytes, 0, length);

                return (int)BitConverter.ToUInt32(bytes, 0);
            }


            /// <summary>
            /// Get Long from byte range
            /// 指定バイト範囲から符号なし整数を取得する。
            /// </summary>
            /// <param name="byteIndex"></param>
            /// <param name="length"></param>
            /// <returns></returns>
            public long GetLong(int byteIndex, int length)
            {
                if (byteIndex < 0 || this._bytes.Count - 1 < byteIndex)
                    throw new ArgumentOutOfRangeException("byteIndex out of range");

                if (length < 1 || 8 < length)
                    throw new ArgumentOutOfRangeException("length out of range");

                if (this._bytes.Count - 1 < byteIndex + length)
                    throw new ArgumentOutOfRangeException("byteIndex + length out of array index");

                var bytes = new byte[8];
                Array.Copy(this._bytes.ToArray(), byteIndex, bytes, 0, length);

                return (long)BitConverter.ToUInt64(bytes, 0);
            }


            public class Index
            {
                public ByteArray ByteArray { get; }
                public int ByteIndex { get; internal set; } = -1;
                public int BitIndex { get; internal set; } = -1;

                public Index Next
                {
                    get
                    {
                        var result = new Index(this.ByteArray);
                        result.ByteIndex = this.ByteIndex;
                        result.BitIndex = this.BitIndex + 1;

                        if (8 <= result.BitIndex)
                        {
                            result.BitIndex = 0;
                            result.ByteIndex++;
                        }

                        if (this.ByteArray.Bytes.Count <= result.ByteIndex)
                        {
                            result.BitIndex = -1;
                            result.ByteIndex = -1;
                        }

                        return result;
                    }
                }


                public Index Prev
                {
                    get
                    {
                        var result = new Index(this.ByteArray);
                        result.ByteIndex = this.ByteIndex;
                        result.BitIndex = this.BitIndex - 1;

                        if (result.BitIndex < 0)
                        {
                            result.BitIndex = 7;
                            result.ByteIndex--;
                        }

                        if (result.ByteIndex < 0)
                        {
                            result.BitIndex = -1;
                            result.ByteIndex = -1;
                        }

                        return result;
                    }
                }

                internal Index(ByteArray byteArray)
                {
                    this.ByteArray = byteArray;
                }
            }

            public Index IndexOf(bool value)
            {
                var result = new Index(this);
                var needle = value ? "1" : "0";

                for (var i = 0; i < this._bytes.Count; i++)
                {
                    var bitString = Xb.Byte.GetBitString(this._bytes[i]);
                    var index = bitString.IndexOf(needle, StringComparison.Ordinal);
                    if (index >= 0)
                    {
                        result.ByteIndex = i;
                        result.BitIndex = index;
                        break;
                    }
                }
                
                return result;
            }

            public Index LastIndexOf(bool value)
            {
                var result = new Index(this);
                var needle = value ? "1" : "0";

                for (var i = this._bytes.Count - 1; i >= 0; i--)
                {
                    var bitString = Xb.Byte.GetBitString(this._bytes[i]);
                    var index = bitString.LastIndexOf(needle, StringComparison.Ordinal);
                    if (index >= 0)
                    {
                        result.ByteIndex = i;
                        result.BitIndex = index;
                        break;
                    }
                }

                return result;
            }
            
        }
    }
}
