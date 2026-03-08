namespace Shared
{
    public static class ConsoleExtension
    {
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
                    total += 2 * currSeq; 

                    currSeq = 0;
                }
                else
                {
                    return "La viga está mal construida!";
                }
            }

            if (currSeq > 0)
                total += currSeq;
         
            return baseStrength >= total
                ? "La viga soporta el peso!"
                : "La viga NO soporta el peso!";
        }
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

            var moves = new (int dc, int dr)[] { (1,2),(2,1),(-1,2),(-2,1),(1,-2),(2,-1),(-1,-2),(-2,-1) };

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
}