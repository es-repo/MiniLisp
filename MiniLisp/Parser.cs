using System.Collections.Generic;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace MiniLisp
{
    public class Evaluator
    {
        private readonly IDictionary<string, LispObject> _defenitions;

        public Evaluator()
        {
            _defenitions = new Dictionary<string, LispObject>();
        }

        public LispObject Eval(LispExpression expression)
        {
            return null;
        }
    }

    public class Parser
    {
        public LispExpression[] Parse(string code)
        {
            return null;
        }

        private LispExpression ParseSinglExpression(string expression)
        {
            return null;
        }
    }
}
