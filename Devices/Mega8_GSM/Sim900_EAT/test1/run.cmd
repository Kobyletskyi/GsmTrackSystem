mkdir .\output
rem perl C:\ARM\SIM900DevIDE_DTMF_MultiTask_GCC\EmbatSIM\gsmos\tools\globalmacro
perl C:\ARM\SIM900DevIDE_DTMF_MultiTask_GCC\EmbatSIM\gsmos\tools\renew
make OS=win  APPDIR=%cd%
if exist code\*.o  move code\*.o     .\output
if exist C:\ARM\SIM900DevIDE_DTMF_MultiTask_GCC\EmbatSIM\gsmos\flcode\*.o move C:\ARM\SIM900DevIDE_DTMF_MultiTask_GCC\EmbatSIM\gsmos\flcode\*.o .\output
if exist *.o move *.o     .\output
if exist *.cla move *.cla   .\output
if exist *.elf move *.elf   .\output
if exist *.map move *.map   .\output
if exist *.sym   move *.sym   .\output
pause