using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;
using System.Collections.Generic;

namespace Rhaeo.Bits.Tests
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void AppendBitWorks()
    {
      var bits = new BitSequence();

      bits.AppendBit(true);
      Assert.IsTrue(bits.Pop());

      bits.AppendBit(false);
      Assert.IsFalse(bits.Pop());
    }

    [TestMethod]
    public void AppendOnBitWorks()
    {
      var bits = new BitSequence();

      bits.AppendBit(true);
      bits.AppendOnBit();
      CollectionAssert.AreEqual(new[] { true, true }, bits.ToBitArray());
    }

    [TestMethod]
    public void AppendOffBitWorks()
    {
      var bits = new BitSequence();

      bits.AppendBit(false);
      bits.AppendOffBit();
      CollectionAssert.AreEqual(new[] { false, false }, bits.ToBitArray());
    }

    [TestMethod]
    public void AppendBitsWorks()
    {
      var bits = new BitSequence();

      bits.AppendBits(true, false, true, false);
      bits.AppendBits(Enumerable.Range(0, 4).Select(i => i % 2 == 1));

      CollectionAssert.AreEqual(new[] { true, false, true, false, false, true, false, true }, bits.ToBitArray());
    }

    [TestMethod]
    public void AppendBitSequenceWorks()
    {
      var bits = new BitSequence();

      bits.AppendBits(true, false, true, false);
      bits.AppendBitSequence(new BitSequence(false, true, false, true));

      CollectionAssert.AreEqual(new[] { true, false, true, false, false, true, false, true }, bits.ToBitArray());
    }

    [TestMethod]
    public void AppendByteWorks()
    {
      var bits = new BitSequence();
      var bytes = new Dictionary<byte, bool[]>()
      {
        { 0, new[] { false, false, false, false, false, false, false, false } },
        { 1, new[] { true, false, false, false, false, false, false, false } },
        { 7, new[] { true, true, true, false, false, false, false, false } },
        { 8, new[] { false, false, false, true, false, false, false, false } },
        { 77, new[] { true, false, true, true, false, false, true, false } },
        { 127, new[] { true, true, true, true, true, true, true, false } },
        { 128, new[] { false, false, false, false, false, false, false, true } },
        { 196, new[] { false, false, true, false, false, false, true, true } },
        { 255, new[] { true, true, true, true, true, true, true, true } },
      };

      foreach (var @byte in bytes.Keys)
      {
        bits.AppendByte(@byte);
      }

      var actual = bits.ToBitArray();
      var expected = bytes.Values.SelectMany(v => v).ToArray();
      CollectionAssert.AreEqual(actual, expected);
    }
  }
}
