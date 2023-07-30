using System;

namespace MiniGame.Untils
{
    internal class AskQuestion
    {
        public string AskQuestionMain(string question, params string[] letters)
        {
            string Res = "";
            bool IsAllOk = false;

            while (!IsAllOk)
            {
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
