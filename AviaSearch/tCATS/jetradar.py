
def define_lowest_price(search_reply):
	base_cur = 'usd'
	base_rate = search_reply['currency_rates'][base_cur]
	cur_rates = dict([(k,base_rate/v) for k,v in search_reply['currency_rates'].items()])
	# usd = eur / cur_rates['eur'] # revert to eliminate division by zero for 0.0 rates

	sellers_cur = dict([(t['id'], t['currency_code']) for t in search_reply['gates_info']])

	sellers_rates = dict(map(lambda x: (str(x),cur_rates[sellers_cur[x]]), sellers_cur))

	for t in search_reply['tickets']:
		t['lowest_price'] = min([v for k,v in t['native_prices'].items()])



with open('c:/2.json', 'r', encoding='utf-8') as f:
	import json
	search_reply = json.load(f)

define_lowest_price(search_reply)

tickets = search_reply['tickets']

sorted_tickets = sorted(tickets, key=lambda x: x['lowest_price'])

from pprint import pprint as pp

pp(sorted_tickets)
