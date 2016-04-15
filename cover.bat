c:\projects\sharphdf\src\sharpHDF\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe ^
	-register:user ^
	-output:c:\projects\sharphdf\sharphdf-coverage.xml ^
	"-filter:+[sharpHDF]*  -[sharpHDF]sharpHDF.Properties.*" ^
	-excludebyattribute:"System.CodeDom.Compiler.GeneratedCodeAttribute" ^
	"-target:c:\projects\sharphdf\test.bat"