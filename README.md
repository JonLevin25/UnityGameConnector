# Unity Game Connector
PoC framework for game jams - joins separate (one-scene) games and lets them pass payloads to one another.

Tried to keep the GameConnection infra relatively tidy, but since this is a PoC some things may be a bit messy/hacky

**Recommended Unity - 2019.3**

## Demo Games
To Load the demo:
* Open Tool Window (MenuItem GameConnection/Config Tool)
* Click "Load Start Scene"
* Enter Play mode.

## Project structure
Unity project containing 3 parts:
* GameConnection (infra for finding/connecting games from the project)
* 2 Demo games.

The demo involves 2 "games", the first gets the name of the player and a rival (i.e pokemon),
and the second has a safe with a code. The code is the name of the rival entered in game 1.

## Configuring Connected Games
* Open Tool Window (MenuItem GameConnection/Config Tool)
* (Re)Generate Game Manifest if you neetd to (this is the asset that holds Game Scene info)
* Reorder games if you like
* Hit "Set Build Indices" to configure scenes

## Creating a new game
TODO