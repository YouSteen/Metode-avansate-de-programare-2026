# Genereaza PNG din fisierele Mermaid din docs/ (ruleaza din Lab1-2; root = folderul docs)
$ErrorActionPreference = "Stop"
$root = Split-Path (Split-Path $MyInvocation.MyCommand.Path -Parent) -Parent

$jobs = @(
    @{ In = "cerinta-1-diagrama-use-case\surse\use-case.mmd"; Out = "cerinta-1-diagrama-use-case\use-case.png" },
    @{ In = "cerinta-2-diagrama-clasa\surse\class-diagram-before.mmd"; Out = "cerinta-2-diagrama-clasa\class-diagram-before.png" },
    @{ In = "cerinta-2-diagrama-clasa\surse\class-diagram-after.mmd"; Out = "cerinta-2-diagrama-clasa\class-diagram-after.png" }
)

foreach ($j in $jobs) {
    $input = Join-Path $root $j.In
    $output = Join-Path $root $j.Out
    npx --yes @mermaid-js/mermaid-cli -i $input -o $output -b transparent
    Write-Host "Generat: $($j.Out)"
}

Write-Host "Gata."
