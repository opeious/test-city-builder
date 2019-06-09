# Sample City Builder
![](https://raw.githubusercontent.com/opeious/test-city-builder/master/SampleImages/6.png)

## Features:
### Regular Mode

![](https://raw.githubusercontent.com/opeious/test-city-builder/master/SampleImages/1.png) ![](https://raw.githubusercontent.com/opeious/test-city-builder/master/SampleImages/2.png) ![](https://raw.githubusercontent.com/opeious/test-city-builder/master/SampleImages/5.png)

Player can select a building by clicking on it and see a building name on top of it and current production progress. (or can start a new production if no production is running)

### Build Mode

![](https://raw.githubusercontent.com/opeious/test-city-builder/master/SampleImages/8.png)
![](https://raw.githubusercontent.com/opeious/test-city-builder/master/SampleImages/3.png)

The Player can buy a new building from the left hand side. The player can only buy a building for which enough resources are present. Player will not be able to select a building which he doesn't have enough resources for, the building card will also be red in color to indicate the same. Resources can be tracked from the panel on the top. After a new building is placed, it starts its construction. Construction progress can be tracked by the progress bar on the top of the building.

If the building is a resource production building, production can only begin once construction is complete.

![](https://raw.githubusercontent.com/opeious/test-city-builder/master/SampleImages/4.png)

After construction is complete the player can also click on the building to pick up and move it.

![](https://raw.githubusercontent.com/opeious/test-city-builder/master/SampleImages/7.png)

The mode can be switched using the buttons in the bottom panel


## Tech Design Choices:

![](https://raw.githubusercontent.com/opeious/test-city-builder/master/SampleImages/11.png)

I chose a Singleton and Events based architecture because it is easy to understand, makes the code modular. The singleton GameObject is already present in the scene (FinalScene) and pre configured. The singletons can be interfaced such that their variables can be saved and loaded to allow for persistence across scenes and sessions.

### DataManager
![](https://raw.githubusercontent.com/opeious/test-city-builder/master/SampleImages/9.png)
Initializes and parses the data from scriptable objects. Other managers and views can use the data manager to get the buildings data.

![](https://raw.githubusercontent.com/opeious/test-city-builder/master/SampleImages/10.png)
All the buildings data is passed to the data manager as editor reference.

![](https://raw.githubusercontent.com/opeious/test-city-builder/master/SampleImages/11.png)
I chose ScriptableObjects as data containers because after creation editing the data to change some values becomes a trivial task.

If given more time, I would have preferred to have an editor script parse the data from a JSON or xml into a custom data class or scriptable object, for scalability.

### GameModeManager
Stores the current mode of the game. When they mode is changed from the UI, it changes a manager variable. Triggering a OnGameModeChanged event. Other UIs and Managers can use this event to track game mode changes.

### GameboardManager
Keeps track of the grid and does grid operations. Input manager forwards grid touches to the grid.

### InputManager
Just a simple class to detect mouse left clicks. Should be replaced with a Gesture/Touch extension to allow cross-platform play.

### ResourceManager
Keeps track of the player inventory and all actions to player inventory are performed through resource manager functions. (Add, Remove, etc). Also triggers events when resources are added or removed for UIs and other managers.

### TooltipsManager
Deals with player clicks and popups on building clicks. Handles the creation and deletion of tooltip UI elements. 

### EntityManager
An Entity is anything that can be placed on the gameboard. A building an extension to an entity adding additional properties and functionality to the same.

The entity manager keeps track of all the entities o n the gameboard. The entity placement controller in the manager allows the player to pickup and move around existing entities and place new entities.

It also checks if the position the player is trying to move a building to or trying to place a new building at is not out of bounds or currently occupied by another entity. (It is better to check only the existing entities on the scenes instead of every tile on the grid)

If I had more time, I would re-factor this manager to make the code more self-explainable and cleaner.

### TimeManager
Keeps track of time, so that other game elements and managers don't have to keep track of time in their Update loops. Can be modified to change the time scale to make the game more responsive as a trade-off for performance.