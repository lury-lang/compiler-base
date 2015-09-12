//
// CodePosition.cs
//
// Author:
//       Tomona Nanase <nanase@users.noreply.github.com>
//
// The MIT License (MIT)
//
// Copyright (c) 2015 Tomona Nanase
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace Lury.Compiling.Utils
{
    /// <summary>
    /// コード上における該当箇所を指し示すための構造体です。
    /// </summary>
    public class CodePosition
    {
        #region -- Public Properties --

        /// <summary>
        /// ソースコードを識別するための名前を表す文字列を取得します。
        /// </summary>
        public string SourceName { get; private set; }

        /// <summary>
        /// 該当箇所を表す <see cref="Lury.Compiling.Utils.CharPosition"/> オブジェクトを取得します。
        /// </summary>
        public CharPosition Position { get; private set; }

        /// <summary>
        /// 該当箇所の文字列の長さを表す 0 以上の整数値を取得します。
        /// </summary>
        public int Length { get; private set; }

        #endregion

        #region -- Constructors --

        /// <summary>
        /// ソース名、文字位置と長さを指定して、
        /// 新しい <see cref="Lury.Compiling.Utils.CodePosition"/> クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="sourceName">ソースコードを識別するための名前。</param>
        /// <param name="position">該当箇所を表す <see cref="Lury.Compiling.Utils.CharPosition"/> オブジェクト。</param>
        /// <param name="length">該当箇所の文字列の長さを表す 0 以上の整数値。</param>
        public CodePosition(string sourceName, CharPosition position, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");

            this.SourceName = sourceName;
            this.Position = position;
            this.Length = length;
        }

        /// <summary>
        /// ソース名、文字位置を指定して、
        /// 新しい <see cref="Lury.Compiling.Utils.CodePosition"/> クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="sourceName">ソースコードを識別するための名前。</param>
        /// <param name="position">該当箇所を表す <see cref="Lury.Compiling.Utils.CharPosition"/> オブジェクト。</param>
        public CodePosition(string sourceName, CharPosition position)
            : this(sourceName, position, 0)
        {
        }

        #endregion
    }
}

