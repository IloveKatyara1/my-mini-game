using MiniGame.Untils;

namespace MiniGame
{
    internal class MainLogic
    {
        private readonly AskQuestion _askQuestion = new();
        private readonly Player _player = new();

        public void WhereGo()
        {
            Console.WriteLine($"\nwhere would you like to go?");

            char Res = _askQuestion.AskQuestionMain("L/M/R", 'L', 'M', 'R');

            Random random = new Random();
            int randomNumber = random.Next(1, 101);

            int SimpleOpponent = 70;
            int DifficultOpponent = 15;
            int EmptyRoom = 10;
            int Something = 5;

            Console.WriteLine(randomNumber);

            if (randomNumber <= SimpleOpponent)
                Console.WriteLine("simpleOpponent");
            else if (randomNumber <= SimpleOpponent + DifficultOpponent)
                Console.WriteLine("difficultOpponent");
            else if (randomNumber <= SimpleOpponent + DifficultOpponent + EmptyRoom)
                Console.WriteLine("emptyRoom");
            else if (randomNumber <= SimpleOpponent + DifficultOpponent + EmptyRoom + Something)
                Console.WriteLine("something");

            ChooseAction();
        }

        public void ChooseAction()
        {
            Console.WriteLine($"\nplease, choose the action");

            char Res = _askQuestion.AskQuestionMain($"Look statistic: S;\nlook inventory: I\ncontinue your way: W", 'S', 'I', 'W');

            switch (Res)
            {
                case 'W':
                    WhereGo();
                    break;
                case 'S':
                    _player.ShowStatistic();
                    break;
                case 'I':
                    Console.WriteLine("Will soon");
                    break;
            }
        }
    }
}