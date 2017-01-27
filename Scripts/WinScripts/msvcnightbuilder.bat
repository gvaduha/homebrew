setlocal enabledelayedexpansion
set VSBAT="C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\VsDevCmd.bat"
set SCRIPT="x_tmp_build_x.bat"

set BUILDS[0]="d:\proj\1.0"
set BUILDS[0]="d:\proj\1.5"
set BUILDS[0]="d:\proj\2.0"

for %%i in (0,1,2) do (

rem === Temp build file ===
  type %VSBAT% > %SCRIPT%

  echo pushd !BUILDS[%%i]! >> %SCRIPT%

  echo svn update . >> %SCRIPT%

  echo rem devenv my.sln /clean >> %SCRIPT%
  echo devenv my.sln /build Debug >> %SCRIPT%
  echo rem devenv my.sln /build Release >> %SCRIPT%

  echo popd >> %SCRIPT%
rem === Temp build file end ===

  call %SCRIPT%

  del %SCRIPT%
)
