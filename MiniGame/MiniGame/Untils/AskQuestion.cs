using System.Runtime.InteropServices;

namespace MiniGame.Untils
{
    internal class AskQuestion
    {
        public char AskQuestionMain(string question, params char[] letters)
        {
            char Res = '\0';
            bool IsAllOk = false;

            while(!IsAllOk)
            {
                Console.WriteLine(question);

                bool WasMatch = char.TryParse(Console.ReadLine(), out Res);

                if (WasMatch)
                    Res = Char.ToUpper(Res);

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
