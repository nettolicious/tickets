[string]$dbContextName = "TicketsDbContext"
[string]$entitiesFolder = "../Contracts/Persistence/Entities"
[string]$connString = "Server=.;Database=Tickets;Integrated Security=SSPI"
[string[]]$tables = "Order",
  "Ticket"
[string]$dbContext = ""
[string]$modelFolder = ""
[string]$scaffoldCommand = ""
[string]$table = ""

$dbContext = Join-Path -Path $PSScriptRoot -ChildPath "$($dbContextName).cs"
$modelFolder = Join-Path -Path $PSScriptRoot -ChildPath $entitiesFolder

Write-Host "Begin scaffolding..."
$scaffoldCommand = "dotnet ef dbcontext scaffold ""$($connString)"" Microsoft.EntityFrameworkCore.SqlServer -d -f -o ""$($entitiesFolder)"" -c ""$($dbContextName)"" --context-dir ""./"" --verbose --json"
foreach ($table in $tables) {
  $scaffoldCommand += " -t $($table)"
}
Invoke-Expression -Command $scaffoldCommand | Write-Host

Write-Host "Complete"