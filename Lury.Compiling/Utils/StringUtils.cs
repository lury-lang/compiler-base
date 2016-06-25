//
// StringUtils.cs
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lury.Compiling.Utils
{
    /// <summary>
    /// 文字列に対する拡張メソッドを提供します。
    /// </summary>
    /// <remarks>
    /// <see cref="StringUtils"/> クラスは <see cref="Lury"/> 名前空間で用いられる、文字列のメソッド群をまとめたものです。
    /// </remarks>
    public static class StringUtils
    {
        #region -- Private Static Fields --

        private static readonly Regex NewLine = new Regex(@"(?:\n|(?:\r\n)|\r|\u2028|\u2029)", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex UnicodeHex = new Regex(@"\\x[0-9A-Fa-f]{1,4}", RegexOptions.Compiled);
        private static readonly Regex UnicodeHex4 = new Regex(@"\\u[0-9A-Fa-f]{4}", RegexOptions.Compiled);
        private static readonly Regex UnicodeHex8 = new Regex(@"\\U[0-9A-Fa-f]{8}", RegexOptions.Compiled);

        #endregion

        #region -- Public Static Methods --

        /// <summary>
        /// 文字列の行数を取得します。
        /// </summary>
        /// <returns>指定された文字列の行数。</returns>
        /// <param name="text">文字列。null の場合は常に 0 が返されます。</param>
        /// <remarks>
        /// 指定された文字列の改行文字を集計し、文字列の行数を計算します。指定された文字列が null でなく、
        /// かつ改行文字が含まれない場合は常に 1 が返されます。文字列が null の場合は常に 0 が返されます。
        /// <c>\\r\\n</c> のような二文字一組の改行文字は一つの改行として集計されます。
        /// このメソッドは複数の改行文字種が混在していても集計を行います。
        /// </remarks>
        public static int GetNumberOfLine(this string text)
            => (text == null) ? 0 : NewLine.Matches(text).Count + 1;

        /// <summary>
        /// 文字列の行と列の位置をインデクスから求めます。
        /// </summary>
        /// <returns>インデクスに対応する行と列の位置を格納した <see cref="CharPosition"/> オブジェクト。</returns>
        /// <param name="text">文字列。</param>
        /// <param name="index">文字列の位置を指し示すインデクス。</param>
        /// <exception cref="ArgumentNullException">パラメータ text が null です。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// パラメータ index が 0 未満、または text の長さに 1 加えた数よりも大きいです。
        /// </exception>
        /// <remarks>
        /// 指定された文字列とインデクスから、行と列の位置を表す <see cref="CharPosition"/> オブジェクトを取得します。
        /// パラメータ text は null を許容されず、index は 0 未満や text の長さに 1 加えた数より大きくすることはできません。
        /// index の値に text の長さに 1 加えた数が指定された場合、それは文字列の最終端の位置を表す
        /// <see cref="CharPosition"/> オブジェクトが取得されます。
        /// </remarks>
        public static CharPosition GetPositionByIndex(this string text, int index)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (index < 0 || index > text.Length + 1)
                throw new ArgumentOutOfRangeException(nameof(index));

            var position = CharPosition.BasePosition;
            Match prevMatch = null;

            foreach (var match in NewLine.Matches(text).Cast<Match>().TakeWhile(match => match.Index < index))
            {
                prevMatch = match;
                position = new CharPosition(position.Line + 1, position.Column);
            }

            position = new CharPosition(position.Line,
                     index - prevMatch?.Index ?? index + 1);

            return position;
        }

        /// <summary>
        /// 指定された文字列と、インデクスを指し示す文字列を生成します。
        /// </summary>
        /// <returns>指定された文字列と、インデクスを指し示した文字列の配列。</returns>
        /// <param name="text">文字列。</param>
        /// <param name="index">指し示されるインデクス。</param>
        /// <param name="length">指し示される長さ。</param>
        /// <param name="position">インデクスが指し示す位置を表す <see cref="CharPosition"/> オブジェクト。</param>
        /// <exception cref="ArgumentNullException">パラメータ text が null です。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// パラメータ index または length が 0 未満、
        /// あるいは index と length の合計が text の長さより大きいです。
        /// </exception>
        /// <remarks>
        /// 指定された文字列内を指し示すインデクスと長さを指定して、その範囲を指し示すような文字列の配列を取得します。
        /// パラメータ position に、指し示された位置を表す <see cref="CharPosition"/> オブジェクトが格納されます。 
        /// </remarks>
        public static string[] GeneratePointingStrings(this string text,
                                                       int index,
                                                       int length,
                                                       out CharPosition position)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (index < 0 || index >= text.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (length < 0 || index + length > text.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            position = text.GetPositionByIndex(index);

            return text.GeneratePointingStrings(position, length);
        }

        /// <summary>
        /// 指定された文字列と、行と列の位置を指し示す文字列を生成します。
        /// </summary>
        /// <returns>指定された文字列と、行と列の位置を指し示した文字列の配列。</returns>
        /// <param name="text">文字列。</param>
        /// <param name="position">指し示す行と列の位置を表す <see cref="CharPosition"/> オブジェクト。</param>
        /// <param name="length">指し示される長さ。</param>
        /// <exception cref="ArgumentNullException">パラメータ text が null です。</exception>
        /// <exception cref="CharPosition">
        /// パラメータ position が空の値、または length が 0 未満です。
        /// </exception>
        /// <remarks>
        /// 指定された文字列内を指し示す <see cref="CharPosition"/> オブジェクトを指定して、
        /// その範囲を指し示すような文字列の配列を取得します。
        /// </remarks>
        public static string[] GeneratePointingStrings(this string text, CharPosition position, int length)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (position.IsEmpty)
                throw new ArgumentOutOfRangeException(nameof(position));

            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            if (length == 0)
                length++;

            var cursorLine = text.GetLine(position.Line)
                .Replace('\t', ' ')
                .Replace('\r', ' ')
                .Replace('\n', ' ');

            // issue #1: StringUtils#GeneratePointingStringsが空行でエラー
            // https://github.com/lury-lang/compiler-base/issues/1
            if (cursorLine.Length > 0 && cursorLine.Length < length)
                throw new ArgumentOutOfRangeException(nameof(length));

            return new[]
            {
                cursorLine,
                new string(' ', position.Column - 1) + new string('^', length)
            };
        }

        /// <summary>
        /// 指定された行の文字列を取得します。
        /// </summary>
        /// <returns>指定された行の文字列。</returns>
        /// <param name="text">元となる文字列。</param>
        /// <param name="line">取得する行の 1 以上の番号。</param>
        /// <exception cref="ArgumentNullException">パラメータ text が null です。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// パラメータ line が 0 以下、または文字列の行数以上の値です。
        /// </exception>
        public static string GetLine(this string text, int line)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var matches = NewLine.Matches(text);
            line--;

            if (line < 0 || line > matches.Count)
                throw new ArgumentOutOfRangeException(nameof(line));

            var lineIndex = (line == 0) ? 0 : matches[line - 1].Index + matches[line - 1].Length;
            var lineLength = ((line == matches.Count) ? text.Length : matches[line].Index) - lineIndex;

            return text.Substring(lineIndex, lineLength);
        }

        /// <summary>
        /// 文字列に含まれる制御文字をバックスラッシュ付き文字に変換します。
        /// </summary>
        /// <param name="text">変換される文字列。</param>
        /// <exception cref="ArgumentNullException">パラメータ text が null です。</exception>
        /// <returns>変換された制御文字を含む文字列。</returns>
        public static string ConvertControlChars(this string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (text.Length < 1024)
            {
                return text.Replace("\\", "\\\\")
                           .Replace("\r", "\\r")
                           .Replace("\n", "\\n")
                           .Replace("\t", "\\t")
                           .Replace("\'", "\\'")
                           .Replace("\"", "\\\"")
                           .Replace("\0", "\\0")
                           .Replace("\a", "\\a")
                           .Replace("\b", "\\b")
                           .Replace("\f", "\\f")
                           .Replace("\v", "\\v");
            }
            return new StringBuilder(text, text.Length * 2)
                .Replace("\\", "\\\\")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t")
                .Replace("\'", "\\'")
                .Replace("\"", "\\\"")
                .Replace("\0", "\\0")
                .Replace("\a", "\\a")
                .Replace("\b", "\\b")
                .Replace("\f", "\\f")
                .Replace("\v", "\\v")
                .ToString();
        }
        
        /// <summary>
        /// 文字列を表す記号を除去し、エスケープ文字を通常の文字に変換した文字列を返します。
        /// </summary>
        /// <param name="text">変換される文字列。開始文字と終端文字が同一で、少なくとも 2 文字以上の文字列。</param>
        /// <returns>開始文字と終端文字を除去し、エスケープ文字、Unicodeエスケープ文字が変換された文字列。</returns>
        public static string ConvertFromEscapedString(this string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (text.Length < 2)
                throw new ArgumentException(nameof(text));

            var marker = text[0];

            ReplaceUnicodeChar(ref text);

            var sb = new StringBuilder(text);
            TrimMarker(sb, marker);
            ReplaceEscapeChar(sb);

            return sb.ToString();
        }

        #endregion

        #region -- Private Methods --

        private static void TrimMarker(StringBuilder value, char marker)
        {
            if (value[0] != value[value.Length - 1])
                throw new ArgumentException(nameof(value));

            if (value[0] != marker || value[value.Length - 1] != marker)
                throw new ArgumentException(nameof(marker));

            value.Remove(0, 1);
            value.Remove(value.Length - 1, 1);
        }

        private static void ReplaceEscapeChar(StringBuilder value)
        {
            value.Replace(@"\\", "\\");
            value.Replace(@"\'", "'");
            value.Replace(@"\""", "\"");
            value.Replace(@"\a", "\a");
            value.Replace(@"\b", "\b");
            value.Replace(@"\f", "\f");
            value.Replace(@"\n", "\n");
            value.Replace(@"\r", "\r");
            value.Replace(@"\t", "\t");
            value.Replace(@"\v", "\v");
        }

        private static void ReplaceUnicodeChar(ref string value)
        {
            // Refer to:
            // http://stackoverflow.com/questions/183907

            // type \xX - \xXXXX
            value = UnicodeHex.Replace(value, m => ((char)Int16.Parse(m.Value.Substring(2), NumberStyles.HexNumber)).ToString());

            // type: \uXXXX
            value = UnicodeHex4.Replace(value, m => ((char)Int32.Parse(m.Value.Substring(2), NumberStyles.HexNumber)).ToString());

            // type: \UXXXXXXXX
            value = UnicodeHex8.Replace(value, m => ToUtf16(m.Value.Substring(2)));
        }

        private static string ToUtf16(string hex)
        {
            var value = int.Parse(hex, NumberStyles.HexNumber);

            if (value < 0 || value > 0x10ffff)
                throw new ArgumentException(nameof(hex));

            if (value <= 0x00ff)
                return ((char)value).ToString();
            var w = value - 0x10000;
            var high = (char)(0xd800 | (w >> 10) & 0x03ff);
            var low = (char)(0xdc00 | (w >> 0) & 0x03ff);
            return new string(new[] { high, low });
        }

        #endregion
    }
}

