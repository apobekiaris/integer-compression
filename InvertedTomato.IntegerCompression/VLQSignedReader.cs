﻿using System;
using System.Collections.Generic;
using System.IO;

namespace InvertedTomato.IntegerCompression {
    /// <summary>
    /// Reader for VLQ signed numbers. Values are translated to unsigned values using ProtoBuffer's ZigZag algorithm.
    /// </summary>
    public class VLQSignedReader : ISignedReader {
        /// <summary>
        /// Read first value from a byte array.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static long ReadOneDefault(byte[] input) {
            if (null == input) {
                throw new ArgumentNullException("input");
            }

            using (var stream = new MemoryStream(input)) {
                using (var reader = new VLQSignedReader(stream)) {
                    return reader.Read();
                }
            }
        }

        /// <summary>
        /// If disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Underlying unsigned reader.
        /// </summary>
        private readonly VLQUnsignedReader Underlying;

        /// <summary>
        /// Standard instantiation.
        /// </summary>
        /// <param name="input"></param>
        public VLQSignedReader(Stream input) {
            Underlying = new VLQUnsignedReader(input);
        }

        /// <summary>
        /// Instantiate with options.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="packetSize">The number of bits to include in each packet.</param>
        public VLQSignedReader(Stream input, int packetSize) {
            Underlying = new VLQUnsignedReader(input, packetSize);
        }
        
        /// <summary>
        /// Read the next value. 
        /// </summary>
        /// <returns></returns>
        public long Read() {
            return ZigZag.Decode(Underlying.Read());
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (IsDisposed) {
                return;
            }
            IsDisposed = true;

            Underlying.Dispose();

            if (disposing) {
                // Dispose managed state (managed objects).
            }
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose() {
            Dispose(true);
        }
    }
}