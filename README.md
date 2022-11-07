# PowerShell Runner

The .NET assembly allows to execute powershell commands from an unmanaged namespace. It also allows to import powershell scripts.
Command syntax for running powershell:
`PS.exe <base64 encoded command>`
Command syntax for importing a module:
`PS.exe loadmodule<base64 encoded script>`

The code was obtained by reversing `PS.exe` original binary created for the PoshC2 framework here https://github.com/nettitude/PoshC2/blob/master/resources/modules/PS.exe. 
# Using it with Sliver C2
Once compiled you can use it inside sliver c2 using this branch of the framework https://github.com/MrAle98/sliver/tree/feat/powershell. Convert PS.exe to a go slice of bytes and store it inside 
`sliver/client/command/powershell/PS.go` in place of the `PSRunner` variable.

Test the `powershell` and `powershell-import` commands.
