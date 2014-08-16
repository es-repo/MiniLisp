using System.Collections.Generic;
using System.IO;
using System.Linq;
using MiniLisp.Expressions;

namespace MiniLisp
{
    public class Interpretator
    {
        private readonly Evaluator _evaluator;

        public Interpretator()
        {
            _evaluator = new Evaluator();
            foreach (var e in ReadFromFile("StdLib.lsp"));
        }

        public IEnumerable<LispValueElement> ReadFromFile(string filePath)
        {
            return Read(File.ReadAllText(filePath));
        }

        public IEnumerable<LispValueElement> Read(string input)
        {
            IEnumerable<LispExpression> expressions = Parser.Parse(input);
            return expressions.Select(e => _evaluator.Eval(e));
        }
    }
}
