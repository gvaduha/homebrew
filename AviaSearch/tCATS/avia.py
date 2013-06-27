def get_date_range(start_date, days):
	return (start_date + timedelta(x) for x in range(0,days))

# start_year, start_month, start_day, start_day_delta, trip_length, trip_length_delta
def get_trip_dates(trip_time):
	from datetime import date, timedelta

	start_date = date(*trip_time[:3])
	start_dates = get_date_range(start_date, trip_time[3])

	end_date = start_date + timedelta(trip_time[4])
	end_dates = get_date_range(end_date, trip_time[5])

	return it.product(start_dates, end_dates)


dates = (2013, 8, 12, 5, 14, 2)

trip_dates = get_trip_dates(dates)


from pprint import pprint as pp
pp(list(trip_dates))
