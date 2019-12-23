set heading off;
set termout off; --no terminal out
set echo off; --no sql command echo
set linesize 32000;
set trimspool on; --do not write spaces to the end of the line up to its size
set trimout on; 
set pagesize 40000;
set long 50000;
spool myfile.csv

ALTER SESSION SET NLS_DATE_FORMAT = 'YYYY-MM-DD HH24:MI:SS';

select
   name||','||address from address_book;

spool off;
set echo on;
set termout on;
