﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace InvertedTomato.Compression.Integers.Tests {
    [TestClass]
    public class EliasGammaSignedReaderTests {
        [TestMethod]
        public void WriteRead_First1000() {
            for (long input = -500; input < 500; input++) {
                var encoded = EliasGammaSignedWriter.WriteOneDefault(input);
                var output = EliasGammaSignedReader.ReadOneDefault(encoded);
                
                Assert.AreEqual(input, output);
            }
        }

        [TestMethod]
        public void WriteRead_First1000_Appending() {
            long min = -500;
            long max = 500;

            using (var stream = new MemoryStream()) {
                using (var writer = new EliasGammaSignedWriter(stream)) {
                    for (long i = min; i < max; i++) {
                        writer.Write(i);
                    }
                }
                stream.Position = 0;
                using (var reader = new EliasGammaSignedReader(stream)) {
                    for (long i = min; i < max; i++) {
                        Assert.AreEqual(i, reader.Read());
                    }
                }
            }
        }
    }
}
