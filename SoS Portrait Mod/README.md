# SoS:PoOT Portrait Mod

This mod adds portraits back into the game. 

<img src="https://github.com/Otakubuns/SoSPoOT-Mods/assets/77337386/873c7548-b741-4d88-9f34-954e1654abd4" width="300"> <img src="https://github.com/Otakubuns/SoSPoOT-Mods/assets/77337386/89b96a5b-2312-4583-874a-ef9788465a6e" width="300"><img src= "https://github.com/Otakubuns/SoSPoOT-Mods/assets/77337386/bc4f129f-395f-43c0-bfbe-11e05a59ecb6" width="300">





The sprite is loaded whenever a conversation is started so technically you can change the sprite in game if you replace the photo and initiate the conversation again.

## Adding Custom Sprites
Having custom sprites isn't my priority so the support for other images is not high on my list of things to work on although you *can* have custom sprites

If you want to add custom sprites you need to do the following:
- For best scaling and look 350x320 is optimal
- Just use the characters name as the sprite, if you want to replace Linh's just add an image to the assets folder(or in another within it) named "Linh".
- (optional) If you want to add custom reaction sprites(only for gifts atm) you need to have a photo with the reaction suffix. The current three reactions implemented:<br>
      ‣*Troubled (ex. "Linh_troubled.png")*<br>
      ‣*Happy<br>*
      ‣*Blushing*

## Issues
- Some sprites aren't perfect but most characters have no issues(dlc characters arent done yet).
- There are plans to make sure all sprites fit perfectly its just not done yet.
- ~~No idea if these work in cutscenes/events~~ **Added in 1.0.1 for cutscenes(I assume the code for heart events is the same)**
- Photos not the size of 350X320 will look stretched(working on fixing that)

## Future Plans
- ~~(DLC Characters) Add happy/love sprite for liked/loved items~~ **1.0.2 has DLC sprites for gifts(though they don't have alot of gift preferences/levels in this game) and support for optional sprites for anyone else**
- (Possibly) Try to make the sprites preloaded(had issues with this due to il2cpp compiling)

## Acknowledgements
DLC sprites were grabbed from https://ranchstory.miraheze.org and upscaled using [waifu2x](https://waifu2x.udp.jp/)
