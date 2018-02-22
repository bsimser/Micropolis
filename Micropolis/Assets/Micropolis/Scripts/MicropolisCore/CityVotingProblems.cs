namespace Micropolis.MicropolisCore
{
    /// <summary>
    /// Problems in the city where citizens vote on
    /// </summary>
    public enum CityVotingProblems
    {
        CVP_CRIME,                    // Crime
        CVP_POLLUTION,                // Pollution
        CVP_HOUSING,                  // Housing
        CVP_TAXES,                    // Taxes
        CVP_TRAFFIC,                  // Traffic
        CVP_UNEMPLOYMENT,             // Unemployment
        CVP_FIRE,                     // Fire

        CVP_NUMPROBLEMS,              // Number of problems

        CVP_PROBLEM_COMPLAINTS = 4,   // Number of problems to complain about.

        PROBNUM = 10,
    }
}