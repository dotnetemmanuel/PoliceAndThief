﻿namespace Tjuv___Polis
{
    public class Person
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public bool Immobilized { get; set; }
        public int ImmobilizedCountdown { get; set; }
        public int directionX, directionY;
        int directionCooldown = 0;

        Random rng;

        //Method moving people 10 steps in same direction then randomizing new directions for the next 10 steps, etc.
        public void Move()
        {
            rng = new Random();

            if (directionCooldown == 0)
            {
                directionX = 0;
                directionY = 0;
                int movement = rng.Next(0, 8);
                directionCooldown = 10;
                switch (movement)
                {
                    case 0:
                        directionY = 1;
                        break;
                    case 1:
                        directionY = 1;
                        directionX = 1;
                        break;
                    case 2:
                        directionX = 1;
                        break;
                    case 3:
                        directionY = -1;
                        directionX = 1;
                        break;
                    case 4:
                        directionY = -1;
                        break;
                    case 5:
                        directionY = -1;
                        directionX = -1;
                        break;
                    case 6:
                        directionX = -1;
                        break;
                    case 7:
                        directionY = 1;
                        directionX = -1;
                        break;
                }
            }

            if (!Immobilized)
            {
                //Unique movement set for the Vigilante, moves two squares instead of one, and checks its perimeter for interactable objects once per square moved.
                //Also checks if the Vigilante still has active time, otherwise despawn. 
                if (this is Vigilante vigilante)
                {
                    PosX += directionX;
                    PosY += directionY;
                    vigilante.DetectProximity();
                    PosX += directionX;
                    PosY += directionY;
                    vigilante.DetectProximity();

                    vigilante.ActiveTime--;

                    if (vigilante.ActiveTime <= 0) 
                    {
                        Vigilante.DespawnVigilante();
                    }

                }
                else
                {
                    PosX += directionX;
                    PosY += directionY;
                }
                directionCooldown--;
            }

            if (Immobilized)
            {
                ImmobilizedCountdown--;
            }

            //Cooldown for prison sentence, returns to city
            if (Program.prisonList.Contains(this))
            {
                (((Thief)this).SentenceTime)--;
                CheckOutOfBoundsAlt();
                if ((((Thief)this).SentenceTime) == 0)
                {
                    Random rng = new Random();
                    this.PosX = rng.Next(0, Program.citySizeX);
                    this.PosY = rng.Next(0, Program.citySizeY);
                    Program.cityList.Add(this);
                    Program.prisonList.Remove(this);
                }
            }
            else if (Program.poorHouseList.Contains(this))
            {
                CheckOutOfBoundsAlt();
            }
            else
            {
                CheckOutOfBoundsCity();
            }
        }

        //Retrieving info about person type, inventory and position
        public void GetInfo()
        {
            if (this is Civilian civilian)
            {
                Console.Write("Medborgare, " + "medhavande föremål: ");
                foreach (Item item in civilian.Possessions)
                {
                    Console.Write(item.Name + ", ");
                }
                Console.Write("På plats: " + this.PosX + "," + this.PosY);
                Console.WriteLine();

            }

            if (this is Police police)
            {
                Console.Write("Polis, " + "medhavande föremål: ");
                foreach (Item item in police.Confiscated)
                {
                    Console.Write(item.Name + ", ");
                }
                Console.Write("På plats: " + this.PosX + "," + this.PosY);
                Console.WriteLine();

            }

            if (this is Thief thief)
            {
                Console.Write("Tjuv, " + "medhavande föremål: ");
                foreach (Item item in thief.Loot)
                {
                    Console.Write(item.Name + ", ");
                }
                Console.Write("På plats: " + this.PosX + "," + this.PosY);
                Console.WriteLine();

            }

            if (this is Vigilante vigilante)
            {
                Console.Write("Vigilante, " + "medhavande föremål: ");
                foreach (Item item in vigilante.Gadgets)
                {
                    Console.Write(item.Name + ", ");
                }
                Console.Write("På plats: " + this.PosX + "," + this.PosY);
                Console.WriteLine();
            }

        }

        public void CheckOutOfBoundsCity()                                      //Check if person if out of bound and replace position to other end. 
        {
            if (PosX < 0 && PosY < 0)
            {
                PosY = Program.citySizeY - 1;
                PosX = Program.citySizeX - 1;
            }
            else if (PosX > Program.citySizeX - 1 && PosY > Program.citySizeY - 1)
            {
                PosY = 0;
                PosX = 0;
            }
            else if (PosX > Program.citySizeX - 1 && PosY < 0)
            {
                PosX = 0;
                PosY = Program.citySizeY - 1;
            }
            else if (PosX < 0 && PosY > Program.citySizeY - 1)
            {
                PosX = Program.citySizeX - 1;
                PosY = 0;
            }

            else if (PosX > Program.citySizeX - 1)
            {
                PosX = 0;
            }
            else if (PosX < 0)
            {
                PosX = Program.citySizeX - 1;
            }
            else if (PosY > Program.citySizeY - 1)
            {
                PosY = 0;
            }
            else if (PosY < 0)
            {
                PosY = Program.citySizeY - 1;
            }
        }

        public void CheckOutOfBoundsAlt()                                      //Check if person if out of bound and replace positon to other end. 
        {
            if (PosX < 0 && PosY < 0)
            {
                PosY = Program.AltSizeY - 1;
                PosX = Program.AltSizeX - 1;
            }
            else if (PosX > Program.AltSizeX - 1 && PosY > Program.AltSizeY - 1)
            {
                PosY = 0;
                PosX = 0;
            }
            else if (PosX > Program.AltSizeX - 1 && PosY < 0)
            {
                PosX = 0;
                PosY = Program.AltSizeY - 1;
            }
            else if (PosX < 0 && PosY > Program.AltSizeY - 1)
            {
                PosX = Program.AltSizeX - 1;
                PosY = 0;
            }

            else if (PosX > Program.AltSizeX - 1)
            {
                PosX = 0;
            }
            else if (PosX < 0)
            {
                PosX = Program.AltSizeX - 1;
            }
            else if (PosY > Program.AltSizeY - 1)
            {
                PosY = 0;
            }
            else if (PosY < 0)
            {
                PosY = Program.AltSizeY - 1;
            }
        }

        public virtual void Action(Person person)
        {
            Console.WriteLine("Person gör någonting");          //Base virtual method 
        }

        public Person()
        {
            rng = new Random();
            PosX = rng.Next(0, Program.citySizeX);          //Gives start position to each person (X,Y)
            PosY = rng.Next(0, Program.citySizeY);
        }
    }





}
