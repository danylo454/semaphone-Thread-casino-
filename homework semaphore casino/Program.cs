using homework_semaphore_casino;

class Progream
{
    private static Semaphore semaphore = new Semaphore(1, 1);
    private static Random random = new Random();
    private static int namberWon = 0;
    private static string nameFile = "PlayerStats.txt";
    private static void WriteInFile(Player pl)
    {
        using (StreamWriter sw = new StreamWriter(File.Open(nameFile, FileMode.Append)))
        {
            sw.Write(pl.Name + "  [ 1000 ]   " + $"  [ {pl.Money} ]\n");
        };
        //StreamWriter sw = new StreamWriter(nameFile);
        //sw.Write(pl.Name + "  [ 1000 ]   " + $"  [ {pl.Money} ]");
        //sw.Close();

    }
    private static void ReadInFile()
    {
        if (!File.Exists(nameFile))
        {
            Console.Clear();
            Console.WriteLine("No one has played in the casino yet!!!");
        }
        else
        {
            using (StreamReader sr = new StreamReader(nameFile))
            {
                while (sr.Peek() >= 0)
                {
                    Console.Write((char)sr.Read());
                }
            };
        }

    }
    private static int RoundBet(Player pl)
    {
        int moneyPlayer = pl.Money;
        if (moneyPlayer > 100)
        {
            return random.Next(1, 100);
        }
        else
        {
            return moneyPlayer;
        }
    }
    private static bool LogicGame(Player pl)
    {
        Player player = pl;
        int raundBetPlayerMoney = 0;
        bool exit = false;
        do
        {
            namberWon = random.Next(0, 5);
            int namberBetPlayer = random.Next(0, 5);
            raundBetPlayerMoney = RoundBet(player);

            if (raundBetPlayerMoney == 0)
            {
                Console.WriteLine($"Player:{player.Name} Lose all money");
                exit = true; break;
            }
            else
            {
                int toPutOrNot = random.Next(1, 10);
                if (toPutOrNot == 1 && player.Money != 1000)
                {
                    Console.WriteLine($"Player:{player.Name} He left the casino");
                    exit = true; break;
                }
                else
                {
                    if (namberBetPlayer == namberWon)
                    {
                        player.Money += raundBetPlayerMoney;
                        Console.WriteLine("Player: " + $"{player.Name} Won: " + raundBetPlayerMoney + $" Now the wallet: {player.Money}");
                    }
                    else
                    {
                        player.Money -= raundBetPlayerMoney;
                        Console.WriteLine("Player: " + player.Name + " Lose: " + raundBetPlayerMoney + $" Now the wallet: {player.Money}");
                    }
                }
            }

        } while (exit != true);
        return exit;
    }
    private static void Game(object pl)
    {
        semaphore.WaitOne();
        Player player = pl as Player;
        Console.WriteLine($"============= {player.Name} ============");
        for (int i = 0; i < 10; i++)
        {
            if (LogicGame(player) == true)
            {
                WriteInFile(player);
                break;
            }
        }
        Console.WriteLine(new String('=', 40));

        semaphore.Release();
    }
    private static void MainMenu()
    {
        int input = 0;
        do
        {
            Console.Write($"1)Play Casin\n" +
                $"2)Show game statistics\n" +
                $"3)Exit\n" +
                $"Enter: ");
            input = int.Parse(Console.ReadLine());
            switch (input)
            {
                case 1:
                    {
                        Console.Clear();
                        Console.Write("Enter how many players will be playing : ");
                        int countPlayers = int.Parse(Console.ReadLine());
                        ParameterizedThreadStart start = new ParameterizedThreadStart(Game);
                        for (int i = 0; i < countPlayers; i++)
                        {
                            Thread thread = new Thread(start);
                            Player player = new Player();
                            player.Name = $"Player_{i}";
                            player.Money = 1000;
                            thread.Start(player);
                            thread.Join();
                        }
                        break;
                    }
                case 2: { Console.Clear(); ReadInFile(); break; }
                case 3: { break; }
            }
        } while (input != 3);
    }
    static void Main(string[] args)
    {

        MainMenu();

    }
}

