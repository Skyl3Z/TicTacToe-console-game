using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tictactoe_console_game
{
    internal class Program
    {
        static void printBoard(char[] board)
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine("|       |       |       |");
            Console.WriteLine($"|   {board[0]}   |   {board[1]}   |   {board[2]}   |");
            Console.WriteLine("|       |       |       |");
            Console.WriteLine("|-----------------------|");
            Console.WriteLine("|       |       |       |");
            Console.WriteLine($"|   {board[3]}   |   {board[4]}   |   {board[5]}   |");
            Console.WriteLine("|       |       |       |");
            Console.WriteLine("|-----------------------|");
            Console.WriteLine("|       |       |       |");
            Console.WriteLine($"|   {board[6]}   |   {board[7]}   |   {board[8]}   |");
            Console.WriteLine("|       |       |       |");
            Console.WriteLine("-------------------------");
        }
        static int checkWin(char[] board)
        {
            if (isWinner(board, 'X') || isWinner(board, 'O'))
                return 1;

            if (board.All(c => c == 'X' || c == 'O'))
                return -1;

            return 0;
        }

        static int Evaluate(char[] board, char aiChar, char playerChar, int depth)
        {
            if (isWinner(board, aiChar))
                return 10 - depth;

            if (isWinner(board, playerChar))
                return -10 + depth;

            if (board.All(c => c == 'X' || c == 'O'))
                return 0;

            return int.MinValue;
        }


        static bool isWinner (char[] board, char p)
        {
            int[,] wins =
            {
                {0,1,2},{3,4,5},{6,7,8},
                {0,3,6},{1,4,7},{2,5,8},
                {0,4,8},{2,4,6}
            };

            for (int i = 0; i < wins.GetLength(0); i++)
            {
                if (board[wins[i, 0]] == p &&
                    board[wins[i, 1]] == p &&
                    board[wins[i, 2]] == p)
                    return true;
            }
            return false;
        }

        static int Minimax(char[] board, int depth, bool isMax, char aiChar, char playerChar)
        {
            int eval = Evaluate(board, aiChar, playerChar, depth);

            if (eval != int.MinValue)
            {
                return eval;
            }

            if (isMax)
            {
                int best = int.MinValue;

                for(int i = 0; i < board.Length; i++)
                {
                    if (board[i] != 'X' && board[i] != 'O')
                    {
                        char temp = board[i];
                        board[i] = aiChar;
                        best = Math.Max(best, Minimax(board, depth, false, aiChar, playerChar));
                        board[i] = temp;
                    }
                }

                return best;
            } else
            {
                int best = int.MaxValue;

                for (int i = 0; i < board.Length; i++)
                {
                    if (board[i] != 'X' && board[i] != 'O')
                    {
                        char temp = board[i];
                        board[i] = playerChar;
                        best = Math.Min(best, Minimax(board, depth + 1, true, aiChar, playerChar));
                        board[i] = temp;
                    }
                }

                return best;
            }
        }

        static int AiMove(char[] board, char aiChar, char playerChar)
        {
            int bestVal = int.MinValue;
            int bestMove = -1;

            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] != 'X' && board[i] != 'O')
                {
                    char temp = board[i];
                    board[i] = aiChar;
                    int moveVal = Minimax(board, 0, false, aiChar, playerChar);
                    board[i] = temp;
                    if (moveVal > bestVal)
                    {
                        bestMove = i;
                        bestVal = moveVal;
                    }
                }
            }

            return bestMove;
        }


            static void Main(string[] args)
        {
            char[] board = {'1', '2', '3', '4', '5', '6', '7', '8', '9'};
            int player = 1;
            char choice = ' ';
            int mode = 0;
            int lastPlayer = 0;
            Player player1 = new Player();
            Player player2 = new Player();
            int playerCount = 0;
            Dictionary<int, Player> players = new Dictionary<int, Player>();


            while (true)
            {
                Console.WriteLine("Welcome to Tic Tac Toe! \n");
                Console.WriteLine("choose which mode do you want to play \n 1. Player vs Player \n 2. Player vs AI");
                bool valid = int.TryParse(Console.ReadLine(), out int i);
                if (i == 1 || i == 2)
                {
                    mode = i;
                    break;
                }
            }

            if (mode  == 1)
            {
                for (int i = 1; i <= 2; i++)
                {
                    while (true)
                    {
                        Console.WriteLine($"Enter name for player {i}:");
                        string name = Console.ReadLine()?.ToLower();

                        if (string.IsNullOrWhiteSpace(name))
                            continue;

                        if (players.Values.Any(p => p.Name == name))
                        {
                            Console.WriteLine("Player with this name already exists.");
                            continue;
                        }
                        playerCount++;
                        if (i == 1)
                        {
                            player1 = new Player { Name = name };
                            players.Add(playerCount, player1);
                        }
                        else
                        {
                            player2 = new Player { Name = name };
                            players.Add(playerCount, player2);
                        }
                        break;
                    }
                }


                do
                {
                    Console.Clear();
                    printBoard(board);

                    lastPlayer = player;

                    if (player == 1)
                    {
                        Console.WriteLine($"its the turn of {player1.Name}(X)");

                        choice = Console.ReadKey().KeyChar;

                        if (!board.Contains(choice) || choice > '9' || choice < '1')
                        {
                            Console.WriteLine($"{choice} is not a valid choice.");
                            Thread.Sleep(500);
                            continue;
                        }

                        player = 2;

                        for (int i = 0; i < board.Length; i++)
                        {
                            if (board[i] == choice)
                            {
                                board[i] = 'X';
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"its the turn of {player2.Name}(O)");

                        choice = Console.ReadKey().KeyChar;

                        if (!board.Contains(choice) || choice > '9' || choice < '1')
                        {
                            Console.WriteLine($"{choice} is not a valid choice.");
                            Thread.Sleep(500);
                            continue;
                        }

                        player = 1;

                        for (int i = 0; i < board.Length; i++)
                        {
                            if (board[i] == choice)
                            {
                                board[i] = 'O';
                            }
                        }
                    }


                } while (checkWin(board) == 0);

            } else
            {
                while (true)
                {
                    Console.WriteLine("Enter your name:");
                    string name = Console.ReadLine()?.ToLower();

                    if (string.IsNullOrWhiteSpace(name))
                        continue;

                    playerCount++;

                    player1 = new Player { Name = name };
                    players.Add(playerCount, player1);
                    break;
                }
                
                player2 = new Player { Name = "AI" };
                Random rand = new Random();
                player = rand.Next(1, 3);

                while (checkWin(board) == 0)
                {
                    Console.Clear();
                    printBoard(board);
                    lastPlayer = player;

                    if (player == 1)
                    {
                        Console.WriteLine($"its the turn of {player1.Name}(X)");

                        choice = Console.ReadKey().KeyChar;

                        if (!board.Contains(choice) || choice > '9' || choice < '1')
                        {
                            Console.WriteLine($"{choice} is not a valid choice.");
                            Thread.Sleep(500);
                            continue;
                        }

                        for (int i = 0; i < board.Length; i++)
                        {
                            if (board[i] == choice)
                            {
                                board[i] = 'X';
                            }
                        }

                        player = 2;
                    } else
                    {
                        if (checkWin(board) != 0)
                            break;


                        int move = AiMove(board, 'O', 'X');
                        if (move == -1)
                            break;

                        board[move] = 'O';
                        player = 1;
                    }
                }

            }

            if (checkWin(board) == 1)
            {
                if (lastPlayer == 1)
                {                    
                    Console.Clear();
                    printBoard(board);                   
                    player1.winCount++;
                    Console.WriteLine($"{player1.Name} (X) wins! Wincount: {player1.winCount}");
                }
                else
                {
                    Console.Clear();
                    printBoard(board);                   
                    player2.winCount++;
                    Console.WriteLine($"{player2.Name} (O) wins! Wincount: {player2.winCount}");
                }
            }
            else
            {
                Console.Clear();
                printBoard(board);
                Console.WriteLine("DRAW");
            }

            foreach (var p in players.Values)
            {
                Console.WriteLine($"{p.Name} - Wincount: {p.winCount}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}