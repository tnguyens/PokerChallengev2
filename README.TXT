# PokerChallengev2
# Author: Toan Nguyen
# Date: 12/18/2015 - 01/06/2016

Project Description
Objective of the project is to create user interactive web-based interface where user is able to choose cards and the application will determine who wins the game.

Source Code 
./PokerApp/Content/style/page_layout.css : holds style code for the page layout. how buttons and texts look.
./PokerApp/Content/Controller/PokerController.cs : hold source code of server, the brain of the whole application.
./PokerApp/Content/Models/Card.cs : holds the model of individual card.
./PokerApp/Content/Models/Player.cs : holds model of individual player.
./PokerApp/Content/Models/PokerHandFromController.cs : hold messages from the controler for the view to use.
./PokerApp/Content/Models/PokerHandFromView.cs : hold messages from the view for controller to extract.
./PokerApp/Views/Poker/Index.cshtml : hold the page representation.

To Run
- Open the whole packet in Visual Studio 2015
- Run the solution in web browser of choice. if the index page pop up you are good to go. Otherwise, at the address bar, type /Poker. That would take you to the index page.

To Use
- At the index page, pick cards from option boxes. Click Evaulate, the application will give who the winner, if everything is in order. Otherwise, error message will display.
- Random Hand button will give both player randommed cards, it is not guaranteed to give you valid hands everytime.  