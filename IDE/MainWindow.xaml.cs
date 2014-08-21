using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using MiniLisp;
using MiniLisp.Exceptions;
using MiniLisp.Expressions;

namespace IDE
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            TextRange inputText = new TextRange(_inputTextBox.Document.ContentStart, _inputTextBox.Document.ContentEnd);
            inputText.Text =
@"(define (solve-hanoi-tower n)
  (define (move n f t s)     
    (cond ((= n 0) null)
        ((list
          (move (- n 1) f s t)          
          (list f '=> t)
          (move (- n 1) s t f)))))
  (move n 1 3 2))

(solve-hanoi-tower 3)";
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                Run();
            }
        }

        
        private void Run()
        {
            TextRange outputText = new TextRange(_outputTextBox.Document.ContentStart, _outputTextBox.Document.ContentEnd);
            try
            {
                string code = new TextRange(_inputTextBox.Document.ContentStart, _inputTextBox.Document.ContentEnd).Text;
                
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Interpretator interpretator = new Interpretator();
                LispValueElement[] result = interpretator.Read(code).ToArray();
                stopwatch.Stop();

                string output = string.Join(Environment.NewLine, result.Where(o => !(o is LispVoid)).Select(o => o.ToString()).ToArray());
                outputText.Text = output;
                outputText.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);

                _outputTextBox.AppendText("\n\nTime: " + stopwatch.Elapsed);
            }
            catch (LispException lispException)
            {
                outputText.Text = lispException.Message;
                outputText.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            }
        }
    }
}
