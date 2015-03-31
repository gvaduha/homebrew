#!/usr/bin/env bash

# Linecount by dir
find "$1" \( -name "*.cpp" -or -name "*.h*" \) -exec wc -l {} +
# Linecount total
find "$1" -type f \( -name "*.cpp" -or -name "*.h*" \) -exec cat {} + | wc -l

# Find all types of tags on comments i.e. //TODO: //HACK: //EXT: etc skipping namespaces like ATL::
find "$1" \( -name "*.cpp" -or -name "*.h*" \) -exec sed 's/.*\/\/[ \\t]*\([A-Z]\{2,10\}:\)[^:].\+$/\1/;tx;d;:x' {} + | sort | uniq

# Find regex $2 in directory $1
find "$1" \( -name "*.h*" -or -name "*.c*" \) -exec sh -c "cat {} | grep -n \"$2\" && echo INFILE:: {}" \;

# Sync remote
rsync -arv --exclude .svn --exclude .git ~/sources gvaduha@buildline:~/robot/queue
