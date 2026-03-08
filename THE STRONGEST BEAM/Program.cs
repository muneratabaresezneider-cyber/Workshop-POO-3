using Shared;

Console.Write("Ingrese la viga: ");
string? beam = Console.ReadLine();
if (beam == null)
{
    Console.WriteLine("La viga está mal construida!");
    return;
}

string result = ConsoleExtension.EvaluateBeam(beam.Trim());
Console.WriteLine(result);
