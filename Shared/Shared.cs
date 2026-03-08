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

            var parts = rest.Split('*');
            if (parts.Any(p => p.Length == 0))
                return "La viga está mal construida!";

            var seqWeights = new List<long>();

            foreach (var p in parts)
            {
                if (!p.All(c => c == '='))
                    return "La viga está mal construida!";

                long k = p.Length;
                long w = k * (k + 1) / 2; 
                seqWeights.Add(w);
            }

            long total = 0;
            for (int i = 0; i < seqWeights.Count; i++)
            {
                total += seqWeights[i];
                if (i > 0)
                {
                    long connWeight = 2 * seqWeights[i - 1];
                    total += connWeight;
                }
            }

            return baseStrength >= total
                ? "La viga soporta el peso!"
                : "La viga NO soporta el peso!";
        }
    }
}