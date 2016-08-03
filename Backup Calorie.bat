
For /f "tokens=1-3 delims=/ " %%a in ('date /t') do (set mydate=%%c-%%b-%%a)
For /f "tokens=1-2 delims=/:" %%a in ('time /t') do (set mytime=%%a%%b)
echo %mydate%_%mytime%

"C:\Program Files (x86)\7-Zip\7z.exe" a -r "C:\Projects\CalorieOffsetting%mydate%_%mytime%.zip" C:\Projects\CalorieOffsetting

COPY C:\Projects\CalorieOffsetting%mydate%_%mytime%.zip "Z:\Google Drive\Source Code\Calorie"

DEL C:\Projects\CalorieOffsetting%mydate%_%mytime%.zip

RMDIR /S /Q "Z:\Google Drive\Source Code\Calorie\Calorie"

xcopy "C:\Projects\CalorieOffsetting\Calorie" "Z:\Google Drive\Source Code\Calorie\Calorie\" /s/h/e/k/f/c

