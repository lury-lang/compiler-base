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
using System.Text;
using System.Text.RegularExpressions;

namespace Lury.Compiling.Utils
{
    /// <summary>
    /// 文字列に対する拡張メソッドを提供します。
    /// </summary>
    public static class StringUtils
    {
        #region -- Private Static Fields --

        private static readonly Regex NewLine = new Regex(@"(?:\n|(?:\r\n)|\r|\u2028|\u2029)", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex unicode_hex = new Regex(@"\\x[0-9A-Fa-f]{1,4}", RegexOptions.Compiled);
        private static readonly Regex unicode_hex4 = new Regex(@"\\u[0-9A-Fa-f]{4}", RegexOptions.Compiled);
        private static readonly Regex unicode_hex8 = new Regex(@"\\U[0-9A-Fa-f]{8}", RegexOptions.Compiled);

        #endregion

        #region -- Public Static Methods --

        /// <summary>
        /// 文字列の行数を取得します。
        /// </summary>
        /// <returns>指定された文字列の行数。</returns>
        /// <param name="text">文字列。null の場合は常に 0 が返されます。</param>
        public static int GetNumberOfLine(this string text)
        {
            return (text == null) ? 0 : NewLine.Matches(text).Count + 1;
        }

        /// <summary>
        /// 文字列の行と列の位置をインデクスから求めます。
        /// </summary>
        /// <returns>インデクスに対応する行と列の位置。</returns>
        /// <param name="text">文字列。</param>
        /// <param name="index">文字列の位置を指し示すインデクス。</param>
        public static CharPosition GetPositionByIndex(this string text, int index)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (index < 0 || index > text.Length + 1)
                throw new ArgumentOutOfRangeException("index");

            CharPosition position = CharPosition.BasePosition;
            Match prevMatch = null;

            foreach (Match match in NewLine.Matches(text))
            {
                if (match.Index >= index)
                    break;

                prevMatch = match;
                position.Line++;
            }

            position.Column = (prevMatch == null) ? index + 1 :
                     index - prevMatch.Index;

            return position;
        }

        /// <summary>
        /// 指定された文字列と、インデクスを指し示す文字列を生成します。
        /// </summary>
        /// <returns>指定された文字列と、インデクスを指し示した文字列の配列。</returns>
        /// <param name="text">文字列。</param>
        /// <param name="index">指し示されるインデクス。</param>
        /// <param name="length">指し示される長さ。</param>
        /// <param name="position">インデクスが指し示す位置。</param>
        public static string[] GeneratePointingStrings(this string text,
                                                       int index,
                                                       int length,
                                                       out CharPosition position)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (index < 0 || index >= text.Length)
                throw new ArgumentOutOfRangeException("index");

            if (length < 0 || index + length > text.Length)
                throw new ArgumentOutOfRangeException("length");

            position = text.GetPositionByIndex(index);

            return text.GeneratePointingStrings(position, length);
        }

        /// <summary>
        /// 指定された文字列と、行と列の位置を指し示す文字列を生成します。
        /// </summary>
        /// <returns>指定された文字列と、行と列の位置を指し示した文字列の配列。</returns>
        /// <param name="text">文字列。</param>
        /// <param name="position">指し示す行と列の位置。</param>
        /// <param name="length">指し示される長さ。</param>
        public static string[] GeneratePointingStrings(this string text, CharPosition position, int length)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (position.IsEmpty)
                throw new ArgumentOutOfRangeException("position");

            if (length < 0)
                throw new ArgumentOutOfRangeException("length");

            if (length == 0)
                length++;

            string cursorLine = text.GetLine(position.Line)
                .Replace('\t', ' ')
                .Replace('\r', ' ')
                .Replace('\n', ' ');

            // issue #1: StringUtils#GeneratePointingStringsが空行でエラー
            // https://github.com/lury-lang/compiler-base/issues/1
            if (cursorLine.Length > 0 && cursorLine.Length < length)
                throw new ArgumentOutOfRangeException("length");

            return new string[]
            {
                cursorLine,
                new string(' ', position.Column - 1) + new string('^', length)
            };
        }

        /// <summary>
        /// 指定された行の文字列を取得します。
        /// </summary>
        /// <returns>一行の文字列。</returns>
        /// <param name="text">元となる文字列。</param>
        /// <param name="line">取得する行。</param>
        public static string GetLine(this string text, int line)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            var matches = NewLine.Matches(text);
            line--;

            if (line < 0 || line > matches.Count)
                throw new ArgumentOutOfRangeException("line");

            int lineIndex = (line == 0) ? 0 : matches[line - 1].Index + matches[line - 1].Length;
            int lineLength = ((line == matches.Count) ? text.Length : matches[line].Index) - lineIndex;

            return text.Substring(lineIndex, lineLength);
        }

        /// <summary>
        /// 文字列に含まれる制御文字をバックスラッシュ付き文字に変換します。
        /// </summary>
        /// <param name="text">変換される文字列。</param>
        /// <returns>変換された制御文字を含む文字列。</returns>
        public static string ConvertControlChars(this string text)
        {
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
            else
            {
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
        }
        
        /// <summary>
        /// 文字列を表す記号を除去し、エスケープ文字を通常の文字に変換した文字列を返します。
        /// </summary>
        /// <param name="text">変換される文字列。開始文字と終端文字が同一で、少なくとも 2 文字以上の文字列。</param>
        /// <returns>開始文字と終端文字を除去し、エスケープ文字、Unicodeエスケープ文字が変換された文字列。</returns>
        public static string ConvertFromEscapedString(this string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (text.Length < 2)
                throw new ArgumentException("text");

            char marker = text[0];

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
            if (value[0] != marker || value[value.Length - 1] != marker)
                throw new ArgumentException("value");

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
            value = unicode_hex.Replace(value, m => ((char)Int16.Parse(m.Value.Substring(2), NumberStyles.HexNumber)).ToString());

            // type: \uXXXX
            value = unicode_hex4.Replace(value, m => ((char)Int32.Parse(m.Value.Substring(2), NumberStyles.HexNumber)).ToString());

            // type: \UXXXXXXXX
            value = unicode_hex8.Replace(value, m => ToUTF16(m.Value.Substring(2)));
        }

        private static string ToUTF16(string hex)
        {
            int value = int.Parse(hex, NumberStyles.HexNumber);

            if (value < 0 || value > 0x10ffff)
                throw new ArgumentException("hex");

            if (value <= 0x00ff)
                return ((char)value).ToString();
            else
            {
                int w = value - 0x10000;
                char high = (char)(0xd800 | (w >> 10) & 0x03ff);
                char low = (char)(0xdc00 | (w >> 0) & 0x03ff);
                return new string(new char[2] { high, low });
            }
        }

        #endregion
    }
}

