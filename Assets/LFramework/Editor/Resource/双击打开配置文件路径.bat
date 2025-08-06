@echo off
setlocal

rem 获取当前目录的完整路径
set "currentDir=%cd%"

rem 获取当前目录的父目录名称
for %%I in ("%currentDir%") do set "parentDir=%%~nxI"

rem 构建目标文件夹路径
set "targetPath=%currentDir%\%parentDir%_Data\StreamingAssets\"

rem 检查目标文件夹是否存在并打开
if exist "%targetPath%" (
    echo 正在打开: %targetPath%
    start "" "%targetPath%"
) else (
    echo 目标文件夹不存在: %targetPath%
)

endlocal
