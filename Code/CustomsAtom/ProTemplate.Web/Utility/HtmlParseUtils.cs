#region License

// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Sgml;

namespace Microsoft.Commerce.Utilities.Web
{
    public static class HtmlParseUtils
    {
        public static string FormatHtml(string html, bool upper, bool formatted)
        {
            try
            {
                SgmlReader r = new SgmlReader();
                r.DocType = "HTML";
                r.InputStream = new StringReader(html);
                if (upper)
                {
                    r.CaseFolding = CaseFolding.ToUpper;
                }
                StringWriter sw = new StringWriter();
                XmlTextWriter w = new XmlTextWriter(sw);
                if (formatted)
                {
                    w.Formatting = Formatting.None;
                    r.WhitespaceHandling = WhitespaceHandling.None;
                }
                else
                {
                    w.Formatting = Formatting.Indented;
                    r.WhitespaceHandling = WhitespaceHandling.All;
                }
                while (!r.EOF)
                {
                    w.WriteNode(r, true);
                }
                w.Close();
                return sw.ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public static string GetSubString(string text, string startPattern, string mediumPattern, string endPattern, string replacement1, string replacement2, string replacement3)
        {
            RegexOptions options = RegexOptions.Singleline;
            Regex regex;
            bool isMatch;
            Match match;
            string pattern;
            string returnText;

            if (string.IsNullOrEmpty(mediumPattern))
            {
                pattern = startPattern + ".*?" + endPattern;
            }
            else
            {
                pattern = startPattern + "(" + mediumPattern + ")*.*?" + endPattern;
            }

            regex = new Regex(pattern, options);
            isMatch = regex.IsMatch(text);
            if (isMatch)
            {
                match = regex.Match(text);
                returnText = Regex.Replace(match.Value, startPattern, "");
                returnText = Regex.Replace(returnText, endPattern.Replace("?", ""), "");
                if (!string.IsNullOrEmpty(replacement1))
                {
                    returnText = returnText.Replace(replacement1, "");
                }
                if (!string.IsNullOrEmpty(replacement2))
                {
                    returnText = returnText.Replace(replacement2, "");
                }
                if (!string.IsNullOrEmpty(replacement3))
                {
                    returnText = returnText.Replace(replacement3, "");
                }
            }
            else
            {
                returnText = "";
            }
            return returnText;
        }

        public static List<string> GetSubStrings(string text, string startPattern, string mediumPattern, string endPattern, string replacement1, string replacement2, string replacement3)
        {
            RegexOptions options = RegexOptions.Singleline;
            Regex regex;
            bool isMatch;
            MatchCollection matches;
            string pattern;
            string returnText;
            List<string> returnTextList = new List<string>();

            if (string.IsNullOrEmpty(mediumPattern))
            {
                pattern = startPattern + ".*?" + endPattern;
            }
            else
            {
                pattern = startPattern + "(" + mediumPattern + ")*.*?" + endPattern;
            }

            regex = new Regex(pattern, options);
            isMatch = regex.IsMatch(text);
            if (isMatch)
            {
                matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    returnText = match.Value;
                    if (!string.IsNullOrEmpty(replacement1))
                    {
                        returnText = returnText.Replace(replacement1, "");
                    }
                    if (!string.IsNullOrEmpty(replacement2))
                    {
                        returnText = returnText.Replace(replacement2, "");
                    }
                    if (!string.IsNullOrEmpty(replacement3))
                    {
                        returnText = returnText.Replace(replacement3, "");
                    }
                    returnTextList.Add(returnText);
                }
                return returnTextList;
            }
            else
            {
                return null;
            }
        }
    }
}