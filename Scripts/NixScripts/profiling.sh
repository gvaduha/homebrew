#! /usr/bin/env bash

strace -e trace=open -o strace.log $1
awk 'BEGIN {FS="\""}{print $2}' strace.log | sed 's/\.\/\(.*\)/$1/' | sort | uniq -c | sort -nr
