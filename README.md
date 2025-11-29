# fishing-game

## Project Structure

### Art
Contains all visual assets for the game.

### Audio
Contains all audio files for the game.

### Prefabs
Contains generic fish model.

### ScriptableObjects
Contains ScriptableObjects used to configure gameplay:  
- **FishTypes**: all types of fish.  

### Script
Organized by gameplay domain:  
- **FishingGameplay**: scripts related to fishing view gameplay.  
- **MonsterGameplay**: scripts related to monster view gameplay.  
- **MapSelection**: scripts related to map selection.
- **InventoryView**: scripts related to inventory view.
- GameManager: the persistent game manager.
- World content: enumerations of world content items (maps, ingredients, equipments...)
- InventoryManager: the persistent inventory.

### Scenes
Contains all game scenes:
- **Main**: contains the persistent objects (game manager and inventory manager), must be the one to be launched.
- **MapSelection**: scene to select maps.  
- **FishingView**: fishing view.
- **MonsterView**: first-person monster combat scene.
- **InventoryView**: the inventory view where use recipe and view inventory.
- **Events/**: scenes handling special scenario events.
