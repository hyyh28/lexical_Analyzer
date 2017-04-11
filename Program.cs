using System;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text.RegularExpressions;
namespace lexical_Analyzer
{
    internal class Program
    {
        public static Regex comments = new Regex("(//)([1-z]*)");
        public static Regex keywords = new Regex(@"int|real|if|then|else|while");
        public static Regex identifier = new Regex(@"([A-Z]|[a-z])+([0-9]*)");
        public static Regex operator_ = new Regex(@"\+|\-|\*|\/|\=|\==|\<|\<=|\>|\>=|\!=");
        public static Regex delimiters = new Regex(@"{|[|(|)]|}|;");
        public static Regex numbers = new Regex(@"-?[0-9]*[.]*[0-9]*");
        public struct token
        {
            public string tok;
            public int line;
            public int position;
            public string type;
            public token(string t, int l, int p)
            {
                tok = t;
                line = l;
                position = p;
                if (keywords.IsMatch(tok))
                {
                    type = "keyword";
                }
                else if (comments.IsMatch(tok)) type = "comment";
                else if (identifier.IsMatch(tok)) type = "identifier";
                else if (operator_.IsMatch(tok)) type = "operator";
                else if (delimiters.IsMatch(tok)) type = "delimiter";
                else if (numbers.IsMatch(tok)) type = "number";
                else type = "Error";
            }

            public void print()
            {
                Console.WriteLine(tok +" type: " + type +" line:" + line + " position:" + position);
            }
        }
        public static List<token> _tokens = new List<token>();
        public static void read_File(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    int numLine = 0;
                    while ((line = line = sr.ReadLine()) != null)
                    {
                        int first = line.Length;
                        int m = 0;
                        while (m < line.Length)
                        {
                            if (line[m] != ' ')
                            {
                                first = m;
                                break;
                            }
                            m++;
                        }
                        int start = first;
                        int end = start;
                        for (int i = first; i < line.Length; i++)
                        {
                            if (line[i] == '/' && line[i + 1] == '/')
                            {
                                numLine++;
                                string T = line.Substring(i);
                                _tokens.Add(new token(T,numLine,i));
                                break;
                            }
                            if (line[i] == ';')
                            {
                                string T = "";
                                T = T += line[i];
                                _tokens.Add(new token(T, numLine, i));
                                i++;
                                continue;
                            }
                            else if (line[i] == ' ' || i == line.Length-1)
                            {
                                end = i;
                                string t = "";
                                if (i != line.Length - 1)
                                {
                                    t = line.Substring(start, end - start);
                                }
                                else
                                {
                                    t = line.Substring(start);
                                }
                                _tokens.Add(new token(t,numLine,start));
                            }
                            int j = end;
                            while (j < line.Length)
                            {
                                if (line[j] != ' ')
                                {
                                    start = j;
                                    break;
                                }
                                j++;
                            }
                        }
                        Console.WriteLine(numLine + " :" + line);
                        numLine++;
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file can't open");
                Console.WriteLine(e.Message);
            }
        }
        public static void Main(string[] args)
        {
            read_File("./test.txt");
            foreach (token token_ in _tokens)
            {
                token_.print();
            }
        }
    }
}