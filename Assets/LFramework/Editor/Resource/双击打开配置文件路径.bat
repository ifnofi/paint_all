@echo off
setlocal

rem ��ȡ��ǰĿ¼������·��
set "currentDir=%cd%"

rem ��ȡ��ǰĿ¼�ĸ�Ŀ¼����
for %%I in ("%currentDir%") do set "parentDir=%%~nxI"

rem ����Ŀ���ļ���·��
set "targetPath=%currentDir%\%parentDir%_Data\StreamingAssets\"

rem ���Ŀ���ļ����Ƿ���ڲ���
if exist "%targetPath%" (
    echo ���ڴ�: %targetPath%
    start "" "%targetPath%"
) else (
    echo Ŀ���ļ��в�����: %targetPath%
)

endlocal
