## 21w44b

- Added event log display
- Added player joined/leave event logs
- Added player tagged player event log
- Remove click to resume, Esc now both pauses and resumes
- Fixed tag event triggering whenever, now only triggers once and if receiver isn't already tagged
- Fixed button controls (e.g. change color) ignoring pause state
- Fixed vector controls (e.g. look, walk) not being set to zero when paused, causing them to continue moving if you pause while moving
