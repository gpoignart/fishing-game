# fishing-game

## Project Structure

### Art
Contains all visual assets for the game.

### Audio
Contains all audio files for the game.

### Prefabs
Contains generic fish model.

### ScriptableObjects
Contains ScriptableObjects used to configure gameplay.

### Script
Organized by gameplay domain:  
- **MenuGameplay**: scripts related to menu.
- **FishingGameplay**: scripts related to fishing view gameplay.  
- **MonsterGameplay**: scripts related to monster view gameplay.  
- **MapSelectionGameplay**: scripts related to map selection.
- **InventoryGameplay**: scripts related to inventory view.
- **TransitionGameplay**: scripts related to transitions.
- **WorldContent**: scripts to initialize SOs objects.
- **SaveSystem**: scripts to save the datas and load them.
- GameManager: the persistent game manager.

### Scenes
Contains all game scenes:
- **Main**: contains the persistent objects (game manager and inventory manager), must be the one to be launched.
- **MapSelection**: scene to select maps.  
- **FishingView**: fishing view.
- **MonsterView**: first-person monster combat scene.
- **InventoryView**: the inventory view where use recipe and view inventory.
- **TransitionView**: the view to display transitions between certain states.
- **Events/**: scenes handling special scenario events.
