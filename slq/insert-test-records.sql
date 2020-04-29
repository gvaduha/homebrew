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
