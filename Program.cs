using System;
using System.Diagnostics;
using System.Globalization;

namespace Pi
{
    class Program
    {
        static string Pi;
        static string info;
        static string _continue;
        static string _continueEnd;
        static string selectAlg;
        static string error;
        static string notAalg;
        static string zeroError;
        static string selectIter;
        static string selDoOutput;
        static string resultF;
        static string resultI;
        static string resultEI;
        static string resultET;
        static string itwaslim;
        static string testResult;
        static string NikalantTest;
        static string revSquareTest;
        static string WallisTest;
        static string MadhavaTest;
        static string msLoc;
        static string sLoc;
        static string addTabN;
        static string addTabM;

        static void Main(string[] args)
        {
            Localize();

            Console.WriteLine(info);//Console.WriteLine("\t\tЧисло Пи: \t| 3,141592653589793238462643832\n\nЭта программа создана как для цели тестирования производительности одного ядра процессора,\nтак и для вычисления числа Пи за отведённое количество итераций");
            Console.WriteLine(_continue);//("\nНажмите любую клавишу, чтобы продолжить..");
            Console.ReadKey();

            uint iterations = 0;
            int choise = -1;
            int doOutput = -1;
            bool output = true;

            while (choise < 0 || choise > 5)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine(Pi);//("\t\tЧисло Пи: \t| 3,141592653589793238462643832\n\n");
                    Console.Write(selectAlg);//("Выберите алгоритм\n0 - Тест производительности\n1 - ряд Лейбница\n2 - ряд Нилаканты\n3 - ряд обратных квадратов\n4 - ряд Валлиса\n5 - ряд Мадхавы (ограничен 57 итерациями ввиду ограничений вычислений)\n\nВыбор: ");
                    choise = int.Parse(Console.ReadLine());
                    if (choise > 5 || choise < 0)
                    {
                        Console.WriteLine(notAalg);//("Данное значение не соответствует никакому из уже предусмотренных алгоритму!");
                        Console.ReadKey();
                    }
                }
                catch
                {
                    Console.WriteLine(error);//("Введённое значение недопустимо.");
                    Console.ReadKey();
                }
            }

            if (choise != 0)
            {
                while (iterations == 0)
                {
                    try
                    {
                        Console.Clear();
                        Console.WriteLine(Pi);//("\t\tЧисло Пи: \t| 3,141592653589793238462643832\n\n");
                        Console.WriteLine(selectAlg + choise);// ("Выберите алгоритм\n0 - Тест производительности\n1 - ряд Лейбница\n2 - ряд Нилаканты\n3 - ряд обратных квадратов\n4 - ряд Валлиса\n5 - ряд Мадхавы (ограничен 57 итерациями ввиду ограничений вычислений)\n\nВыбор: " + choise);
                        Console.Write(selectIter);//("\nВведите количество итераций для цикла вычислений: ");
                        string tempI = Console.ReadLine();
                        if (decimal.Parse(tempI) <= 0)
                        {
                            Console.WriteLine(zeroError);//("Значение должно быть больше нуля!");
                            Console.ReadKey();
                        }
                        else
                            iterations = uint.Parse(tempI);
                    }
                    catch
                    {
                        Console.WriteLine(error);//("Введённое значение недопустимо.");
                        Console.ReadKey();
                    }
                }

                while (doOutput < 0 || doOutput > 1)
                {
                    try
                    {
                        Console.Clear();
                        Console.WriteLine(Pi);//("\t\tЧисло Пи: \t| 3,141592653589793238462643832\n\n");
                        Console.WriteLine(selectAlg + choise);//("Выберите алгоритм\n0 - Тест производительности\n1 - ряд Лейбница\n2 - ряд Нилаканты\n3 - ряд обратных квадратов\n4 - ряд Валлиса\n5 - ряд Мадхавы (ограничен 57 итерациями ввиду ограничений вычислений)\n\nВыбор: " + choise);
                        Console.WriteLine(selectIter + iterations);//("\nВведите количество итераций для цикла вычислений: " + iterations);
                        Console.Write(selDoOutput);//("Выводить результат вычисления на каждой итерации? (0 - нет, 1 - да): ");
                        doOutput = int.Parse(Console.ReadLine());
                        if (doOutput < 0 || doOutput > 1)
                        {
                            Console.WriteLine(error);//("Введённое значение недопустимо");
                            Console.ReadKey();
                        }
                    }
                    catch
                    {
                        Console.WriteLine(error);//("Введённое значение недопустимо");
                        Console.ReadKey();
                    }
                }
                output = doOutput == 1 ? true : false;
                decimal result = 0.0m;
                long elapsedMS = 0;

                Stopwatch watcher = Stopwatch.StartNew();
                result = choise == 1 ? Leibnic(iterations, output) : choise == 2 ? Nilakanta(iterations, output) : choise == 3 ? reverseSquares(iterations, output) : choise == 4 ? Wallis(iterations, output) : Madhava(iterations, output);
                watcher.Stop();
                elapsedMS = watcher.ElapsedMilliseconds;

                long seconds = (long)Math.Floor( (double)(elapsedMS / 1000) );
                long milliseconds = elapsedMS - seconds * 1000;
                
                Console.WriteLine(resultF + (choise == 5 && iterations > 57 ? resultEI : iterations.ToString() + resultI) + result.ToString() + resultET + (seconds == 0 ? "" : seconds.ToString() + sLoc + " ") + milliseconds + msLoc);
                if (choise == 5 && iterations > 57)
                {
                    Console.WriteLine(itwaslim);//("\n* Количество итераций было ограничено до 57 ввиду ограничений");
                }
            } else
                Test();

            Console.WriteLine(_continueEnd);//("\nНажмите любую клавишу, чтобы завершить..");
            Console.ReadKey();
        }

        static decimal Leibnic(ulong iterations, bool output=true)
        {
            decimal result = 0;
            for (ulong i = 0; i < iterations; i++)
            {
                decimal delta = i % 2 == 0 ? 1.0m / ((i * 2) + 1) : -(1.0m / ((i * 2) + 1));
                result += delta;
                if (output)
                    Console.WriteLine( (i + 1).ToString() + " | " + (result * 4).ToString() );

            }
            return (result * 4);
        }

        static decimal Nilakanta(ulong iterations, bool output = true)
        {
            decimal result = 3;
            int lastNum = 2;
            for (ulong i = 0; i < iterations; i++)
            {
                decimal delta = i % 2 == 0 ? 4.0m / ((lastNum) * (lastNum + 1) * (lastNum + 2)) : -(4.0m / ((lastNum) * (lastNum + 1) * (lastNum + 2)));
                result += delta;
                lastNum += 2;
                if (output)
                    Console.WriteLine((i + 1).ToString() + " | " + result.ToString());
            }
            return result;
        }

        static decimal reverseSquares(ulong iterations, bool output = true)
        {

            decimal result = 0;
            for (ulong i = 1; i <= iterations; i++)
            {
                result += 1.0m / (i * i);
                if (output)
                    Console.WriteLine(i.ToString() + " | " + Sqrt(result * 6).ToString());
            }
            return Sqrt(result * 6);
        }

        static decimal Wallis(ulong iterations, bool output = true)
        {
            decimal result = 2;
            decimal lastFDigit = 2;
            decimal lastSDigit = 3;

            for (ulong i = 1; i <= iterations; i++)
            {
                result *= lastFDigit / lastSDigit;
                lastFDigit += 2;
                if (output)
                    Console.WriteLine(i.ToString() + " | " + (result * 2).ToString());

                result *= lastFDigit / lastSDigit;
                lastSDigit += 2;
                if (output)
                    Console.WriteLine(i.ToString() + " | " + (result * 2).ToString());
            }

            return result * 2;
        }

        static decimal Madhava(ulong iterations, bool output = true)
        {
            decimal result = Sqrt(12);
            if (iterations > 56)
                iterations = 56;
            for (ulong i = 1; i <= iterations; i++)
            {
                result += i % 2 == 0 ? (Sqrt(12) / ( (i * 2 + 1) * (decimal)Math.Pow(3, i)) ) : -(Sqrt(12) / ( (i * 2 + 1) * (decimal)Math.Pow(3, i)) );
                if (output)
                    Console.WriteLine(i.ToString() + " | " + result.ToString());
            }
            return result;
        }

        static void Test()
        {
            Stopwatch time10kOperations;
            long firstMS = 0;
            long secondMS = 0;
            long thirdMS = 0;
            long fourthMS = 0;
            long fifthMS = 0;
            decimal f = 0;
            decimal s = 0;
            decimal t = 0;
            decimal ft = 0;
            decimal ff = 0;

            time10kOperations = Stopwatch.StartNew();
            f = Leibnic(1000000, false);
            time10kOperations.Stop();
            firstMS = time10kOperations.ElapsedMilliseconds;

            time10kOperations = Stopwatch.StartNew();
            s = Nilakanta(1000000, false);
            time10kOperations.Stop();
            secondMS = time10kOperations.ElapsedMilliseconds;

            time10kOperations = Stopwatch.StartNew();
            t = reverseSquares(1000000, false);
            time10kOperations.Stop();
            thirdMS = time10kOperations.ElapsedMilliseconds;

            time10kOperations = Stopwatch.StartNew();
            ft = Wallis(1000000, false);
            time10kOperations.Stop();
            fourthMS = time10kOperations.ElapsedMilliseconds;

            time10kOperations = Stopwatch.StartNew();
            ff = Madhava(1000000, false);
            time10kOperations.Stop();
            fifthMS = time10kOperations.ElapsedMilliseconds;


            long secondsF = (long)Math.Floor((double)(firstMS / 1000));
            long millisecondsF = firstMS - secondsF * 1000;

            long secondsS = (long)Math.Floor((double)(secondMS / 1000));
            long millisecondsS = secondMS - secondsS * 1000;

            long secondsT = (long)Math.Floor((double)(thirdMS / 1000));
            long millisecondsT = thirdMS - secondsT * 1000;

            long secondsFT = (long)Math.Floor((double)(fourthMS / 1000));
            long millisecondsFT = fourthMS - secondsFT * 1000;

            long secondsFF = (long)Math.Floor((double)(fifthMS / 1000));
            long millisecondsFF = fifthMS - secondsFF * 1000;


            Console.WriteLine(testResult + (secondsF == 0 ? "" : secondsF.ToString() + sLoc + " ") + millisecondsF + msLoc + " \t\t| " + f 
                            + NikalantTest + (secondsS == 0 ? "" : secondsS.ToString() + sLoc + " ") + millisecondsS + msLoc + addTabN + " \t| " + s
                            + revSquareTest + (secondsT == 0 ? "" : secondsT.ToString() + sLoc + " ") + millisecondsT + msLoc + " \t| " + t
                            + WallisTest + (secondsFT == 0 ? "" : secondsFT.ToString() + sLoc + " ") + millisecondsFT + msLoc + " \t\t| " + ft
                            + MadhavaTest + (secondsFF == 0 ? "" : secondsFF.ToString() + sLoc + " ") + millisecondsFF + msLoc + addTabM + " \t| " + ff
                );
        }

        static void Localize()
        {
            bool isRussian = false;
            if (CultureInfo.InstalledUICulture.EnglishName.Contains("Russian"))
            {
                isRussian = true;
            }
            Pi = isRussian ? "\t\tЧисло Пи: \t| 3,141592653589793238462643832\n\n" : "\t\tThe Pi Number: \t| 3,141592653589793238462643832\n\n";
            info = isRussian ? "\t\tЧисло Пи: \t| 3,141592653589793238462643832\n\nЭта программа создана как для цели тестирования производительности одного ядра процессора,\nтак и для вычисления числа Пи за отведённое количество итераций" :
                "\t\tThe Pi Number: \t| 3,141592653589793238462643832\n\nThis program was created for testing perfomance of one of CPU cores and for computing Pi for limited iterations.";
            _continue = isRussian ? "\nНажмите любую клавишу, чтобы продолжить.." : "\nPress any key to continue...";
            _continueEnd = isRussian ? "\nНажмите любую клавишу, чтобы завершить.." : "\nPress any key to exit...";
            selectAlg = isRussian ? "Выберите алгоритм\n0 - Тест производительности\n1 - ряд Лейбница\n2 - ряд Нилаканта\n3 - ряд обратных квадратов\n4 - ряд Валлиса\n5 - ряд Мадхавы (ограничен 57 итерациями ввиду ограничений вычислений)\n\nВыбор: " :
                "Select an algorithm\n0 - Perfomance test\n1 - Leibniz series\n2 - Nilakantha series\n3 - Inverse squares series\n4 - Wallis series\n5 - Madhava series (limited to 57 iterations due computational limitations)\n\nSelect: ";
            error = isRussian ? "Введённое значение недопустимо." : "This value isn't valid";
            notAalg = isRussian ? "Данное значение не соответствует никакому из уже предусмотренных алгоритму!" : "This value does not match to any of the existing algorithms!";
            zeroError = isRussian ? "Значение должно быть больше нуля!" : "The value must be more than zero!";
            selectIter = isRussian ? "\nВведите количество итераций для цикла вычислений: " : "\nEnter iterations count for computation cycle: ";
            selDoOutput = isRussian ? "Выводить результат вычисления на каждой итерации? (0 - нет, 1 - да): " : "Print computations result at each iteration? (0 - no, 1 - yes): ";
            resultF = isRussian ? "\nРезультат за " : "";
            resultI = isRussian ? " вычислений: " : " iterations: ";
            resultEI = isRussian ? "57 вычислений*: " : "57 iterations*";
            resultET = isRussian ? "\nЗатраченное время: " : "\nElapsed time: ";
            itwaslim = isRussian ? "\n* Количество итераций было ограничено до 57 ввиду ограничений" : "\n* Iterations were limited to 57 due computation limitations";
            testResult = isRussian ? "\nРезультаты теста (1 млн. итераций):\nРяд Лейбница: " : "\nTest results (1M iteraions):\nLeibniz series: ";
            NikalantTest = isRussian ? "\nРяд Нилаканта: " : "\nNilakantha series: ";
            revSquareTest = isRussian ? "\nРяд обр. квадратов: " : "\nInverse squares series: ";
            WallisTest = isRussian ? "\nРяд Валлиса: " : "\nWallis series: ";
            MadhavaTest = isRussian ? "\nРяд Мадхавы (57 ит.): " : "\nMadhava series: ";
            msLoc = isRussian ? " мс" : " ms";
            sLoc = isRussian ? " с" : " ms";
            addTabN = isRussian ? "\t" : "";
            addTabM = isRussian ? "" : "\t";
        }

        static decimal Sqrt(decimal x, decimal epsilon = 0.0M)
        {
            decimal current = (decimal)Math.Sqrt((double)x);
            decimal previous;
            do
            {
                previous = current;
                if (previous == 0.0M) return 0;
                current = (previous + x / previous) / 2;
            }
            while (Math.Abs(previous - current) > epsilon);
            return current;
        }
    }
}
