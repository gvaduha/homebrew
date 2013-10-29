#if there's no PIL, install pillow package for 3.x python
import PIL.Image as Image
import os

def findTag(f, tag):
	tagLen = len(tag)
	txt = f.read(tagLen)
	while(True):
		if(len(txt) < tagLen):
			return -1
		if(txt == tag):
			return f.tell()-tagLen
		txt = txt[1:] + f.read(1)

def seekToTag(f, tag):
	pos = findTag(f, tag)
	if pos != -1: f.seek(pos)
	return pos

def findTags(f, tag):
	tags = []
	pos = findTag(f,tag)
	while pos != -1:
		tags.append(pos)
		pos = findTag(f,tag)
	return tags

def splitFile(f, chunks):
        '''chunks is a [(pos, size), ...]'''
        name='split[0x%x-0x%x]'
        saved = []
        for start,size in chunks:
                f.seek(start)
                b = f.read(size)
                foName = str(name % (start,size))
                fo = open(foName, mode='wb+')
                fo.write(b)
                fo.close()
                saved.append(foName)
        return saved

def makeJpegFiles(files):
	jpgCnt = 0
	for f in files:
		try:
                        Image.open(f).save(f+".jpg")
		except IOError as e:
			print('Error processing %s: %s' % (f, str(e)))


def main():
        jpgTag = b'\xff\xd8'
        def continousChunks(tagPos):
                chunks = [(a,b-a) for a,b in zip(tagPos[:len(tagPos)],tagPos[1:])]
                return chunks
        def fixedLengthChunks(tagPos, length):
                chunks = [(a,length) for a in tagPos]
                return chunks
        f = open('x:/IndiaPhoto.bin',mode='rb')
        tags = findTags(f, jpgTag)
        tags = sorted(tags)
        #chunks = continousChunks(tags + [os.path.getsize(f.name)]) #include end of file to split last part
        chunks = fixedLengthChunks(tags, 99999999)
        files = splitFile(f, chunks)
        makeJpegFiles(files)
        f.close()
