using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueLike
{
    public class Entity
    {
        
        public int health = 0;
        public int armor = 0;
        public int magic = 0;
        public int magicresist = 0;
        public int attackspeed = 0;
        public int X = 0;
        public int Y = 0;
        public int OldX = 0;
        public int OldY = 0;
        public string Char="§";
        public Entity() { }
        public virtual void MoveXY(map M, int X, int Y)
        {
            if (X < M.Width && X > 0 && Y < M.Height && Y >= 0)
            {
                //replace the old x,y position of the player with map's character.
                Console.SetCursorPosition(this.OldX, this.OldY);
                Console.Write(M.CharArray[this.OldX, this.OldY]);

                Console.SetCursorPosition(X, Y);
                Console.Write(this.Char);
                //This is done on purpose. Make sure to always set this way
                //to prevent disappearing text from occuring
                this.X = this.OldX = X;
                this.Y = this.OldY = Y;
            }
            else
            {
                Console.SetCursorPosition(this.OldX, this.OldY);
                Console.Write(this.Char);
            }
        }
        public virtual void MoveBack()
        {
            this.X = this.OldX;
            this.Y = this.OldY;
            Console.SetCursorPosition(X, Y);
            Console.Write(this.Char);
        }
        public virtual void MoveUp(map M)
        {
            this.MoveXY(M, this.X, this.Y - 1);
        }
        public virtual void MoveDown(map M)
        {
            this.MoveXY(M, this.X, this.Y + 1);
        }
        public virtual void MoveLeft(map M)
        {
            this.MoveXY(M, this.X - 1, this.Y);
        }
        public virtual void MoveRight(map M)
        {
            this.MoveXY(M, this.X + 1, this.Y);
        }
    }
    public class player : Entity
    {
        //initialize player stats
        public player() 
        {
            this.armor = 0;
            this.attackspeed = 0;
            this.Char = "P";
            this.health = 0;
            this.magic = 0;
            this.magicresist = 0;
            //This is done on purpose. Make sure to always set this way
            //to prevent disappearing text from occuring
            this.X = this.OldX = 10;
            this.Y = this.OldY = 4;
            Console.SetCursorPosition(X, Y);
            Console.Write(this.Char);
        }
    }
    public class monster : Entity
    {
        public monster() { }
    }

    public class map
    {
        public int storedX;
        public int storedY;
        public char[,] CharArray;
        public int Width;
        public int Height;
        public bool initialized = false;

        public map() { }
        public map(string filename)
        {
            InitializeMap(System.IO.File.ReadAllLines("map.txt"));
        }
        public void InitializeMap(string[] lines)
        {
            this.Width = lines[0].Length;
            this.Height = lines.Length;
            CharArray = new char[this.Width, this.Height];

            int j = 0;
            foreach (string line in lines)
            {
                int i = 0;
                foreach (char ch in line)
                {
                    // make sure to skip over the newline
                    if (i < this.Width)
                    {
                        CharArray[i, j] = ch;
                        ++i;
                    }
                }
                ++j;
            }
            initialized = true;
        }
        public void printMapXY(int X, int Y)
        {
            if (initialized == true)
                Console.Write(CharArray[X, Y]);
            else
                Console.Write("Map was not initialized. Please make sure to initialize map before you try to print\n");
        }
        public void printMapAll()
        {
            if (initialized == true)
            {
                for (int j = 0; j < this.Height; j++)
                {
                    for (int i = 0; i < this.Width; i++)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(this.CharArray[i, j]);
                    }
                }
            }
            else
                Console.Write("Map was not initialized. Please make sure to initialize map before you try to print\n");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            

            map testMap = new map("map.txt");
            testMap.printMapAll();
            player Player = new player();

            // Prevent example from ending if CTL+C is pressed.
            Console.TreatControlCAsInput = true;
            // Hide the cursor from blinking while we are running the game
            Console.CursorVisible = false;
            //key variable is used for the input switch below
            ConsoleKey Key;
            do
            {
                //Console.Write(" --- You pressed ");
                Key = System.Console.ReadKey(true).Key;
                switch (Key)
                {
                        //respond to player's input
                    case ConsoleKey.UpArrow:
                        //move the player up one space
                        Player.MoveUp(testMap);
                        break;
                    case ConsoleKey.DownArrow:
                        
                        Player.MoveDown(testMap);

                        break;
                    case ConsoleKey.LeftArrow:

                        Player.MoveLeft(testMap);

                        break;
                    case ConsoleKey.RightArrow:

                        Player.MoveRight(testMap);

                        break;
                }
                //Console.WriteLine(System.Console.ReadKey(true).Key.ToString());
            } while (Key != ConsoleKey.Escape);
        }
    }
}
