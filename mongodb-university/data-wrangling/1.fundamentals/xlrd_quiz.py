#!/usr/bin/env python
"""
Your task is as follows:
- read the provided Excel file
- find and return the min, max and average values for the COAST region
- find and return the time value for the min and max entries
- the time values should be returned as Python tuples

Please see the test function for the expected return format
"""

import os
import sys
import xlrd
from zipfile import ZipFile
datafile = "2013_ERCOT_Hourly_Load_Data.xls"


def open_zip(datafile):
    with ZipFile('{0}.zip'.format(datafile), 'r') as myzip:
        myzip.extractall()


def parse_file(datafile):
    workbook = xlrd.open_workbook(datafile)
    sheet = workbook.sheet_by_index(0)

    data = {
            'maxtime': (0, 0, 0, 0, 0, 0),
            'maxvalue': sys.float_info.min,
            'mintime': (0, 0, 0, 0, 0, 0),
            'minvalue': sys.float_info.max,
            'avgcoast': 0
    }

    row = 1
    maxcell = 0
    mincell = 0
    while row <= sheet.nrows-1:
        if row % 500 == 0: print row
        rowval = sheet.cell_value(row, 1)
        if data['maxvalue'] < rowval:
            data['maxvalue'] = rowval
            maxcell = row
        if data['minvalue'] > rowval:
            data['minvalue'] = rowval
            mincell = row
        data['avgcoast'] += rowval
        row += 1

    data['maxtime'] = xlrd.xldate_as_tuple(sheet.cell_value(maxcell, 0), 0)
    data['mintime'] = xlrd.xldate_as_tuple(sheet.cell_value(mincell, 0), 0)
    data['avgcoast'] = data['avgcoast'] / (sheet.nrows-1)

    return data


def test():
    print "Unzip..."
    open_zip(datafile)
    print "Process..."
    data = parse_file(datafile)
    print data
    print "Delete..."
    os.remove(datafile)
    print "Done..."

    assert data['maxtime'] == (2013, 8, 13, 17, 0, 0)
    assert round(data['maxvalue'], 10) == round(18779.02551, 10)


test()