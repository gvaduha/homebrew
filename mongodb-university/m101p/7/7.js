use ps
var im = db.images.find({});
var counter = 0;
while (im.hasNext()) {
	var i = im.next();
	var al = db.albums.findOne({ "images": i._id });
	if (al == null)
	{
		++counter;
		db.images.remove({"_id": i._id});
	}
}
print(counter);	
