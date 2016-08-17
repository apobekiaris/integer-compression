﻿using System;

namespace InvertedTomato.VariableLengthIntegers {
    public interface ISignedWriter : IDisposable {
        void Write(long value);

        // static byte[] Write (params long value);
        // static byte[] Write (IEnumerable<long> values);
    }
}