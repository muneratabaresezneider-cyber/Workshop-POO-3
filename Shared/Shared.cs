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
    }
}