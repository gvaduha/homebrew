db.zips.aggregate([
{$match: {$or: [ {state: "CT"}, {state: "NJ"}]}},
{$match: {pop: {$gt: 25000}}},
{$group: 
 { 
	_id: {ST:"$state",CT:"$city"}, 
	total:{$sum:"$pop"} 
 }
},
{$sort: {_id: 1}}
//{$group: {_id: "", AVG: {$avg: "$total"}}}
]);
