-- user_tables or user_indexes;
select owner ||'.' || table_name as table_name,
round((num_rows * avg_row_len)/1024/1024,2) as MB,
initial_Extent
from all_tables --all_indexes
where blocks is not null and not owner in ('SYS', 'SYSTEM') 
and TABLE_NAME= '<TABLE_NAME>'
order by MB desc;


-- sql*plus --
compute sum of blocks on report
break on report
select extent_id, bytes, blocks
from user_extents
where segment_type = 'TABLE'
and segment_name = '<TABLE_NAME>';

clear breaks
select blocks, empty_blocks,
avg_space, num_freelist_blocks
from user_tables
where table_name = '<TABLE_NAME>';
-- --*-- --
