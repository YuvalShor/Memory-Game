﻿using System;
using System.Runtime.InteropServices;

namespace B20_Ex02
{
    internal class Menu
    {
        public bool Run(out string o_PlayerName1, out string o_PlayerName2, out int o_Width, out int o_Height)
        {
            Console.WriteLine("Welcome to Memory Game!");
            Console.WriteLine("Please enter your name:");
            o_PlayerName1 = Console.ReadLine();

            Console.WriteLine("Hello {0}! Please choose a game mode:", o_PlayerName1);
            bool isPlayerVsPlayer = SelectGameMode(out o_PlayerName2);

            GetBoardSize(out o_Height, out o_Width);

            return isPlayerVsPlayer;
        }

        public void GetBoardSize(out int o_Height, out int o_Width)
        {
            bool isBoardSizeEven = false;
            o_Height = 6;
            o_Width = 6;

            while (!isBoardSizeEven)
            {
                Console.WriteLine("Choose board height:");
                o_Height = GameUI.GetNumberInRange(4, 6);

                Console.WriteLine("Choose board width:");
                o_Width = GameUI.GetNumberInRange(4, 6);

                isBoardSizeEven = o_Height * o_Width % 2 == 0;
            }
        }

        private bool SelectGameMode(out string o_PlayerName2)
        {
            bool isPlayerVsPlayer = false;

            Console.WriteLine("1) Player vs. Player");
            Console.WriteLine("2) Player vs. Computer");

            string playerSelection = validateSelection();

            o_PlayerName2 = "Computer";

            if (playerSelection == "1")
            {
                Console.WriteLine("Please enter player 2 name:");
                o_PlayerName2 = Console.ReadLine();
                isPlayerVsPlayer = true;
            }

            return isPlayerVsPlayer;
        }

        private string validateSelection()
        {
            string playerSelection = Console.ReadLine();

            while(playerSelection != "1" && playerSelection != "2")
            {
                Console.WriteLine("Wrong option entered. Please enter a different number: ");
                playerSelection = Console.ReadLine();
            }

            return playerSelection;
        }
    }
}