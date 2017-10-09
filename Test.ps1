param([String]$Tests="Functional", [switch]$Parallel, [switch]$Build);

$paths = Get-ChildItem test -Directory | % { Join-Path $_.FullName -ChildPath ("bin/Debug/netcoreapp2.0/$($_.Name).dll") };
$filter = "";
$execute = "";

if ($Tests -eq "functional" -or $Tests -eq "f")
{
    $filter = "--TestCaseFilter:Category=Functional";
}
elseif ($Tests -eq "performance" -or $Tests -eq "p")
{
    $filter = "--TestCaseFilter:Category=Performance";
}

if ($Parallel)
{
    $execute = "--Parallel";
}

if ($Build)
{
    dotnet build AppBrix.Backgammon.sln;
}

dotnet vstest $paths $filter $execute;
