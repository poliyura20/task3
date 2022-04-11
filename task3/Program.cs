using System;
using System.Security.Cryptography;
using System.Text;

namespace task3
{

    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length<3 || args.Length%2==0)
            {
                Console.WriteLine("Error404 (Entered incorrectly)");
                return ;
            }
            Random us = new Random();
            int cards = us.Next(args.Length);
            cards++;

            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] data = new byte[32];
            rng.GetBytes(data);

            Console.WriteLine("HMAC:\n{0}",RemoveSpecialChars(BitConverter.ToString(GenerateHMAC(args[cards-1], data)), '-'));

            Console.WriteLine("Available moves:");

            for (int i = 0; i < args.Length; i++)
            { 
                Console.WriteLine("{0}- {1}", i+1, args[i]);
            }
            Console.WriteLine("?- help");


            Console.Write("Enter your move:");
            string input = Console.ReadLine();

            int v;
            while (true)
            {
                if (String.Compare(input, "?") == 0)
                {
                    for (int i = 0; i < args.Length + 1; i++)
                    {
                        Console.WriteLine(HelpGame(i, args.Length, args));
                    }
                    Console.Write("Enter your move:");
                    input = Console.ReadLine();
                }
                else
                {
                    if (IsAllDigits(input))
                    {
                        v = Int32.Parse(input);
                        if (v <= 0 || v >= args.Length + 1)
                        {
                            Console.WriteLine("Error404 (Number entered incorrectly)");
                            Console.Write("Enter your move:");
                            input = Console.ReadLine();
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error404 (Number entered incorrectly)");
                        Console.Write("Enter your move:");
                        input = Console.ReadLine();
                    }
                }
            }
            Console.WriteLine("Your move: {0}\nComputer move: {1}",v,cards);
            Console.WriteLine(RulesOfTheGame(v,cards,args.Length));
            Console.WriteLine("HMAC key:\n{0}", RemoveSpecialChars(BitConverter.ToString(data), '-'));
      
        }
        public static bool IsAllDigits(string raw)
        {
            string s = raw.Trim(); 
            if (s.Length == 0) return false;
            for (int index = 0; index < s.Length; index++)
            {
                if (Char.IsDigit(s[index]) == false) return false;
            }
            return true;
        }
        public static string HelpGame(int str,int len,string[] vs)
        {
            string s = "";
            if (str == 0)
            {
                s = s.PadRight(16);
                for (int i = 0; i < len; i++)
                {
                    vs[i] = vs[i].PadRight(16);
                    s = String.Concat(s, vs[i]);
                }
            }
            else
            {
                for (int i = 0; i < len + 1; i++)
                {
                    if (i == 0)
                    {
                        vs[str-1] = vs[str-1].PadRight(16);
                        s = vs[str-1];
                    }
                    else
                    {
                        string vsi = "";
                        vsi = RulesOfTheGame(str, i, len);
                        vsi = vsi.PadRight(16);
                        s = String.Concat(s, vsi);
                    }
                }
            }
            return s;
        }
        public static string RulesOfTheGame(int v,int cards,int len)
        {
            if (v == cards)
            {
                return "Draw";
            }
            else
            {
                if (cards <= (len + 1) / 2)
                {
                    if (v > cards & v - cards <= (len - 1) / 2)
                    {
                        return "You win!";
                    }
                    else
                    {
                        return "Computer win(((";
                    }
                }
                else
                {
                    if (v < cards & cards - v <= (len - 1) / 2)
                    {
                        return "Computer win(((";
                    }
                    else
                    {
                        return "You win!";
                    }
                }
            }
        }
        public static string RemoveSpecialChars( string input, char targets)
        {
            string[] subStrings = input.Split(targets);
            string output = "";
            foreach(string subString in subStrings)
            {
                output = String.Concat(output, subString);
            }
            return output;
        }
      
        public static byte[] GenerateHMAC(string text, byte[] keyBytes)
        {

            var encoding = new ASCIIEncoding();

            var textBytes = encoding.GetBytes(text);

            Byte[] hashBytes;

            using (var hash = new HMACSHA256(keyBytes))

                hashBytes = hash.ComputeHash(textBytes);

            return hashBytes;
            
        }
    }
}
