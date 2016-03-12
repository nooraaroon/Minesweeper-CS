using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinesweeperProject
{
    class ConsoleMinesweeperGame : MinesweeperGame
    {
        string startMessage = "Welcome to the game “Minesweeper”. Try to reveal all cells without mines. Use 'top' to view the scoreboard, 'restart' to start a new game and 'exit' to quit the game.";
        int Score;

        public ConsoleMinesweeperGame(int rows, int columns, int minesCount) : base(rows, columns, minesCount)
        {
            Score = 0; 
        }

        //skriv til konsoll
        private void print(String message)
        {
            Console.WriteLine(message);
        }

        //nullstil poengsum
        private void resetScore()
        {
            Grid.Reset();
            int Score = 0;
        }

        //les input
        private void read()
        {
            return Console.ReadLine().ToUpper().Trim();
        }

        //top vinnere
        private void top()
        {
            PrintScoreBoard();
            NextCommand();
        }

        private void noe(List<string> commandList)
        {
            int row = 0;
	        int column = 0;
	        bool tryParse = false;
	   
            if (commandList.Count < 2) 
               throw new CommandUnknownException();

            tryParse = (Int32.TryParse(commandList.ElementAt(0), out row) || tryParse);
	        tryParse = (Int32.TryParse(commandList.ElementAt(1), out column) || tryParse);
	   
            if (!tryParse) throw new CommandUnknownException();	
	 
	        if (Grid.RevealCell(row, column) == '*')
	        {
		        killed();
	        }
	        else
	        {
	            notKilled();
	        }
        }

        private void notKilled()
        {
           print(Grid.ToString());
           Score++;
           NextCommand(); 
        }

        private void killed()
        {
            Grid.mark('-');
	        Grid.RevealMines();
	        print(Grid.ToString());
	        print(String.Format("Booooom! You were killed by a mine. You revealed {0} cells without mines.", Score));
	        print("Please enter your name for the top scoreboard: ");
	        string playerName = Console.ReadLine();
	        ScoreBoard.Add(new ScoreRecord(playerName, Score));
	        Console.WriteLine();
	        PrintScoreBoard();
	        Start();
        }

        public override void Start()
        {
	       resetScore();
           print(startMessage);
           print(Grid.ToString());
           NextCommand();
        }

        public void NextCommand()//console -  output grid and message to request command
        {
            Console.Write("Enter row and column:");
            List<string> commandList = read().Split(' ').ToList();
            
            //if command list is empty
            if (commandList.Count == 0)
                NextCommand();
            try
            {
                string firstCommand = commandList.ElementAt(0);
                switch (firstCommand)
                {
                    case "RESTART": Start(); break;    
                    case "TOP": top(); break;
                    case "EXIT": Exit(); break;
                    default: noe(commandList); break;
                }
            }
        
            //if the row number or column number is wrong
            catch (InvalidCellException)
            {
                Console.WriteLine("Illegal move!");
                NextCommand();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                NextCommand();
            }
        }

        public void Exit()
        {
            print("Good bye!");
        }

        public void PrintScoreBoard()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Scoreboard:");
            ScoreBoard.Sort();
            int i = 0;
            foreach (ScoreRecord sr in ScoreBoard)
            {
                i++;
                sb.AppendFormat("{0}. {1} --> {2} cells \n", i, sr.PlayerName, sr.Score);
            }
            print(sb.ToString());
        }
    }
}
