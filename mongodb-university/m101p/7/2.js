db.messages.aggregate([
   {$project:
	{ "headers.From": 1,
	"headers.To": { "$setUnion": [ "$headers.To", [] ] }
	}
   },
   {$unwind: "$headers.To"
   },
/*   {$match:
     {
	"scores.type": {$ne: "quiz" }
     }
   },
   {$group:
     {
	_id: {from:"$headers.From", to:"$headers.To"},
	count: {$sum:1}
     }
   },*/
   {$group:
     {
	_id: {from:"$headers.From", to:"$headers.To"},
	count: {$sum:1}
     }
   },
/*   {$match:{
       "_id.from": "susan.mara@enron.com"
	} 
   },*/
/*   {$lookup:
	{
		from: messages

	}},
*/
   {$sort: {count:-1}},

])