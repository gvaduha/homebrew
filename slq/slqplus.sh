#! /bin/bash

#sqlplus.exe user/pass@host
#sql>@script.sql
nohup sqlplus user/pass@host -s @script.sql > /dev/null
