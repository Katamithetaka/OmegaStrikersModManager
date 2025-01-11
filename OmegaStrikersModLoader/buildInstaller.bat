@REM # download https://github.com/trumank/repak/releases/download/v0.2.2/repak_cli-x86_64-pc-windows-msvc.zip

mkdir .\bin\Release\net8.0\repak\temp\windows

call curl.exe -o ./bin/Release/net8.0/repak/temp/windows/repak_cli-x86_64-pc-windows-msvc.zip -fL "https://github.com/trumank/repak/releases/latest/download/repak_cli-x86_64-pc-windows-msvc.zip"
call curl.exe -o ./bin/Release/net8.0/repak/temp/repak_cli-x86_64-unknown-linux-gnu.tar.xz -fL "https://github.com/trumank/repak/releases/latest/download/repak_cli-x86_64-unknown-linux-gnu.tar.xz"


pushd .\bin\Release\net8.0\repak\temp

"C:\Program Files\7-Zip\7z.exe" x "repak_cli-x86_64-unknown-linux-gnu.tar.xz" 
"C:\Program Files\7-Zip\7z.exe" x "repak_cli-x86_64-unknown-linux-gnu.tar" 


cd repak_cli-x86_64-unknown-linux-gnu

move repak ..\..\..\repak.elf


popd


pushd .\bin\Release\net8.0\repak\temp\windows

tar -xf repak_cli-x86_64-pc-windows-msvc.zip
move repak.exe ..\..\..\


popd 


rd /s /q ".\bin\Release\net8.0\repak\"

call iscc /O./Output /Q InstallerScript.iss 
pause