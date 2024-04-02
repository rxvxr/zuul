using System;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;

	Item medkit = new Item(15, "Medkit");

	// Constructor
	public Game()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		Room outside = new Room("outside the main entrance of the university");
		Room theatre = new Room("in a lecture theatre");
		Room pub = new Room("in the campus pub");
		Room lab = new Room("in a computing lab");
		Room office = new Room("in the computing admin office");

		// 
		Room hallway = new Room("in the hallway");
		Room up = new Room("on the second floor of the university");

		// Initialise room exits
		outside.AddExit("east", theatre);
		outside.AddExit("south", lab);
		outside.AddExit("west", pub);

		theatre.AddExit("west", outside);

		// 
		theatre.AddExit("hallway", hallway);

		hallway.AddExit("stairs", up);
		hallway.AddExit("theatre", theatre);

		up.AddExit("stairs", hallway);

		theatre.Chest.Put("medkit", medkit);
		// 

		pub.AddExit("east", outside);

		lab.AddExit("north", outside);
		lab.AddExit("east", office);

		office.AddExit("west", lab);

		// Create your Items here
		// ...
		// And add them to the Rooms
		// ...

		// Start game outside
		player.CurrentRoom = outside;
	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished)
		{
			Command command = parser.GetCommand();
			finished = ProcessCommand(command);
		}
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
		Console.WriteLine("Type 'help' if you need help.");
		Console.WriteLine();
		Console.WriteLine(player.CurrentRoom.GetLongDescription(player));
	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if (!player.IsAlive() && command.CommandWord != "quit")
        {
            Console.WriteLine("You bled out, you died...");
            Console.WriteLine("You can only use the command:");
            Console.WriteLine("quit");
            return wantToQuit;
        }

		if(command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "look":
				Look();
				break;
			case "status":
				Status();
				break;
			case "take":
				Take(command);
				break;
			case "drop":
				Drop(command);
				break;
			case "use":
                Use(command);
                break;
		}

		return wantToQuit;
	}

	// ######################################
	// implementations of user commands:
	// ######################################
	
	// Print out some help information.
	// Here we print the mission and a list of the command words.
	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around at the university.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}
	
	private void Look()
	{
		Console.WriteLine(player.CurrentRoom.GetLongDescription(player));

		Dictionary<string, Item> roomItems = player.CurrentRoom.Chest.RetrieveItems();
		if (roomItems.Count > 0)
		{
			Console.WriteLine("Items in this room:");
			foreach (var itemEntry in roomItems)
			{
				Console.WriteLine($"{itemEntry.Value.Description} - ({itemEntry.Value.Weight} kg)");
			}
		}
	}

	    private void Status()
    {
        Console.WriteLine($"Your health is: {player.GetHealth()}");

        Dictionary<string, Item> items = player.RetrieveItems();

        if (items.Count > 0)
        {
            Console.WriteLine("Your current items:");

            // Iterate over elk item in player zijn inv
            foreach (var itemEntry in items)
            {
                Console.WriteLine($"- {itemEntry.Key}: Weight {itemEntry.Value.Weight}");
            }
        }
        else
        {
            Console.WriteLine("You have no items in your inventory.");
        }
    }

	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void GoRoom(Command command)
	{
		if(!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to "+direction+"!");
			return;
		}


		player.CurrentRoom = nextRoom;
		player.Damage(25);
		Console.WriteLine(player.CurrentRoom.GetLongDescription(player));

		if (!player.IsAlive()) 
        {
            Console.WriteLine("You bled out and now have died.");
        }

	}

		private void Take(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Take what?");
			return;
		}

		string itemName = command.SecondWord.ToLower();

		bool success = player.TakeFromChest(itemName);

	}

	private void Drop(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Drop what?");
			return;
		}

		string itemName = command.SecondWord.ToLower();

		bool success = player.DropToChest(itemName);


	}

	    private void Use(Command command)
    {
        if (!command.HasSecondWord())
        {
            Console.WriteLine("Use what?");
            return;
        }

        string itemName = command.SecondWord.ToLower();

        player.Use(itemName);
    }
}
