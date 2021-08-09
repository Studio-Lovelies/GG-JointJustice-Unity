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
  
- Wait for Unity to launch, then install the following:

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
- (First Time Only) If you see the "Free Aspect", click on it
  - Add a custom resolution via the + button, with the type "Fixed Resolution" and Width/Height of 1280 x 768. Make sure this is selected after creation.
- Hit the play button on the top!
