Made in a record time of 2 full days <3

# desktop-time-limiter
Monitors the time spent on a PC and prevents using it after a certain time limit. Perfect for parents to set a time limit on children's PC.
Resets at a specified time. The UI is in Czech.
Variables that can be changed:
- The hour the timer resets at
- The time that passed
- Password
- Time limit
- Inactivity threshold (after how much time the user is considered inactive)
- State (pause/resume/close)

## Behavior
- When you open the program for the first time, it copies itself to AppData and sets itself to run on PC login.
- The program itself can be accessed from tray. When the program window is opened, it's always on-top and cannot be moved outside the screen.
- When a certain time level is reached (30 minutes left, 10 minutes left...), the program makes a notification sound and says out loud the remaining time.
- When the maximal time set is reached, the user is logged out (but NO work is lost or closed) and the PC is "locked" - a dark transparent overlay is put over the whole screen, and the program is brought to front and cannot be closed. The user can log back in and won't be logged out again, but the dark overlay and program on top stays - it's always on top of other programs and no program can override it.
- When the time to reset the limiter comes, the PC is "unlocked" - passed time is reset, the overlay is hidden and the program UI can be closed again.
- The program updates itself automatically (according to releases here on github).

## Security
The program deploys some light security against uninstalling or closing, but just very shallowly, because this is not the point of this program.
- Invisible from Task Manager (can still be seen in "details" tab)
- Can be closed only by Windows Shutdown event (and by closing it from the UI)
- Copies itself to an arbitrary location
- Runs on startup

If security was a concern, it would
- be invisible from the list of run-on-startup apps in Task Manager (via re-applying Windows' RunOnce registry value)
- deploy a second process that would monitor the program's run state and restart it if it was closed (either by getting the Process' Window handle or by polling current processes)
- encrypt the config file where it stores variable values (by using a generic encryption/decryption algorithm on the text on writing/reading and obfuscating
the encryption key and not loading it into memory to prevent memory dump),
- vary it's location every time it launches
- possibly get admin rights and move itself to Windows/system32 so that the user would be unable to close the process
- exclude itself from Antivirus (via PowerShell's Set-MpPreference -ExclusionPath)
- be renamed to something sneaky (like a generic windows process name)
- have obfuscated code (to confuse IL instructions viewer and decompilers like DotPeek)

## How to install
VirusTotal comes out clean (including ESET, McAfee, Kaspersky, Avast, MalwareBytes...), but Windows Defender's Machine Learning flags it - sometimes immediately, sometimes on the next PC login.
If you're using Windows Defender, exclude the process "Wellbeing.exe" and location %appdata%\Roaming\Wellbeing

[VirusTotal for version 2.0.3](https://www.virustotal.com/gui/file/9937ffab99f7c0f7bd33a7aabdeb05d6ec356316db954631d3320d6210f68e84/detection)

![image](https://user-images.githubusercontent.com/57546404/176916296-d06788bd-c4b5-4c6d-93d8-b9e9d37cf805.png)


Otherwise just download and run and it will take care of itself.

## Screenshots
UI

![image](https://user-images.githubusercontent.com/57546404/170340720-261647eb-02dd-4334-b7ae-168201d472f5.png)

Prompts

![image](https://user-images.githubusercontent.com/57546404/170340868-ae9b2208-bebd-448e-84d8-c1213e1765ad.png)
![image](https://user-images.githubusercontent.com/57546404/170340987-ba464f21-6829-4b02-b2c9-7894312533b1.png)


## Relatable meme for a good day

![Project-completeness-over-time](https://user-images.githubusercontent.com/57546404/170346611-9c4b34df-4033-4025-afdb-0cd6f81c3317.png)
