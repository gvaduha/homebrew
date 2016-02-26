#! /usr/bin/env bash
if [ -z "$2" ]
  then echo "use: $0 bookid lastpagenum"; exit
fi
BOOKID=$1
BOOKROOT="http://mreadz.com/new/index.php?id=$BOOKID"
LASTPAGE=$2
BOOKNAME="_book.html"
echo starting $BOOKROOT for $LASTPAGE pages in $BOOKNAME
echo '<!DOCTYPE html><html><head><meta charset="utf-8"></head><body>' > $BOOKNAME
for i in $(seq 1 1 $LASTPAGE)
do 
	wget $BOOKROOT\&pages=$i
	egrep 'h3>|h4>|p>' index.php@id\=$BOOKID\&pages\=$i >> _book.html
done
echo '</body></html>' >> $BOOKNAME

#BOOKROOT="http://litread.me/pages/439821/480000-481000"
#wget $BOOKROOT\?page=$i
#egrep 'h3>|h4>|p>' 480000-481000@page\=$i >> _book.html
