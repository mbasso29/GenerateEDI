namespace WPF_EDI.Models
{
    public static class BadChars
    {
        public static readonly char[] Interchange_String = new char[] { '~', '`', ':', '!', '@', '#', '$', '%', '^', '*', '_', '=', '?', '/', '>', '<', '.', '\'', '{', '}', '[', ']', '|', '\\', '\"' };
        public static readonly char[] Interchange_Account_String = new char[] { ' ', '~', '`', ':', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '=', '?', '/', '>', '<', ',', ';', '.', '\'', '{', '}', '[', ']', '|', '\\', '\"' };

        public static readonly char[] Phone = new char[] { ' ', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '=', '?', '/', '>', '<', ',', '.' };
        public static readonly char[] Zip = new char[] { ' ', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '=', '?', '/', '>', '<', ',', '.' };
        public static readonly char[] AccountNo = new char[] { ' ', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '=', '?', '/', '>', '<', ',', '.', ';', ':' };
        public static readonly char[] Identifier = new char[] { ' ', '~', '`', ':', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '=', '?', '/', '>', '<', ',', ';', '.', '\'', '{', '}', '[', ']', '|', '\\', '\"' };
        public static readonly char[] Diagnosis = new char[] { ' ', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '=', '?', '/', '>', '<', ',', '.' };
        public static readonly char[] Str = new char[] { '*', '~', ':', '\'', '.', '\"' };
        public static readonly char[] Name = new char[] { '~', '`', ':', '!', '@', '#', '$', '%', '^', '*', '_', '=', '?', '/', '>', '<', '.', '\'', '{', '}', '[', ']', '|', '\\', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '\"' };
        public static readonly char[] Email = new char[] { '*', '~', ':', '\'', '\"' };
        public static readonly char[] City = new char[] { '*', '~', ':', '\'', '.', '\"' };
    }
}
