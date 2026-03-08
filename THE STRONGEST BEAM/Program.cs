using Shared;
var answer = string.Empty;
var options = new List<string> { "s", "n" };


do
{
    Console.Write("Ingrese la viga: ");
    string? beam = Console.ReadLine();
    if (beam == null)
    {
        Console.WriteLine("La viga está mal construida!");
        return;
    }

    string result = ConsoleExtension.EvaluateBeam(beam.Trim());
    Console.WriteLine(result);
    do
    {
        answer = ConsoleExtension.GetValidOptions("¿Deseas continuar [S]í, [N]o?: ", options);
    } while (!options.Any(x => x.Equals(answer, StringComparison.CurrentCultureIgnoreCase)));

} while (answer!.Equals("s", StringComparison.CurrentCultureIgnoreCase));
Console.WriteLine("has salido del programa.");




