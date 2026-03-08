using Shared;

// Se solicita al usuario que ingrese las posiciones de los caballos en notación algebraica
Console.Write("Ingrese ubicación de los caballos: ");
string? input = Console.ReadLine();
if (string.IsNullOrWhiteSpace(input))
{
    Console.WriteLine("Ingrese ubicación de los caballos: ");
    return;
}
foreach (var line in ConsoleExtension.AnalyzeHorseConflicts(input))
{
    Console.WriteLine(line);
}