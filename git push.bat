git add .
set /p var=请输入构建备注：
git commit -m "%var%"
git push
pause
exit