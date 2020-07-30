param([string]$name)

process
{
    mkdir $name
    mkdir $name/Source
    mkdir $name/Expected
    mkdir $name/Diagnostic
    echo "[]" > $name/Diagnostic/Result.json
}