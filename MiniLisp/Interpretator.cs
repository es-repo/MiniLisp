using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public IEnumerable<LispObject> ReadFromFile(string filePath)
        {
            return Read(File.ReadAllText(filePath));
        }

        public IEnumerable<LispObject> Read(string code)
        {
            IEnumerable<LispExpression> expressions = Parser.Parse(code);
            return expressions.Select(e => _evaluator.Eval(e));
        }
    }
}
