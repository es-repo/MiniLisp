namespace MiniLisp.LispObjects
{
    public class LispDefine : LispObject
    {
        public LispDefine() : base(null)
        {
        }

        public override string ToString()
        {
            return "#define";
        }
    }
}
