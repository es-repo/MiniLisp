using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MiniLisp.LispObjects;

namespace MiniLisp
{
    public class Parser
    {
        private static readonly Regex _tokenizeRegex = new Regex(@"(""[^""]*"")|([^\s^\(^\)^'^""]+)|\(|\)|'", RegexOptions.Compiled);

        public static IEnumerable<string> Tokenize(string code)
        {
            MatchCollection matches = _tokenizeRegex.Matches(code);
            return matches.Cast<Match>().Select(m => m.Value);
        }

        public static IEnumerable<LispExpression> Parse(string code)
        {
            IEnumerable<string> tokens = Tokenize(code);
            
            Stack<LispExpression> stack = new Stack<LispExpression>();
            foreach (string t in tokens)
            {
                LispObject lispObject = null;
                bool boolValue;
                string stringValue;
                double numberValue;
                
                if (t == "(")
                {
                    lispObject = new LispEval();
                }
                else if (t == ")")
                {
                    // TODO: throw parser excpetion if ")" is not expected
                    LispExpression expression = stack.Pop();
                    if (stack.Count == 0)
                        yield return expression;
                    continue;
                }
                else if (t == "'")
                {
                }
                else if (t == "define")
                {
                    lispObject = new LispDefine();
                }
                else if (t == "nil")
                {
                    lispObject = new LispNil();
                }
                else if (IsBoolean(t, out boolValue))
                {
                    lispObject = new LispBoolean(boolValue);
                }
                else if (IsString(t, out stringValue))
                {
                    lispObject = new LispString(stringValue);
                }
                else if (IsNumber(t, out numberValue))
                {
                    lispObject = new LispNumber(numberValue);
                }
                else
                {
                    lispObject = new LispIdentifier(t);
                }

                LispExpression childExpression = new LispExpression(lispObject);
                
                if (stack.Count > 0)
                {
                    stack.Peek().Children.Add(childExpression);
                }
                
                if (lispObject is LispEval)
                {
                    stack.Push(childExpression);
                }
                
                if (stack.Count == 0)
                {
                    yield return childExpression;
                }
            }

            // TODO: throw parser excpetion if expression is not finished.

            yield break;
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
