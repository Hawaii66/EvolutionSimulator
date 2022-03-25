# EvolutionSimulator
Evolution Simulation in custom enviorments.
- The brain of the mouse is a state machine with predefined states.
- All the food on the map is batched together into one big mesh for performance.
- Currently working on a UI graph to track the average stats for a overview of how the mice are evolving.

# How to use:
- Select the object in the scene named Terrain Generation
- In Terrain/ScriptableObjects are some presets of different terrains the evolution can take place in.
- Drag one of those into the field named "Scriptable Colors" in Terrain Generation. Press play and terrain is generated based on the scriptable object.

#Custom Terrain
- If you want to make custom terrain, duplicate one of the presets and edit the variables. The noiseheight param goes from 0 - 1 but the last item should always have a height of 2 as Mathf.PerlinNoise(x,y) can return values a bit larger than 1 but never larger than 2.
- Color determines the color on the map that tile will be drawn in.
- Final Height: Scale factor of the noise so for example water can be lower in the world and mountains a bit higher. Rembember that there is no interpelation between different "biomes".
- Is Walkable: Tells the AI if the mouse can walk on this biome.
- The rest of the booleans tells the AI what sort of a tile it is and will in the future determine things like coldness and speed.
