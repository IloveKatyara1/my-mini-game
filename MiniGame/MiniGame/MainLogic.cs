using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniGame.Untils;
using MiniGame;

namespace MiniGame
{
    internal class MainLogic
    {
        private readonly AskQuestion _askQuestion = new ();

        private readonly Player _player = new ();
        public void WhereGo()
        {
            Console.WriteLine("where would you like to go?");
            Console.WriteLine("L/M/R");

            char Char = _askQuestion.AskQuestionMain("L/M/R", 'L', 'M', 'R');
        }

        public void ChooseAction()
        {
            Console.WriteLine("please, choose the action");

            char Char = _askQuestion.AskQuestionMain($"Look statistic: S;\nlook inventory: I\ncontinue your way: W", 'S', 'I', 'W');

            switch (Char)
            {
                case 'W':
                    WhereGo();
                    break;
                 case 'S':
                    _player.Statistic();
                    break;
            }
        }
    }
}
