#! /usr/bin/env bash

#  sed -i 's/\r//g' $1

# count duplicate words
IFS=$',.;:-\r\n\t '; words=($(<"$1"))
echo "${words[@]}" | tr ' ' '\n' | LC_ALL='C' sort | LC_ALL='C' uniq -c -d

#parse log excluding columns
awk -F, '{printf $1FS$7; for(i=11;i<=NF;++i) {printf FS$i;} printf "\n"}'

