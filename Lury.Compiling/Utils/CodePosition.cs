﻿//
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
using System.IO;

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
        public string SourceName { get; }

        /// <summary>
        /// 該当箇所を表す <see cref="Lury.Compiling.Utils.CharPosition"/> オブジェクトを取得します。
        /// </summary>
        public CharPosition CharPosition { get; }

        /// <summary>
        /// 該当箇所の文字列の長さを表す 0 以上の整数値を取得します。
        /// </summary>
        public int Length { get; }

        #endregion

        #region -- Constructors --

        /// <summary>
        /// ソース名、文字位置と長さを指定して、
        /// 新しい <see cref="Lury.Compiling.Utils.CodePosition"/> クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="sourceName">ソースコードを識別するための名前。</param>
        /// <param name="charPosition">該当箇所を表す <see cref="Lury.Compiling.Utils.CharPosition"/> オブジェクト。</param>
        /// <param name="length">該当箇所の文字列の長さを表す 0 以上の整数値。</param>
        public CodePosition(string sourceName, CharPosition charPosition, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            SourceName = sourceName;
            CharPosition = charPosition;
            Length = length;
        }

        /// <summary>
        /// ソース名、文字位置を指定して、
        /// 新しい <see cref="Lury.Compiling.Utils.CodePosition"/> クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="sourceName">ソースコードを識別するための名前。</param>
        /// <param name="charPosition">該当箇所を表す <see cref="Lury.Compiling.Utils.CharPosition"/> オブジェクト。</param>
        public CodePosition(string sourceName, CharPosition charPosition)
            : this(sourceName, charPosition, 0)
        {
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>現在のオブジェクトを説明する文字列。</returns>
        public override string ToString()
        {
            return $"{CharPosition}#{Path.GetFileName(SourceName)}";
        }

        /// <summary>
        /// 指定した <see cref="System.Object"/> が、現在の <see cref="System.Object"/> と等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">現在のオブジェクトと比較するオブジェクト。</param>
        /// <returns>指定したオブジェクトが現在のオブジェクトと等しい場合は true。それ以外の場合は false。</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is CodePosition))
                return false;

            return Equals(this, (CodePosition)obj);
        }

        /// <summary>
        /// 特定の型のハッシュ関数として機能します。
        /// </summary>
        /// <returns>現在の <see cref="Lury.Compiling.Utils.CodePosition"/> のハッシュ コード。</returns>
        public override int GetHashCode()
            => CharPosition.GetHashCode() ^ Length.GetHashCode() ^ SourceName.GetHashCode();

        /// <summary>
        /// 指定されたインスタンスが等しいかどうかを判断します。
        /// </summary>
        /// <param name="objA">比較対象の第 1 オブジェクト。</param>
        /// <param name="objB">2 番目に比較するオブジェクト。</param>
        /// <returns>オブジェクトが等しいと見なされた場合は true。それ以外の場合は false。
        /// objA と objB の両方が null の場合、このメソッドは true を返します。</returns>
        public static bool Equals(CodePosition objA, CodePosition objB)
        {
            if ((object)objA == null && (object)objB == null)
                return true;

            if ((object)objA != null && (object)objB != null)
                return (objA.CharPosition == objB.CharPosition) &&
                       (objA.Length == objB.Length) &&
                       (objA.SourceName == objB.SourceName);

            return false;
        }

        public static bool operator==(CodePosition objA, CodePosition objB)
            => Equals(objA, objB);

        public static bool operator !=(CodePosition objA, CodePosition objB)
            => !Equals(objA, objB);

        #endregion
    }
}

