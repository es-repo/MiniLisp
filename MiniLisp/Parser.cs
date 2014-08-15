using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MiniLisp.Exceptions;
using MiniLisp.Expressions;

namespace MiniLisp
{
    public class Parser
    {
        private static readonly Regex _tokenizeRegex = new Regex(@"(""[^""]*"")|([^\s^\(^\)^'^""]+)|\(|\)|'", RegexOptions.Compiled);

        public static string[] Tokenize(string code)
        {
            MatchCollection matches = _tokenizeRegex.Matches(code);
            return matches.Cast<Match>().Select(m => m.Value).ToArray();
        }

        public static IEnumerable<LispExpression> Parse(string code)
        {
            string[] tokens = Tokenize(code);
            if (tokens.Length == 0)
                yield break;

            int start = 0;
            while (start < tokens.Length)
            {
                int end = GetExpressionEnd(tokens, start);
                yield return ParseSingleExpression(tokens, start, end);
                start = end;
            }
        }

        private static LispExpression ParseSingleExpression(string[] tokens, int start, int end)
        {
            Stack<LispExpression> stack = new Stack<LispExpression>();
            for (int i = start; i < end; i++)
            {
                LispExpressionElement lispElement = null;
                bool boolValue;
                string stringValue;
                double numberValue;
                string t = tokens[i];

                if (t == "(")
                {
                    if (i + 1 < tokens.Length)
                    {
                        switch (tokens[i + 1])
                        {
                            case "lambda":
                                lispElement = new LispLambda();
                                break;

                            case "define":
                                lispElement = new LispDefine();
                                break;

                            case "set!":
                                lispElement = new LispSet();
                                break;

                            case "if":
                                lispElement = new LispIf();
                                break;
                        }
                    }

                    if (lispElement == null)
                    {
                        LispExpression prevExpr = stack.Count > 0 ? stack.Peek() : null;
                        bool isGroup = prevExpr != null && prevExpr.Children.Count == 0 && (prevExpr.Value is LispLambda || prevExpr.Value is LispDefine);
                        lispElement = isGroup ? (LispExpressionElement) new LispGroupElement() : new LispEval();
                    }
                    else
                    {
                        i++;
                    }
                }
                else if (t == ")")
                {
                    if (stack.Count == 0)
                        throw new LispParsingException("Unexpected \")\"");

                    LispExpression expression = stack.Pop();
                    if (stack.Count == 0)
                        return expression;

                    continue;
                }
                else if (t == "'")
                {
                    int exprObjEnd = GetExpressionEnd(tokens, i + 1);
                    if (exprObjEnd == i + 1)
                        throw new LispParsingException("Expected an element for quoting \"'\".");

                    LispExpression expression = ParseSingleExpression(tokens, i + 1, exprObjEnd);
                    lispElement = new LispExpressionValue(expression);
                    i = exprObjEnd - 1;
                }
                else if (t == "nil")
                {
                    lispElement = new LispNil();
                }
                else if (IsBoolean(t, out boolValue))
                {
                    lispElement = new LispBoolean(boolValue);
                }
                else if (IsString(t, out stringValue))
                {
                    lispElement = new LispString(stringValue);
                }
                else if (IsNumber(t, out numberValue))
                {
                    lispElement = new LispNumber(numberValue);
                }
                else
                {
                    lispElement = new LispIdentifier(t);
                }

                LispExpression childExpression = new LispExpression(lispElement);

                if (stack.Count > 0)
                {
                    stack.Peek().Children.Add(childExpression);
                }

                if (t == "(")
                {
                    stack.Push(childExpression);
                }

                if (stack.Count == 0)
                {
                    return childExpression;
                }
            }

            throw new LispParsingException("Expected a \")\" to close \"(\".");
        }

        private static int GetExpressionEnd(string[] tokens, int start)
        {
            int i = start;
            while (i < tokens.Length && tokens[i] == "'")
                i++;

            if (i == tokens.Length)
                return i;

            if (tokens[i] != "(")
                return i + 1;

            i++;
            int depth = 1;
            while (i < tokens.Length && depth > 0)
            {
                if (tokens[i] == "(")
                    depth++;
                else if (tokens[i] == ")")
                    depth--;
                i++;
            }
            return i;
        }

        private static bool IsNumber(string token, out double value)
        {            
            return double.TryParse(token, out value);
        }

        private static bool IsString(string token, out string value)
        {            
            if (token[0] == '"' && token[token.Length - 1] == '"')
            {
                value = token.Substring(1, token.Length - 2);
                return true;
            }
            value = null;
            return false;
        }

        private static bool IsBoolean(string token, out bool value)
        {
            if (token == "false" || token == "#f" || token == "#F")
            {
                value = false;
                return true;
            }

            if (token == "true" || token == "#t" || token == "#T")
            {
                value = true;
                return true;
            }

            value = false;
            return false;
        } 
    }
}
