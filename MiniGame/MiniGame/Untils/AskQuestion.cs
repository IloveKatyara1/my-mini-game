namespace MiniGame.Untils
{
    internal class AskQuestion
    {
        public char AskQuestionMain(string question, params char[] letters)
        {
            Console.WriteLine(question);

            char.TryParse(Console.ReadLine(), out char Char);
            bool WasMatch = false;

            foreach (var item in letters)
            {
                WasMatch = item == Char;
            }

            while (!WasMatch)
            {
                Console.WriteLine(question);
                char.TryParse(Console.ReadLine(), out Char);
            }

            return Char;
        }
    }
}
