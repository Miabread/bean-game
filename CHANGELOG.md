## 21w44c

- Added first map (no lighting or textures yet though)
- Added crude FPS counter to debug display
- Added is-host flag to debug display
- Added a length limit to player names (16 characters)
- Fixed input persisting during pause if held (e.g. hold jump; pause; would still jump)

## 21w44b

- Added event log display
- Added player joined/leave event logs
- Added player tagged player event log
- Remove click to resume, Esc now both pauses and resumes
- Fixed tag event triggering whenever, now only triggers once and if receiver isn't already tagged
- Fixed button controls (e.g. change color) ignoring pause state
- Fixed vector controls (e.g. look, walk) not being set to zero when paused, causing them to continue moving if you pause while moving
