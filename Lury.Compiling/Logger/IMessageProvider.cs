//
// IMessageProvider.cs
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

namespace Lury.Compiling.Logger
{
    /// <summary>
    /// コンパイル出力に対するメッセージ文字列を供給するメッセージプロバイダを規定します。
    /// </summary>
    public interface IMessageProvider
    {
        #region -- Methods --

        /// <summary>
        /// メッセージを取得します。
        /// </summary>
        /// <param name="number">エラー番号。</param>
        /// <param name="category">出力メッセージのカテゴリ。</param>
        /// <param name="message">プロバイダから取得されたメッセージを表す文字列。</param>
        /// <returns>文字列を取得できたとき true、できないとき false。</returns>
        bool GetMessage(int number, OutputCategory category, out string message);

        /// <summary>
        /// 提案メッセージを取得します。
        /// </summary>
        /// <param name="number">エラー番号。</param>
        /// <param name="category">出力メッセージのカテゴリ。</param>
        /// <param name="suggestion">プロバイダから取得された提案メッセージを表す文字列。</param>
        /// <returns>文字列を取得できたとき true、できないとき false。</returns>
        bool GetSuggestion(int number, OutputCategory category, out string suggestion);

        /// <summary>
        /// サイトリンクを取得します。
        /// </summary>
        /// <param name="number">エラー番号。</param>
        /// <param name="category">出力メッセージのカテゴリ。</param>
        /// <param name="siteLink">プロバイダから取得されたサイトリンクを表す文字列。</param>
        /// <returns>文字列を取得できたとき true、できないとき false。</returns>
        bool GetSiteLink(int number, OutputCategory category, out string siteLink);

        #endregion
    }
}
