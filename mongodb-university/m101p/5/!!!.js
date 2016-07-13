db.grades.aggregate([
   {$unwind: "$scores"
   },
   {$match:
     {
	"scores.type": {$ne: "quiz" }
     }
   },
   {$group:
     {
	_id: {class:"$class_id", student:"$student_id"},
	savg: {$avg:"$scores.score"}
     }
   },
   {$group:
     {
	_id: {class:"$_id.class"},
	cavg: {$avg:"$savg"}
     }
   },
   {$sort: {cavg:1}},
])