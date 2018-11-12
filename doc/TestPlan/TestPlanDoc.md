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
 * **UNTESTED**.  

Notes:  

Procedure:

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
 * **UNTESTED**.
 
Notes:  

Procedure:

------------------------

*Player Can Die*:  
 * **UNTESTED**.  
 
Notes:  

Procedure:

------------------------

*Heads Up Display*:  
 * **UNTESTED**.  
 
Notes:  

Procedure:


