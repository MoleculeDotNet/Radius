using System;
using Microsoft.SPOT;
using System.Text;
using System.Collections;

namespace IngenuityMicro.Radius.Core
{
    public static class Json
    {
        private enum TokenClass
        {
            tk_unknown, tk_comma, tk_colon, tk_string, tk_number, tk_null, tk_false, tk_true, tk_array_start, tk_array_end, tk_object_start, tk_object_end, tk_error, tk_end
        }

        public static object Deserialize(string json)
        {
            int offset = 0;
            TokenClass tk;

            var token = GetNextToken(json, ref offset, out tk);
            if (tk == TokenClass.tk_object_start)
                return DeserializeObject(json, ref offset);
            else if (tk == TokenClass.tk_array_start)
                return DeserializeArray(json, ref offset);
            else
                return null;
        }

        public static string Serialize(object obj)
        {
            if (obj is Hashtable)
                return SerializeDictionary((Hashtable)obj);
            else if (obj is ArrayList)
                return SerializeArray((ArrayList)obj);
            else
                return null;
        }

        private static string SerializeDictionary(Hashtable dict)
        {
            StringBuilder result = new StringBuilder();

            bool first = true;
            result.Append('{');
            foreach (DictionaryEntry item in dict)
            {
                if (!first)
                    result.Append(',');
                result.Append("\"" + item.Key + "\":");
                if (item.Value is int || item.Value is double)
                    result.Append(item.Value.ToString());
                else if (item.Value is string)
                    result.Append("\"" + (string)item.Value + "\"");
                else if (item.Value is Hashtable)
                    result.Append(SerializeDictionary((Hashtable)item.Value));
                else if (item.Value is ArrayList)
                    result.Append(SerializeArray((ArrayList)item.Value));
                first = false;
            }
            result.Append('}');

            return result.ToString();
        }

        public static string SerializeArray(ArrayList list)
        {
            StringBuilder result = new StringBuilder();

            bool first = true;
            result.Append('[');
            foreach (object item in list)
            {
                if (!first)
                    result.Append(',');
                if (item is int || item is double)
                    result.Append(item.ToString());
                else if (item is string)
                    result.Append("\"" + (string)item + "\"");
                else if (item is Hashtable)
                    result.Append(SerializeDictionary((Hashtable)item));
                else if (item is ArrayList)
                    result.Append(SerializeArray((ArrayList)item));
                first = false;
            }
            result.Append(']');

            return result.ToString();
        }

        private static object DeserializeObject(string json, ref int offset)
        {
            var result = new Hashtable();
            TokenClass tk = TokenClass.tk_unknown;
            string token;

            while (true)
            {
                string key = GetNextToken(json, ref offset, out tk);
                if (tk == TokenClass.tk_object_end)
                    break;
                if (tk != TokenClass.tk_string)
                    return null;

                string colon = GetNextToken(json, ref offset, out tk);
                if (tk != TokenClass.tk_colon)
                    break; // Error!

                object value = null;
                token = GetNextToken(json, ref offset, out tk);
                if (tk == TokenClass.tk_object_start)
                    value = DeserializeObject(json, ref offset);
                else if (tk == TokenClass.tk_array_start)
                    value = DeserializeArray(json, ref offset);
                else if (tk == TokenClass.tk_number)
                {
                    if (token.IndexOf('.') != -1)
                        value = double.Parse(token);
                    else
                        value = int.Parse(token);
                }
                else if (tk == TokenClass.tk_string)
                    value = token;
                else if (tk == TokenClass.tk_true)
                    value = true;
                else if (tk == TokenClass.tk_false)
                    value = false;
                else if (tk == TokenClass.tk_null)
                    value = null;
                else
                {
                    // error - return what we got so far. Things will probably go badly wrong after this
                    break;
                }

                result.Add(key, value);

                string comma = GetNextToken(json, ref offset, out tk);
                if (tk == TokenClass.tk_object_end)
                    break;
                if (tk != TokenClass.tk_comma)
                    break; // Error!
            }

            return result;
        }

        private static object DeserializeArray(string json, ref int offset)
        {
            var result = new ArrayList();
            TokenClass tk = TokenClass.tk_unknown;
            string token;

            while (true)
            {
                token = GetNextToken(json, ref offset, out tk);
                if (tk == TokenClass.tk_array_end)
                    break;

                object value = null;
                token = GetNextToken(json, ref offset, out tk);
                if (tk == TokenClass.tk_object_start)
                    value = DeserializeObject(json, ref offset);
                else if (tk == TokenClass.tk_array_start)
                    value = DeserializeArray(json, ref offset);
                else if (tk == TokenClass.tk_number)
                {
                    if (token.IndexOf('.') != 0)
                        value = double.Parse(token);
                    else
                        value = int.Parse(token);
                }
                else if (tk == TokenClass.tk_string)
                    value = token;
                else if (tk == TokenClass.tk_true)
                    value = true;
                else if (tk == TokenClass.tk_false)
                    value = false;
                else if (tk == TokenClass.tk_null)
                    value = null;
                else
                {
                    // error - return what we got so far. Things will probably go badly wrong after this
                    break;
                }

                result.Add(value);

                string comma = GetNextToken(json, ref offset, out tk);
                if (tk == TokenClass.tk_array_end)
                    break;
                if (tk != TokenClass.tk_comma)
                    break; // Error!
            }

            return result;
        }

        private static string GetNextToken(string json, ref int offset, out TokenClass tk)
        {
            var len = json.Length;

            // find the first non-whitespace character
            while (IsWhiteSpace(json[offset]) && offset < len)
                ++offset;
            if (offset == len)
            {
                tk = TokenClass.tk_end;
                return null;
            }

            if (json[offset] == '{' || json[offset] == '}' || json[offset] == '[' || json[offset] == ']' || json[offset] == ':' || json[offset] == ',')
            {
                switch (json[offset])
                {
                    case '{': tk = TokenClass.tk_object_start; break;
                    case '}': tk = TokenClass.tk_object_end; break;
                    case '[': tk = TokenClass.tk_array_start; break;
                    case ']': tk = TokenClass.tk_array_end; break;
                    case ':': tk = TokenClass.tk_colon; break;
                    case ',': tk = TokenClass.tk_comma; break;
                    default: tk = TokenClass.tk_error; break; // just to assign tk to make the compiler happy
                }
                return json[offset++].ToString();
            }

            // Is it a quoted string?
            if (json[offset]=='"' || json[offset]=='\'')
            {
                char quote;
                StringBuilder token = new StringBuilder();

                quote = json[offset++];
                if (offset == len)
                {
                    tk = TokenClass.tk_error;
                    return null;
                }
                while (json[offset] != quote)
                {
                    token.Append(json[offset++]);
                    if (offset == len)
                    {
                        tk = TokenClass.tk_error;
                        return null;
                    }
                }
                ++offset; // move past end quote
                tk = TokenClass.tk_string;
                return token.ToString();
            }

            // Is it a token? (true, false, null)
            if (json.Substring(offset, 4) == "true")
            {
                offset += 4;
                tk = TokenClass.tk_true;
                return "true";
            }

            if (json.Substring(offset, 5) == "false")
            {
                offset += 5;
                tk = TokenClass.tk_false;
                return "false";
            }

            if (json.Substring(offset, 4) == "null")
            {
                offset += 4;
                tk = TokenClass.tk_null;
                return "null";
            }

            // Is it a number?
            if (IsDigit(json[offset]))
            {
                StringBuilder numtoken = new StringBuilder();
                while (IsDigit(json[offset]))
                {
                    numtoken.Append(json[offset++]);
                    if (offset == len)
                    {
                        tk = TokenClass.tk_error;
                        return null;
                    }
                }
                tk = TokenClass.tk_number;
                return numtoken.ToString();
            }

            // an error - unexpected character
            tk = TokenClass.tk_error;
            return null;
        }

        private static bool IsWhiteSpace(char ch)
        {
            string ws = " \t\r\n";
            return ws.IndexOf(ch) != -1;
        }
        private static bool IsDigit(char ch)
        {
            return (ch >= '0' && ch <= '9') || ch == '+' || ch == '-' || ch == '.';
        }
    }
}
