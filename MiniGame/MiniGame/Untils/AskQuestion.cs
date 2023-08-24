using System;

namespace MiniGame.Untils
{
    public static class AskQuestion
    {
        public static string Main(string question, params string[] letters)
        {
            string Res = "";
            bool IsAllOk = false;

            while (!IsAllOk)
            {
                Console.WriteLine();
                Console.WriteLine(question);

                Res = Console.ReadLine().ToUpper();

                foreach (var item in letters)
                {
                    if (!IsAllOk)
                        IsAllOk = item == Res;
                    else
                        break;
                }
            }

            return Res;
        }
    }
}
