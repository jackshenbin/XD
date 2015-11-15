echo off
setlocal EnableDelayedExpansion
set name=%date:~0,4%%Date:~5,2%%date:~8,2%
set /p b=<"release note.txt"
echo %b%
set a=%b:~-11%
set version=!b:%a%=!
setlocal DisableDelayedExpansion
"C:\Program Files\WinRAR\Rar.exe" a -o+ -r -x%0 -x*.log -xcomment.txt -x*.pdb -xXD_CDZ*.exe -zcomment.txt -sfx XD_CDZ_%version%_%name%.exe