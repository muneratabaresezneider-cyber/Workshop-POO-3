namespace Shared;

public static class ConsoleExtension
{
    public static string GetValidOptions(string message, IEnumerable<string> options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        var opts = options.Select(o => o.Trim().ToLowerInvariant()).ToArray();
        while (true)
        {
            Console.Write(message);
            var answer = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(answer))
                continue; 
            var a = answer.Trim().ToLowerInvariant();
            if (a == "sí" || a == "si") a = "s";
            if (a == "no") a = "n";
            if (a.Length > 0)
            {
                var first = a[0].ToString();
                if (opts.Contains(first) || opts.Contains(a))
                    return first; 
            }
        }
    }
    /// Evalúa la representación textual de una viga y determina si la base soporta el peso.    
    public static string EvaluateBeam(string beam)
    {
        if (string.IsNullOrEmpty(beam))
            return "La viga está mal construida!";
        char baseChar = beam[0];
        int baseStrength = baseChar switch
        {
            '%' => 10,
            '&' => 30,
            '#' => 90,
            _ => -1
        };
        if (baseStrength < 0)
            return "La viga está mal construida!";
        string rest = beam.Substring(1);
        if (rest.Length == 0)
            return "La viga soporta el peso!"; 

        long total = 0; 
        int currSeq = 0;
        if (rest[0] != '=')
            return "La viga está mal construida!";
        for (int i = 0; i < rest.Length; i++)
        {
            char c = rest[i];
            if (c == '=')
            {
                currSeq++;
            }
            else if (c == '*')
            {
                if (currSeq == 0)
                    return "La viga está mal construida!";
                total += currSeq;
                // Cada conexión pesa el doble que la secuencia anterior.
                total += 2 * currSeq;

                // Reiniciar contador de la secuencia tras procesar la conexión.
                currSeq = 0;
            }
            else
            {
                // Cualquier otro carácter invalida la construcción.
                return "La viga está mal construida!";
            }
        }
        if (currSeq > 0)
            total += currSeq;

        // Comparar la resistencia de la base con el peso total calculado.
        return baseStrength >= total
            ? "La viga soporta el peso!"
            : "La viga NO soporta el peso!";
    }

    /// Analiza una lista de posiciones de caballos
    /// y genera líneas describiendo con qué otros caballos entra en conflicto cada uno.

    public static IEnumerable<string> AnalyzeHorseConflicts(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            yield break;
        var tokens = input.Split(',')
            .Select(t => t.Trim().ToUpper())
            .Where(t => t.Length > 0)
            .ToList();
        var positions = new List<(int col, int row, string token)>();
        foreach (var tok in tokens)
        {
            if (tok.Length != 2)
            {
                positions.Add((-1, -1, tok));
                continue;
            }
            char c = tok[0];
            char r = tok[1];
            if (c < 'A' || c > 'H' || r < '1' || r > '8')
            {
                positions.Add((-1, -1, tok));
                continue;
            }
            int col = c - 'A';
            int row = (r - '1');
            positions.Add((col, row, tok));
        }
        // Desplazamientos válidos del caballo (dc, dr).
        var moves = new (int dc, int dr)[] { (1,2),(2,1),(-1,2),(-2,1),(1,-2),(2,-1),(-1,-2),(-2,-1) };

        // Para cada caballo, buscar si alguno de los demás está en una de las casillas atacadas.
        for (int i = 0; i < positions.Count; i++)
        {
            var p = positions[i];
            string label = p.col >=0 && p.row >=0 ? $"{p.row + 1}{(char)('A' + p.col)}" : p.token;
            var conflicts = new List<string>();

            if (p.col >= 0 && p.row >= 0)
            {
                for (int j = 0; j < positions.Count; j++)
                {
                    if (i == j) continue; 
                    var q = positions[j];
                    if (q.col < 0 || q.row < 0) continue; 
                    int dc = q.col - p.col;
                    int dr = q.row - p.row;
                    if (moves.Any(m => m.dc == dc && m.dr == dr))
                    {
                        conflicts.Add($"{q.row + 1}{(char)('A' + q.col)}");
                    }
                }
            }
            if (conflicts.Count == 0)
            {
                yield return $"Analizando Caballo en {label} =>";
            }
            else
            {
                var joined = string.Join("  ", conflicts.Select(c => $"Conflicto con {c}"));
                yield return $"Analizando Caballo en {label} => {joined}";
            }
        }
    }
}
