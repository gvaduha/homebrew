// Arguments:
// 1 - exe image name of main application (i.e. TestApp.exe)
// 2 - IE + link to .application file of main application

// TestApp "C:\Program Files\Internet Explorer\iexplore.exe" "http://server.com/dx/?application=TestApp"

function KillProcessWithNoChildsByName(name)
{
	strcomputer = "."
	var objWMIService, colitems, e
	objWMIService = GetObject("winmgmts:{impersonationlevel=impersonate}!\\\\" + strcomputer + "\\root\\cimv2");
	colitems = objWMIService.ExecQuery("SELECT * FROM Win32_Process WHERE Name = '"+name+"'");
	e = new Enumerator(colitems);
	for (; !e.atEnd(); e.moveNext())
	{
		subproc = objWMIService.ExecQuery("SELECT * FROM Win32_Process WHERE ParentProcessId = '"+e.item().ProcessId+"'");
		se = new Enumerator(subproc);

		if (se.atEnd())
		{
			e.item().Terminate();
			return true;
		}
	}

	return false;
}

function IsProcessStarted(name)
{
	strcomputer = "."
	var objWMIService, colitems, e
	objWMIService = GetObject("winmgmts:{impersonationlevel=impersonate}!\\\\" + strcomputer + "\\root\\cimv2");
	colitems = objWMIService.ExecQuery("SELECT * FROM Win32_Process WHERE Name = '"+name+"'");
	e = new Enumerator(colitems);
	
	if (e.atEnd()) return false;
	
	return true;
}

function RunProcessAndWait(name)
{
	var wsh = new ActiveXObject("WScript.Shell");

	wsh.Run(name, 1, true);
}



var SLEEP_TIME=1000

RunProcessAndWait(WScript.Arguments.Item(1));

while(true)
{
	if ( IsProcessStarted(WScript.Arguments.Item(0)) ) break;

	WScript.Sleep(SLEEP_TIME);
}

while(true)
{
	if ( KillProcessWithNoChildsByName("dfsvc.exe") ) break;

	WScript.Sleep(SLEEP_TIME);
}
