#!/usr/bin/env bash

# Linecount by dir
find . \( -name "*.cpp" -or -name "*.h*" \) -exec wc -l {} +
# Linecount total
find . -type f \( -name "*.cpp" -or -name "*.h*" \) -exec cat {} + | wc -l

# Find all types of tags on comments i.e. //TODO: //HACK: //EXT: etc skipping namespaces like ATL::
find . \( -name "*.cpp" -or -name "*.h*" \) -exec sed 's/.*\/\/[ \\t]*\([A-Z]\{2,10\}:\)[^:].\+$/\1/;tx;d;:x' {} + | sort | uniq

# Sync remote
rsync -arv --exclude .svn --exclude .git ~/sources gvaduha@buildline:~/robot/queue
