#! /bin/bash
DISPLNAME="Virtual1"
RESSTR=$(cvt 1920 1080 60 | tail -n 1 | cut -d ' ' -f2-)
MODESTR=$(cut -d ' ' -f1 <<< $RESSTR)
echo $RESSTR
xrandr --newmode $RESSTR
xrandr --addmode $DISPLNAME $MODESTR
xrandr --output $DISPLNAME --mode $MODESTR
xrandr -q
