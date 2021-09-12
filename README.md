This is the Unity Port of the [Lua-based Court Engine for Game Grumps Joint Justice](https://github.com/IsaacLaquerre/GameGrumpsJointJustice), the Game Grumps Phoenix Wright fan project.

---

## Set-Up Requirements

To contribute to the project, you will need to have the following tools instead (order recommended):

- [Git](https://git-scm.com/downloads)
- [Git Large File Storage (Git LFS)](https://git-lfs.github.com/)
- [Unity Hub](https://unity3d.com/get-unity/download)
  - This will prompt you to create a Unity ID and select a Micrograme before it lets you launch Unity. This second step is expected to take some time.
- [Unity Version 2020.3.15f2](https://unity3d.com/get-unity/download/archive)
  - Click on the Unity 2020.x tab, scroll to Unity 2020.3.15f2 (LTS) and click on the green "Unity Hub" button to download
  
Wait for Unity to launch, then install the following:

- [GitHub for Unity](https://assetstore.unity.com/packages/tools/version-control/github-for-unity-118069)
  - Click on "Add to My Assets", then "Open in Unity".
  - Once the Package Manager opens, hit "Download", then "Import"

_Optional:_

- [GitHub for Desktop](https://desktop.github.com/) - Helpful UI if you're not familiar with Git CLI, Recommended install after Git LFS
- Some IDE ([Visual Studio Code](https://code.visualstudio.com/), [Atom](https://atom.io/), [Sublime](https://www.sublimetext.com/download) are all free examples)

## How to Run

- Via GitHub CLI or Desktop, clone this repository somewhere reasonable
- Launch Unity Hub - this should open up the Project tab by default
- (First Time Only) Click "Add"
  - Select your copy of "GG-JointJustice-Unity" and hit "Open"
  - Once it appears in the Projects list, makes sure it's pulling in the correct Unity Version
- Select GG-JointJustice-Unity to open the project
- In the "Project" tab, navigate to `Assets/Scenes/SampleScene`
- Click on the "Game" tab in the main view
- Hit the play button on the top!

## Contributing
When updating code, it is mandatory to create a pull request to make sure
- the game still builds properly on all platforms **on a clean setup**
- tests are run successfully **on a clean setup**
- code adheres to [this project's style guide](https://docs.google.com/document/d/1zN4Yx62PpyXhu1g_AhtTZSC8k9e4zqWbzQPt1Hlw9Ag/edit)
- changed code is reviewed by at least two other developers

It is also recommended to run all [PlayMode and EditMode tests](https://docs.unity3d.com/Packages/com.unity.test-framework@1.0/manual/edit-mode-vs-play-mode-tests.html) locally **before creating a pull request**.  

## Running tests locally
Running tests ensures no existing functionallity is broken by a change or - if the behavior change is intended - all existing test suits are updated accordingly.

Alternatively these steps can also be used to better analyse a failed test case. While our automated deployment workflow generates callstacks of offending test cases, it sometimes may be easier to attach a debugger to the running process.

To run tests locally:
1. Open the project in Unity
2. Select `Window` -> `General` -> `Test Runner` from the top menu
3. Inside the `Test Runner`-tab/-window...
   1. Select `PlayMode`
   2. Click on `Run All` and ensure every test is prepended with a green checkmark (✅)
   3. Select `EditMode`
   4. Click on `Run All` and ensure every test is prepended with a green checkmark (✅)
4. If any tests were unable to complete successfully...
   1. Click on the row containing the red cross (❌)
   2. Inspect the log at the bottom of the `Test Runner` window and understand which assertion can no longer be made. Either...
      1. Change the game code to match the assertion **or**
      2. **(if the test case is outdated or invalid due to the change)** update the test suite to reflect the new intended behavior