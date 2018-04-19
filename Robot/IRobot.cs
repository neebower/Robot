using System.Collections.Generic;

namespace Robot
{
    public interface IRobot
    {
        //Строки на вход получаются из input.
        //Результат выполнения отдаёт метод.
        List<string> Evaluate(List<string> commands, IEnumerable<string> input);

        //Строки на вход получаются с помощью Console.ReadLine();
        //Результат выполнения пишется в консоль с помощью команды Console.WriteLine();
        void Evaluate(List<string> commands);
    }
}
