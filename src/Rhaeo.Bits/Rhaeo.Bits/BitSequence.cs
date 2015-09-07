using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhaeo.Bits
{
  public sealed class BitSequence
  {
    #region Fields

    /// <summary>
    /// Internal list that holds <see cref="bool"/> values representing the manipulated bits.
    /// </summary>
    private readonly List<bool> list;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="BitSequence"/> class.
    /// </summary>
    public BitSequence()
    {
      list = new List<bool>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BitSequence"/> class with given bit capacity.
    /// </summary>
    /// <param name="bitCapacity">The number of bits the underlying array is initialized to handle.</param>
    public BitSequence(int bitCapacity)
    {
      list = new List<bool>(bitCapacity);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BitSequence"/> class with given bits.
    /// </summary>
    /// <param name="bits">The bits.</param>
    public BitSequence(params bool[] bits)
    {
      list = new List<bool>(bits);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BitSequence"/> class with given bytes.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    /// <param name="byteOrdering">A <see cref="ByteOrdering"/> value specifying whether the bytes are big-endian or little-endian (endianness).</param>
    public BitSequence(byte[] bytes, ByteOrdering byteOrdering = ByteOrdering.LittleEndian)
    {
      list = new List<bool>(bytes.Length * 8);
      for (var index = 0; index < bytes.Length * 8; index++)
      {
        var byteIndex = index / 8;
        var bitIndex = index % 8;
        var mask = (byte)(1 << bitIndex);
        list.Add((bytes[byteIndex] & mask) != 0);
      }

      ByteOrdering = byteOrdering;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value specifying whether the bytes are little endian.
    /// </summary>
    public ByteOrdering ByteOrdering { get; }

    /// <summary>
    /// Gets the bit count.
    /// </summary>
    public int BitCount => list.Count;

    /// <summary>
    /// Gets the byte count.
    /// </summary>
    public int ByteCount => list.Count / 8;

    #endregion

    #region Methods

    /// <summary>
    /// Appends a <c>on</c> (<c>1</c>) bit to the sequence.
    /// </summary>
    public void AppendOnBit()
    {
      list.Add(true);
    }

    /// <summary>
    /// Appends a <c>off</c> (<c>0</c>) bit to the sequence.
    /// </summary>
    public void AppendOffBit()
    {
      list.Add(false);
    }

    /// <summary>
    /// Appens a bit to the sequence.
    /// </summary>
    /// <param name="bit">
    /// <para>A value specifying whether the bit is on or off.</para>
    /// <para><c>true</c>, if the bit is on; otherwise <c>false</c>.</para>
    /// </param>
    public void AppendBit(bool bit)
    {
      list.Add(bit);
    }

    /// <summary>
    /// Appens multiple bits to the sequence.
    /// </summary>
    /// <param name="bits">The bits to append.</param>
    public void AppendBits(params bool[] bits)
    {
      list.AddRange(bits);
    }

    /// <summary>
    /// Appends multiple bits to the sequence.
    /// </summary>
    /// <param name="bits">The bits to append.</param>
    public void AppendBits(IEnumerable<bool> bits)
    {
      list.AddRange(bits);
    }

    /// <summary>
    /// Appends bits content of another <see cref="BitSequence"/> instance.
    /// </summary>
    /// <param name="bits">The <see cref="BitSequence"/> instance whose bits to append.</param>
    public void AppendBitSequence(BitSequence bits)
    {
      list.AddRange(bits.list);
    }

    /// <summary>
    /// Appends a byte.
    /// </summary>
    /// <param name="value">The byte to append.</param>
    public void AppendByte(byte value)
    {
      for (var index = 0; index < 8; index++)
      {
        var bit = (value & (1 << index)) != 0;
        list.Add(bit);
      }
    }

    /// <summary>
    /// Appends an array of bytes with specified endianness.
    /// </summary>
    /// <param name="bytes">The bytes to append.</param>
    /// <param name="byteOrdering">A value that specifies the byte ordering (endianness) of the array.</param>
    public void AppendBytes(byte[] bytes, ByteOrdering byteOrdering)
    {
      if (byteOrdering != ByteOrdering.BigEndian && byteOrdering != ByteOrdering.LittleEndian)
      {
        throw new ArgumentException("Only big-endian and little-endian byte ordering is supported.", nameof(byteOrdering));
      }

      foreach (var @byte in (byteOrdering == ByteOrdering.LittleEndian ? bytes : bytes.Reverse()))
      {
        AppendByte(@byte);
      }
    }

    /// <summary>
    /// Appends an array of bytes to the sequence in little-endian byte order.
    /// </summary>
    /// <param name="bytes">The bytes to append.</param>
    public void AppendLittleEndianBytes(params byte[] bytes)
    {
      foreach (var @byte in bytes)
      {
        AppendByte(@byte);
      }
    }

    /// <summary>
    /// Appends a <see cref="ushort"/> value bytes to the sequence in a little-endian byte order.
    /// </summary>
    /// <param name="value">The <see cref="ushort"/> value whose bytes to append.</param>
    public void AppendLittleEndianUInt16Bytes(ushort value)
    {
      var bytes = BitConverter.GetBytes(value);
      if (!BitConverter.IsLittleEndian)
      {
        bytes = bytes.Reverse().ToArray();
      }

      AppendLittleEndianBytes(bytes);
    }

    /// <summary>
    /// Appends a <see cref="uint"/> value bytes to the sequence in a little-endian byte order.
    /// </summary>
    /// <param name="value">The <see cref="uint"/> value whose bytes to append.</param>
    public void AppendLittleEndianUInt32Bytes(uint value)
    {
      var bytes = BitConverter.GetBytes(value);
      if (!BitConverter.IsLittleEndian)
      {
        bytes = bytes.Reverse().ToArray();
      }

      AppendLittleEndianBytes(bytes);
    }

    /// <summary>
    /// Appends a <see cref="ulong"/> value bytes to the sequence in a little-endian byte order.
    /// </summary>
    /// <param name="value">The <see cref="ulong"/> value whose bytes to append.</param>
    public void AppendLittleEndianUInt64Bytes(ulong value)
    {
      var bytes = BitConverter.GetBytes(value);
      if (!BitConverter.IsLittleEndian)
      {
        bytes = bytes.Reverse().ToArray();
      }

      AppendLittleEndianBytes(bytes);
    }

    /// <summary>
    /// Removes a bit from the beginning of the sequence and returns a <see cref="bool"/> value representing the state of that bit (<c>on</c> or <c>off</c>).
    /// </summary>
    /// <returns>A <see cref="bool"/> value representing the state of the removed bit.</returns>
    public bool Pop()
    {
      var bit = list[0];
      list.RemoveAt(0);
      return bit;
    }

    /// <summary>
    /// Removes mutliple bits from the beginning of the sequence and returns a <see cref="BitSequence"/> instance that manipulates the removed bits.
    /// </summary>
    /// <param name="count">The number of bits to remove.</param>
    /// <returns>A <see cref="BitSequence"/> instance initializes to manipulate over the removed bits.</returns>
    public BitSequence PopBits(int count)
    {
      var bits = new bool[count];
      list.CopyTo(0, bits, 0, count);
      list.RemoveRange(0, count);
      return new BitSequence(bits);
    }

    /// <summary>
    /// Removes multiple bytes from the beginning of the sequence and returns them in little-endian byte order.
    /// </summary>
    /// <param name="count">The number of bytes to remove.</param>
    /// <returns>A <see cref="byte[]"/> array of little-endian ordered bytes.</returns>
    public byte[] PopLittleEndianBytes(int count)
    {
      var byteArray = new byte[count / 8];
      for (int index = 0; index < count * 8; index++)
      {
        var byteIndex = index / 8;
        var bitIndex = index % 8;
        byte mask = (byte)(1 << bitIndex);
        byteArray[byteIndex] = (byte)(list[index] ? (byteArray[byteIndex] | mask) : (byteArray[byteIndex] & ~mask));
      }

      list.RemoveRange(0, count * 8);
      return byteArray;
    }

    /// <summary>
    /// Produces an array of <see cref="bool"/> values corresponding to the states (<c>on</c> or <c>off</c>) of the bits in this instance.
    /// </summary>
    /// <returns>A <see cref="bool[]"/> array.</returns>
    public bool[] ToBitArray()
    {
      return list.ToArray();
    }

    /// <summary>
    /// Produces an array of bytes in little-endian byte order constructed from the whole sequence.
    /// </summary>
    /// <returns>A <see cref="byte[]"/> array of little-endian ordered bytes.</returns>
    public byte[] ToLittleEndianByteArray()
    {
      var byteArray = new byte[list.Count / 8];
      for (int index = 0; index < list.Count; index++)
      {
        var byteIndex = index / 8;
        var bitIndex = index % 8;
        byte mask = (byte)(1 << bitIndex);
        byteArray[byteIndex] = (byte)(list[index] ? (byteArray[byteIndex] | mask) : (byteArray[byteIndex] & ~mask));
      }

      return byteArray;
    }

    /// <summary>
    /// Returns a <see cref="string"/> with binary form of the sequence.
    /// </summary>
    /// <returns>A <see cref="string"/> with binary form of the sequence.</returns>
    public override string ToString() => $"{string.Join(null, list.Select((b, i) => (i % 2 == 0 ? " " : "") + (b ? "1" : "0")))}";

    #endregion
  }
}
