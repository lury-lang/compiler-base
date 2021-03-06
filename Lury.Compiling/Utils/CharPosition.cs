﻿//
// CharPosition.cs
//
// Author:
//       Tomona Nanase <nanase@users.noreply.github.com>
//
// The MIT License (MIT)
//
// Copyright (c) 2014-2015 Tomona Nanase
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
    /// 文字列中の行と列の位置を表すための構造体です。
    /// </summary>
    public struct CharPosition
    {
        #region -- Private Static Fields --

        /// <summary>
        /// 文字列中のどの位置も指し示さないような、
        /// 空の <see cref="Lury.Compiling.Utils.CharPosition"/> オブジェクトです。
        /// この変数は読み取り専用です。
        /// </summary>
        public static readonly CharPosition Empty = default(CharPosition);

        /// <summary>
        /// 文字列の先頭を指し示す <see cref="Lury.Compiling.Utils.CharPosition"/> オブジェクトです。
        /// この変数は読み取り専用です。
        /// </summary>
        public static readonly CharPosition BasePosition = new CharPosition(1, 1);

        #endregion

        #region -- Public Properties --

        /// <summary>
        /// 行位置を取得または設定します。
        /// </summary>
        /// <value>行を表す 1 以上の整数値。</value>
        public int Line { get; }

        /// <summary>
        /// 列位置を取得または設定します。
        /// </summary>
        /// <value>列を表す 1 以上の整数値。</value>
        public int Column { get; }

        /// <summary>
        /// この <see cref="Lury.Compiling.Utils.CharPosition"/> オブジェクトが
        /// 空（どの位置も指し示さない）であるかの真偽値を取得します。
        /// </summary>
        /// <value>true のときこのオブジェクトは空、それ以外のとき false。</value>
        public bool IsEmpty => this == Empty;

        #endregion

        #region -- Constructor --

        /// <summary>
        /// 行と列を指定して新しい <see cref="Lury.Compiling.Utils.CharPosition"/> 構造体のインスタンスを初期化します。
        /// </summary>
        /// <param name="line">行位置。</param>
        /// <param name="column">列位置。</param>
        public CharPosition(int line, int column)
        {
            if (line < 1)
                throw new ArgumentOutOfRangeException(nameof(line));

            if (column < 1)
                throw new ArgumentOutOfRangeException(nameof(column));

            Line = line;
            Column = column;
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// このオブジェクトを文字列に変換します。
        /// </summary>
        /// <returns>このオブジェクトの状態を表す文字列。</returns>
        public override string ToString()
            => $"({Line}, {Column})";

        /// <summary>
        /// 2つのオブジェクトのインスタンスが等しいかどうかを判定します。
        /// </summary>
        /// <param name="obj">このオブジェクトと比較される別のオブジェクト。</param>
        /// <returns>2つのオブジェクトが等しいとき true、それ以外のとき false。</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is CharPosition))
                return false;

            var x = (CharPosition)obj;

            return (x.Line == Line) && (x.Column == Column);
        }

        /// <summary>
        /// このオブジェクトに対するハッシュ値を取得します。
        /// </summary>
        /// <returns>このオブジェクトに対するハッシュ値を表した整数値。</returns>
        public override int GetHashCode()
            => Line ^ Column;

        #endregion

        #region -- Public Static Methods --

        /// <summary>
        /// 2つの <see cref="CharPosition"/> オブジェクトが等価であるかを判定します。
        /// </summary>
        /// <param name="cp1">1つ目の <see cref="CharPosition"/> オブジェクト。</param>
        /// <param name="cp2">2つ目の <see cref="CharPosition"/> オブジェクト。</param>
        /// <returns>2つのオブジェクトが等価であるとき true、それ以外のとき false。</returns>
        public static bool operator ==(CharPosition cp1, CharPosition cp2)
            => cp1.Line == cp2.Line && cp1.Column == cp2.Column;

        /// <summary>
        /// 2つの <see cref="CharPosition"/> オブジェクトが等価でないかを判定します。
        /// </summary>
        /// <param name="cp1">1つ目の <see cref="CharPosition"/> オブジェクト。</param>
        /// <param name="cp2">2つ目の <see cref="CharPosition"/> オブジェクト。</param>
        /// <returns>2つのオブジェクトが等価でないとき true、それ以外のとき false。</returns>
        public static bool operator !=(CharPosition cp1, CharPosition cp2)
            => cp1.Line != cp2.Line || cp1.Column != cp2.Column;

        #endregion
    }
}

