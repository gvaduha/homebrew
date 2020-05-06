-- first script
begin
  for m in 1 .. 12 loop
    for i in 1 .. 28 loop
      insert into test_table 
      (test_date1, test_date2)
      values
      (TO_DATE('14.'||TO_CHAR(m)||'.2020 14:00:00', 'DD.MM.YYYY HH24:MI:SS'), TO_DATE('15.'||TO_CHAR(m)||'.2020 14:00:00', 'DD.MM.YYYY HH24:MI:SS'));
    end loop;
  end loop;
  commit;
end;

-- second script
interval Number(8,5) := 10 / (60 * 60 * 24);
start_time Date := TO_DATE('2019/05/11 8:30:00', 'YYYY/MM/DD HH:MI:SS');
--end_time Date   := TO_DATE('2019/05/11 8:32:00', 'YYYY/MM/DD HH:MI:SS');
end_time Date   := start_time+(interval*10);
begin
while (start_time <= end_time)
loop
    DBMS_OUTPUT.PUT_LINE(start_time);
    insert into test_table (time_from, time_to)
    values (start_time, start_time+interval);
    start_time := start_time+interval;
end loop;                                                                                      
