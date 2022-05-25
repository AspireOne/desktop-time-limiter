# desktop-time-limiter
Monitors the time spent on a PC and prevents using it after a certain time limit. Resets at a specified time. Variables that can be changed:
- the hour the timer resets at
- Passed time
- Password
- Maximal time (time limit)
- Inactivity threshold (after how much time the user is considered inactive)
- State (stop/pause/close)

## Behavior
- When you open the program for the first time, it copies itself to AppData and sets itself to run on PC login.
- The program itself can be accessed from tray. When the program window is opened, it's always on-top and cannot be moved outside the screen.
- When a certain time level is reached (30 minutes left, 10 minutes left...), the program makes a notification sound and says out loud the remaining time.
- When the maximal time set is reached, the user is logged out (but NO work is lost or closed), a dark transparent overlay is put over the whole screen,
and the program is brought to front and cannot be closed. The user can log back in and won't be logged out again, but the dark overlay and program on top stays -
it's always on top of other programs and no program can override it.
- When the time to reset the limiter comes, the passed time is reset, the overlay is hidden and the program UI can be closed again.

## Security
The program deploys some light security against uninstalling or closing, but just very shallowly, because this is not the point of this program.
- Invisible from Task Manager (can still be seen in "details" tab)
- Can be closed only by Windows Shutdown event (and by closing it from the UI)
- Copies itself to an arbitrary location
- Runs on startup

## How to install
VirusTotal comes out clean, but Windows Defender's Machine Learning flags it as "Trojan:Script/Wacatac.B!ml" - sometimes immediately, sometimes on the next PC login.
If you're using Windows Defender, I'd recommend excluding the program from detection or allowing it when it prompts.

Otherwise just download and run and it will take care of itself.

## Screenshots
UI

![image](https://user-images.githubusercontent.com/57546404/170340720-261647eb-02dd-4334-b7ae-168201d472f5.png)

Prompts

![image](https://user-images.githubusercontent.com/57546404/170340868-ae9b2208-bebd-448e-84d8-c1213e1765ad.png)
![image](https://user-images.githubusercontent.com/57546404/170340987-ba464f21-6829-4b02-b2c9-7894312533b1.png)

