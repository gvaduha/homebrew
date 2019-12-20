set heading off
set termout off
spool myfile.csv

select
   name||','||address from address_book;

spool off;
set termout on
