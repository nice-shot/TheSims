Submitted by:
Yaron Pneuli 203540679
Nadav Sheffer 203653894

Game description:
Zombie apocalypse survival game. You are one of a group of survivers. You must
survive as long as possible. Protecting your friends will help you with that.

How to play:
Move your character (red) using the arrow keys and aim and shoot with the
mouse. You can reload at the shelter.
If the shelter has the shield on the zombies won't attack you while inside.

Agents:

Zombie
------
His goal is just to move and he can do that by either wandering around or
eating brains. Eating brains has higher priority but is not always possible.

Scavenger
---------
His goal is the keep the shield in the shelter up. He scavenges for fuel in the
gas station and brings it to the shelter.

Defender
--------
His goal is to shoot zombies. He approaches a zombie from a safe distance and
shoots. He also reloads at the shelter.

Notes:
We've added a few changes to the Goap scripts:
- We added the possiblity to abort a mission during movement for cases where
  zombies try to approach people in the shelter.
- We also added a custom move for the Defender agent so he'll be able to shoot
  zombies from a distance.

Ideas we didn't have time to implement:
- Having limited fuel stock at the gas station and putting more stations
  further away.
- Giving the scavenger the option to stay inside the shelter if there's enough
  fuel and too many zombies outside.
- Having zombies interact better with the shelter and shield:
  - They shouldn't immediatly stop moving when their target is safe
  - They shoudln't be able to move through the shield (we need to play with
    colliders a bit...)
- Have limited ammunition in the shelter which the scavenger needs to bring as
  well.
- Have a gate mechanic in the shelter so instead of just moving to the shielded
  shelter, characters needed to open a gate which will take time and may cause
  zombies to barge in.
- Make a bigger and more complex map with multiple shelters and perhapse a
  possible escape.
- Zombie doesn't instantly kill but hurts and grabs the victim. The player and
  Defender agent might be able to save someone during that time.
- Add food as a resource that characters need to eat to survive.
- If we had a bigger map, perhapse the player will meet additional characters
  on the way and could join their group or ask them to join his.
- Additional character ideas:
  - Leader - gives the different people assignments and is in charge of
    rationing ammo, fuel and food. Stays in the shelter.
  - Doctor - Heals wonded people. Might also act as a scavenger that will look
    for medicine.
