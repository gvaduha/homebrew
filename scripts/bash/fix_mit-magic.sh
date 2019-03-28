#! /usr/bin/env bash
# 1st param is user name for whom correct MIT-COOKIE installed
TEMP-COOKIE=$(su $1 -c xauth list $DISPLAY)
echo $TEMP-COOKIE
xauth add $TEMP-COOKIE
