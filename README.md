# fishing-game

## Project Structure

### Art
Contains all visual assets for the game.

### Audio
Contains all audio files for the game.

### Prefabs
Contains all reusable game objects, including:  
- **General Fish Prefab**: generic fish model.  
- **General Monster Prefab**: generic monster model.  
- **Character**: player's character.  
- **Character Items**: flashlight and fishing rod.  
- **UI Elements**: reusable user interface components (as the timerUI for example).  

### ScriptableObjects
Contains all ScriptableObjects used to configure gameplay:  
- **FishTypes**: all types of fish.  
- **MonsterTypes**: all types of monsters.  
- **Recipes**: all recipes.  

### Script
Organized by gameplay domain:  
- **FishingGameplay**: scripts related to fishing gameplay.  
- **MonsterGameplay**: scripts related to monster gameplay.  
- **GlobalManagers**: scripts managing overall game systems and state.  

### Scenes
Contains all game scenes:  
- **MapSelection**: scene to select maps.  
- **FishingView**: fishing view.
- **MonsterView**: first-person monster combat scene.  
- **Events**: scene handling special events and encounters.
