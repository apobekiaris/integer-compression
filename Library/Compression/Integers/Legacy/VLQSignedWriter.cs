﻿using System;
using System.Collections.Generic;
using System.IO;

namespace InvertedTomato.Compression.Integers {
    /// <summary>
    /// Writer for VLQ signed numbers. Values are translated to unsigned values using ProtoBuffer's ZigZag algorithm.
    /// </summary>
    
    public class VLQSignedWriter : ISignedWriter {
        /// <summary>
        /// Write a given value.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Byte[] WriteOneDefault(Int64 value) {
            using (var stream = new MemoryStream()) {
                using (var writer = new VLQSignedWriter(stream)) {
                    writer.Write(value);
                }
                return stream.ToArray();
            }
        }

        /// <summary>
        /// If disposed.
        /// </summary>
        public Boolean IsDisposed { get; private set; }

        /// <summary>
        /// Underlying unsigned writer.
        /// </summary>
        private readonly VLQUnsignedWriter Underlying;

        /// <summary>
        /// Standard instantiation.
        /// </summary>
        /// <param name="output"></param>
        public VLQSignedWriter(Stream output) {
            Underlying = new VLQUnsignedWriter(output);
        }

        /// <summary>
        /// Instantiate with options.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="packetSize">The number of bits to include in each packet.</param>
        public VLQSignedWriter(Stream output, Int32 packetSize) {
            Underlying = new VLQUnsignedWriter(output, packetSize);
        }

        /// <summary>
        /// Append value to stream.
        /// </summary>
        /// <param name="value"></param>
        public void Write(Int64 value) {
            Underlying.Write(ZigZag.Encode(value));
        }

        /// <summary>
        /// Flush any unwritten bits and dispose.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(Boolean disposing) {
            if (IsDisposed) {
                return;
            }
            IsDisposed = true;

            Underlying.Dispose();

            if (disposing) {
                // Dispose managed state (managed objects)
                Underlying?.Dispose();
            }
        }

        /// <summary>
        /// Flush any unwritten bits and dispose.
        /// </summary>
        public void Dispose() {
            Dispose(true);
        }
    }
}
