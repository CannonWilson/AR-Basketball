# AR-Basketball

### Description

As my first foray into AR development, this small project taught me a great deal. I learned how to set up a Unity project using
the experimental Oculus passthrough API (cloning this project will not work well because the project settings are gitignored). 
I also created physics materials to give balls a realistic bounce. The most important thing I learned from this project was how 
to create OVR rigs (for the camera) that work with XR rigs and the XR Interaction Manager (for user input) in the same scene.
You can see a full demo on YouTube [here](https://youtu.be/5oA4htdD4F8). The video is taken from SideQuest, a third-party
application that can capture footage from games using the passthrough API. 

***

### Features

![Spawn the Goal](https://j.gifs.com/Y7lKjW.gif)

The player can spawn a basketball goal by pressing the left grip button. Holding down the same button allows the player to move
and rotate the goal around.

![Dribble and Dunk](https://j.gifs.com/lRM3J1.gif)

By pressing the trigger button on either controller, the player can spawn a ball. Releasing the trigger throws/drops the ball,
which can bounce off of the ground or the goal. You can also dunk, which is shown above. Putting the ball through the hoop
increases the score shown on the backboard.
