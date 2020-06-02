This triple gameplay tool is designed to assist either programmers or artists for performing their work in any kind of environment. This asset pack contains a player controller with a good variety of controls,
a Snapshot manager tool which captures in game content while playtesting for any purpose( examine content after playtesting or save it as a wallpaper and a Third Person (TP) camera which contains a variety of options allowing both
programmers and artists to adjust their work in their designated scene . These 3 assets come as a bundle ,but each one of them can work independently.
	(Important note: if your player object is not a humanoid rig you should still tag it as "Player" for TP Dynamic Camera to properly work.)

This triple bundle of gameplay tools has been mostly tested on 5.6.5f1 version of Unity. Earlier versions had been initially developed in Unity 5.0.0f4. The asset has also been tested and works through 2017.2 version.
It is strongly recommended to use it from version 5.6.5f1 and later.


Instructions: 
	How to import the package:
		Step 1: While in Unity Editor go to Assets/Import Package/Custom Package...
		Step 2: Find the Asset Package which contains the files associated and import it.
		
	Setup: 
		Test the Assets:
		Step 1: The following package contains an already prepared demo scene to test the asset's functionality and features.Navigate through the scene folder within the asset folder "GameplayAssetTool" and load "SampleScene".
			(Important Note: Before loading demo scene or any other scene which includes these assets please check the project directory after importing the package for a ".txt" file with the name "lastDirectorySaveTarget".
			 If that file exists please remove any text ,save the file and exit afterwards.Do not attempt playtest with the snapshotViewer content active if this isn't resolved properly.
			 If the file doesn't exist you don't need to take further action ,it will automatically be created on your first playtest.)  
			
		Step 2: Navigate through Prefabs folder within the "GameplayAssetTools" folder and read the ".txt" file for each asset to help you understand how to properly use it and all of their features.
			(Note 1: Each of these assets is independent to each other.)
			(Note 2: If you wish to use "snapshotViewer" as a standalone asset you need to have an active camera to your desired scene.)

		Step 3: The folder "PlayerPrefabs" contains a player controller with a humanoid rig and the animator with the name "PlayerAC" . In order for this to be functional you need to import
			Unity's animations through Standard Assets and assign each of the following animations listed below to the proper state field inside the animator tab in your editor:
		
		Step 4 : The list below contains all the animations needed from Standard Assets for humanoid motions which are supported from the player controller asset.
			(Note:If you wish to use you own animations or any other third-party animations feel free to do so.Check each of the states contained within the animator tab while having your active rig on the scene and assign the proper clips to each of the states on the Animation Controller.)
		State Name | Animation Name
		  "Idle"   |  "HumanoidIdle"		(Layer: BaseLayer)
		"LeftIdle" | "StandQuarterTurnLeft"	(Layer: BaseLayer)
                "RightIdle"| "StandQuarterTurnRight"	(Layer: BaseLayer)
                "JumpIdle" | "HumanoidJumpUp"		(Layer: BaseLayer)
                 "Walk"    | "HumanoidWalk"		(Layer: BaseLayer)
 		"WalkLeft" | "HumanoidWalkLeft"		(Layer: BaseLayer)
 		"WalkRight"| "HumanoidWalkRight"	(Layer: BaseLayer)
 		"JumpWalk" | "HumanoidMidAir"		(Layer: BaseLayer)
		   "Run"   | "HumanoidRun"		(Layer: BaseLayer)
		"RunLeft"  | "HumanoidRunLeft"		(Layer: BaseLayer)
		"RunRight" | "HumanoidRunRight"		(Layer: BaseLayer)
		"JumpRun"  | "HumanoidJumpForwardLeft"	(Layer: BaseLayer)

              "IdleCrouch" | "HumanoidCrouchIdle"  	(Layer:Crouch)
      "IdleCrouchTurnLeft" | "HumanoidCrouchTurnLeft"   (Layer:Crouch)
      "IdleCrouchTurnRight"| "HumanoidCrouchTurnRight"  (Layer:Crouch)
 "CrouchWalk"(BlendingTree)| Assign from top to bottom with the following sequence the clips: "HumanoidCrouchWalkLeft","HumanoidCrouchWalk","HumanoidCrouchWalkLeft"(Check if Mirror Animation is enabled)(Layer:Crouch)

Important Note: Read documentation texts in each of the specified folders containing the assets for further understanding and proper use(Note: Main Assets are located within the package's folder "Prefabs").