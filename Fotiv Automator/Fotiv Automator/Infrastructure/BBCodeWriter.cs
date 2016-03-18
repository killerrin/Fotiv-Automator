using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Infrastructure
{
    public class BBCodeWriter
    {
        protected StringBuilder _stringBuilder;
        public StringBuilder StringBuilder { get { return _stringBuilder; } protected set { _stringBuilder = value; } }

        public BBCodeWriter() { _stringBuilder = new StringBuilder(); }
        protected BBCodeWriter(StringBuilder stringBuilder) { _stringBuilder = stringBuilder; }

        public void Append(string text) => _stringBuilder.Append(text);
        public void Append(string bbcode, string text) => _stringBuilder.Append(CodeFormattedString(bbcode, text));

        public void AppendLine() => _stringBuilder.AppendLine();
        public void AppendLine(string text) => _stringBuilder.AppendLine(text);
        public void AppendLine(string bbcode, string text) => _stringBuilder.AppendLine(CodeFormattedString(bbcode, text));

        public void Replace(string oldValue, string newValue) => _stringBuilder.Replace(oldValue, newValue);
        public void Replace(string oldValue, string newValue, int startIndex, int count) => _stringBuilder.Replace(oldValue, newValue, startIndex, count);

        public void Remove(int startIndex, int length) => _stringBuilder.Remove(startIndex, length);

        public override string ToString() { return _stringBuilder.ToString(); }

        #region Conversions
        public static implicit operator BBCodeWriter(StringBuilder stringBuilder) { return new BBCodeWriter(stringBuilder); }
        public static implicit operator StringBuilder(BBCodeWriter bbCodeWriter) { return bbCodeWriter._stringBuilder; }
        #endregion

        public static string CodeFormattedString(string bbcode, string text)
        {
            return string.Format("[{0}]{1}[/{0}]", bbcode, text);
        }
    }
}
