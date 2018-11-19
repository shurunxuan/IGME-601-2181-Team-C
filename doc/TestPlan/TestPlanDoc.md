# Test Plan Document
This document will propose and record the results of tests used to ensure that acceptance criteria are met. All stories require testing of some sort or another.

## Example
*Camera Tool*: 
 * **PASSED**. MD 11/2 
 
Notes: If the test failed give notes.

Procedure:
1. Open Assets/Scenes/TestScenes/CameraTestScene.Unity
2. Use the key: 1 to activate the camera.
3. Use the key: 1 a second time to record visual info that is in front of the aiming reticle.
4. Repeat until you have 100% info denoted at the top left corner of the game window. Note that there are three unique instances of visual data as denoted in this test by color. Collecting the same information twice will not increase the amount of info gathered.


## Stories

*Refined Camera*:  
 * **PASSED** MD 11/7.  
 * **PASSED** JJ 11/7.  

Notes: 

Procedure:
1. Open Assets/Scenes/DemoScenes/Sprint3DemoScene.Unity
2. Use mouse or right stick to rotate the camera.
3. Use the key 1 or A button to switch to first person camera.
4. In third person camera mode, try use some strange angle to test the camera collider.

------------------------

*Drone 3D Asset*: 
 * **UNTESTED**.  

Notes:  

Procedure:

------------------------

*Prototype Level*:  
 * **UNTESTED**.  

Notes:  

Procedure:

------------------------

*Environment Assets*:  
 * **UNTESTED**.  

Notes:  

Procedure:

------------------------

*Guards 3D Asset*:  
 * **UNTESTED**.  

Notes:  

Procedure:

------------------------

*Guard States*:  
 * **UNTESTED**.  

Notes:  

Procedure:

------------------------


*Enemy Sound Detection*:  
 * **PASSED** MD 11/12.  
 * **PASSED** VS 11/12.  
 
Notes:  

Procedure:

1. When enemy can't hear or listen anything then it should be in PATROL state.
2. When it detects something, you should be able to see that in DEBUG console. 
3.  When detected it should wait for few moments before it actually moves to the destination. This is the time we give our player where he can evade safely. 
4.  Regarding Audio range: It should not be able to hear anything which is outside the trigger zone and if a object is in trigger zone but the distance between the enemy and audio source is greater than the max we have set up. (Using Decoy object for demoing this).
5.  When our enemy moves to INVESTIGATE state and moves to the target's position but can't find anything there, then it should go back to patrol in few seconds with alert set to the 0f. Again, you can use DEBUG console view relevant statements.
------------------------

*Hacking*:  
 * **PASSED** VS 11/5.
 * **PASSED** SJ 11/5.
 
Notes:  

Procedure:
1. Open Assets/Scenes/TestScenes/HackingTestScene.Unity
2. Use the key: 2 to connect to both charge points one at a time (Press 2 again to release)
3. Observe as you ease into position that you stop losing energy
4. Observe after easing in that energy has begun to charge
5. Observe that one charge point grants info
6. Observe that the other charge point raises and lowers a yellow info block
7. Open Assets/Scenes/DemoScenes/Sprint3DemoScene.Unity
8. Proceed through the level to the charge point below the computer
9. Connect to the charge point
10. Observe as you ease into position that you stop losing energy
11. Observe after easing in that energy has begun to charge
12. Observe that there are no relevant exceptions in the inspector

------------------------

*Decoy Device*:  
 * **PASSED** VS 11/12. 
 * **PASSED** SJ 11/12. 
 
Notes:  

Procedure:
1. Open Assets/Scenes/DemoScenes/Sprint3DemoScene.Unity
2. Use the key: 5 (or r1) to go cycle through equipped tools until their is a red trajectory in front of you
3. Use the key: 3 to launch a decoy pellet
4. Observe that the pellet dissapears after time has passed
5. Observe that pressing 1, 2, or 4 will unequip the decoy tool
6. Observe that pressing 3 or 5 while no non-core tool is equipped will equip the last one equipped
7. Navigate to where the guard is
8. Use a Decoy in front of the guard to distract it
9. Navigate to the charge point
10. Use the key: 2 to connect to the charge point
11. Observe that attempting to move upward off of the charge point (with spacebar) dissengages you from it
12. Use the key: 2 to connect to the charge point
13. Observe that pressing 1, 3, or 4 will dissengage from the charge point

------------------------

*Camouflage*:  
 * **PASSED** MD 11/18.
 
Notes:  

Procedure:
1. Open Assets/Scenes/DemoScenes/PlayTestDemoScene.unity
2. Navigate to the first guard
	Go through the vent
	Use the computer to hack open the door
	Exit into hallway
	Turn left
	Go through the first door on the right
3. Activate Camo tool
4. Observe the cool shader effects on the drone
5. Observe an initial energy cost and an increase in the energy loss over time effect
6. Fly in front of the guard
7. Observe lack of response
8. Deactivate the Camo tool
9. Observe that deactivating the tool does not cost energy
10. Observe immediate response from guard
11. Reactivate the Camo tool
12. Observe that as soon as the energy runs out, the Camo tool will be deactivated.

------------------------

*Player Can Die*:  
 * **PASSED** VS 11/18. 
 
Notes:  

Procedure:
1. Open Assets/Scenes/DemoScenes/PlayTestDemoScene.unity
2. Waste all of your energy using tools
3. Observe as the death message appears and the drone falls to the ground
4. Press backspace to restart the level

------------------------

*Heads Up Display*:  
 * **UNTESTED**.  
 
Notes:  

Procedure:


