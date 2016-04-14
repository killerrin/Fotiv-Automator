using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Infrastructure
{
    public interface IBBCodeFormatter
    {
        string ToBBCode();
    }

    public class BBCodeWriter
    {
        protected StringBuilder _stringBuilder;
        public StringBuilder StringBuilder { get { return _stringBuilder; } protected set { _stringBuilder = value; } }

        public BBCodeWriter() { _stringBuilder = new StringBuilder(); }
        protected BBCodeWriter(StringBuilder stringBuilder) { _stringBuilder = stringBuilder; }

        #region Begin/End Tag
        Stack<string> BBCodeStack = new Stack<string>();
        public BBCodeWriter BeginTag(string bbcode, bool newLine = false)
        {
            BBCodeStack.Push(bbcode);

            if (newLine)
                AppendLine($"[{bbcode}]");
            else Append($"[{bbcode}]");

            return this;
        }

        public BBCodeWriter BeginTag(string bbcode, bool newLine = false, params BBCodeParameter[] bbCodeParameters)
        {
            BBCodeStack.Push(bbcode);

            string parameters = string.Join(" ", bbCodeParameters.Select(x => x.ToString()));
            if (parameters[0] != '=')
                parameters = " " + parameters;

            if (newLine)
                AppendLine($"[{bbcode}{parameters}]");
            else Append($"[{bbcode}{parameters}]");

            return this;
        }

        public BBCodeWriter EndTag(bool newLine = false)
        {
            if (newLine)
                AppendLine($"[/{BBCodeStack.Pop()}]");
            else Append($"[/{BBCodeStack.Pop()}]");

            return this;
        }
        #endregion

        #region Append
        public BBCodeWriter Append(string text)
        {
            _stringBuilder.Append(text);
            return this;
        }
        public BBCodeWriter Append(string bbcode, string text)
        {
            _stringBuilder.Append(CodeFormattedString(bbcode, text));
            return this;
        }
        public BBCodeWriter Append(string bbcode, string text, params BBCodeParameter[] bbCodeParameters)
        {
            _stringBuilder.Append(CodeFormattedString(bbcode, text, bbCodeParameters));
            return this;
        }
        #endregion

        #region AppendLine
        public BBCodeWriter AppendLine()
        {
            _stringBuilder.AppendLine();
            return this;
        }
        public BBCodeWriter AppendLine(string text)
        {
            _stringBuilder.AppendLine(text);
            return this;
        }
        public BBCodeWriter AppendLine(string bbcode, string text)
        {
            _stringBuilder.AppendLine(CodeFormattedString(bbcode, text));
            return this;
        }
        public BBCodeWriter AppendLine(string bbcode, string text, params BBCodeParameter[] bbCodeParameters)
        {
            _stringBuilder.AppendLine(CodeFormattedString(bbcode, text, bbCodeParameters));
            return this;
        }
        #endregion

        #region Replace
        public BBCodeWriter Replace(string oldValue, string newValue)
        {
            _stringBuilder.Replace(oldValue, newValue);
            return this;
        }
        public BBCodeWriter Replace(string oldValue, string newValue, int startIndex, int count)
        {
            _stringBuilder.Replace(oldValue, newValue, startIndex, count);
            return this;
        }
        #endregion

        public BBCodeWriter Remove(int startIndex, int length)
        {
            _stringBuilder.Remove(startIndex, length);
            return this;
        }

        public override string ToString() { return _stringBuilder.ToString(); }

        #region Conversions
        public static implicit operator BBCodeWriter(StringBuilder stringBuilder) { return new BBCodeWriter(stringBuilder); }
        public static implicit operator StringBuilder(BBCodeWriter bbCodeWriter) { return bbCodeWriter._stringBuilder; }
        #endregion

        public static string CodeFormattedString(string bbcode, string text)
        {
            return $"[{bbcode}]{text}[/{bbcode}]";
        }

        public static string CodeFormattedString(string bbcode, string text, params BBCodeParameter[] bbCodeParameters)
        {
            string parameters = string.Join(" ", bbCodeParameters.Select(x => x.ToString()));
            return $"[{bbcode} {parameters}]{text}[/{bbcode}]";
        }
    }

    public class BBCodeParameter 
    {
        public string Attribute { get; } = "";
        public string Value { get; } = "";

        public BBCodeParameter(string value)
        {
            Attribute = "";
            Value = value;
        }
        public BBCodeParameter(string attribute, string value)
        {
            Attribute = attribute;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Attribute}={Value}";
        }
    }
}
