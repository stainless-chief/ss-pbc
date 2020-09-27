﻿using Common.FirendlyConverters;
using System;
using Xunit;

namespace Common.Tests.FirendlyConverters
{
    public class FriendlyConvertTests
    {
        [Theory]
        [InlineData(00,                      "0 B")]
        [InlineData(01,                      "1 B")]
        [InlineData(01 * 1000,               "1000 B")]
        [InlineData(01 * 1024,               "1 KB")]
        [InlineData(08 * 1024,               "8 KB")]
        [InlineData(19 * 1024,               "19 KB")]
        [InlineData(01 * 1024 * 1024,        "1 MB")]
        [InlineData(16 * 1024 * 1024,        "16 MB")]
        [InlineData(01 * 1024 * 1024 * 1024, "1 GB")]
        public void BytesToString_ValidInput(long bytes, string expected)
        {
            var result = FriendlyConvert.BytesToString(bytes);

            Assert.True(result.Equals(expected));
        }

        [Theory]
        [InlineData(00,                      new string[] { "a", "b", "c", "d" }, "0 a")]
        [InlineData(01,                      new string[] { "a", "b", "c", "d" }, "1 a")]
        [InlineData(01 * 1000,               new string[] { "a", "b", "c", "d" }, "1000 a")]
        [InlineData(01 * 1024,               new string[] { "a", "b", "c", "d" }, "1 b")]
        [InlineData(08 * 1024,               new string[] { "a", "b", "c", "d" }, "8 b")]
        [InlineData(19 * 1024,               new string[] { "a", "b", "c", "d" }, "19 b")]
        [InlineData(01 * 1024 * 1024,        new string[] { "a", "b", "c", "d" }, "1 c")]
        [InlineData(16 * 1024 * 1024,        new string[] { "a", "b", "c", "d" }, "16 c")]
        [InlineData(01 * 1024 * 1024 * 1024, new string[] { "a", "b", "c", "d" }, "1 d")]
        public void BytesToString_ValidLocalizedInput(long bytes, string[] suffices, string expected)
        {
            var result = FriendlyConvert.BytesToString(bytes, suffices);

            Assert.True(result.Equals(expected));
        }


        [Theory]
        [InlineData(-1 , null)]
        [InlineData(long.MinValue, null)]
        [InlineData(12, new string[] { "", "" })]
        [InlineData(77, new string[] { "", "", "" })]
        public void BytesToString_InValidInput(long bytes, string[] suffices)
        {
            Assert.Throws<ArgumentException>(() => FriendlyConvert.BytesToString(bytes, suffices));
        }

    }
}