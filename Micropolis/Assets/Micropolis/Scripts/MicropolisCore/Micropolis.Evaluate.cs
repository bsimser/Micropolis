using System;

namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Evaluate city
        /// TODO Handle lack of voting explicitly
        /// </summary>
        public void cityEvaluation()
        {
            if (totalPop > 0)
            {
                short[] problemTable = new short[(int) CityVotingProblems.PROBNUM];
                for (int z = 0; z < (int) CityVotingProblems.PROBNUM; z++)
                {
                    problemTable[z] = 0;
                }

                getAssessedValue();
                doPopNum();
                doProblems(problemTable);
                getScore(problemTable);
                doVotes(); // How well is the mayor doing?
                changeEval();
            }
            else
            {
                evalInit();
                cityYes = 50; // No population => no voting. Let's say 50/50
                changeEval();
            }
        }

        /// <summary>
        /// Initialize evaluation variables
        /// </summary>
        public void evalInit()
        {
            cityYes = 0;
            cityPop = 0;
            cityPopDelta = 0;
            cityAssessedValue = 0;
            cityClass = CityClass.CC_VILLAGE;
            cityScore = 500;
            cityScoreDelta = 0;
            for (int i = 0; i < (int)CityVotingProblems.PROBNUM; i++)
            {
                problemVotes[i] = 0;
            }
            for (int i = 0; i < (int)CityVotingProblems.CVP_PROBLEM_COMPLAINTS; i++)
            {
                problemOrder[i] = (int) CityVotingProblems.CVP_NUMPROBLEMS;
            }
        }

        /// <summary>
        /// Assess value of the city.
        /// TODO Make function return the value, or change the name of the function.
        /// </summary>
        public void getAssessedValue()
        {
            long z;

            z = roadTotal * 5;
            z += railTotal * 10;
            z += policeStationPop * 1000;
            z += fireStationPop * 1000;
            z += hospitalPop * 400;
            z += stadiumPop * 3000;
            z += seaportPop * 5000;
            z += airportPop * 10000;
            z += coalPowerPop * 3000;
            z += nuclearPowerPop * 6000;

            cityAssessedValue = z * 1000;
        }

        /// <summary>
        /// Compute city population and city classification.
        /// </summary>
        public void doPopNum()
        {
            long oldCityPop = cityPop;

            cityPop = getPopulation();

            if (oldCityPop == -1)
            {
                oldCityPop = cityPop;
            }

            cityPopDelta = cityPop - oldCityPop;
            cityClass = getCityClass(cityPop);
        }

        /// <summary>
        /// Compute city population.
        /// </summary>
        /// <returns></returns>
        public long getPopulation()
        {
            long pop = (resPop + (comPop + indPop) * 8L) * 20L;
            return pop;
        }

        /// <summary>
        /// Classify the city based on its population.
        /// </summary>
        /// <param name="cityPopulation">Number of people in the city.</param>
        /// <returns>City classification.</returns>
        private CityClass getCityClass(long cityPopulation)
        {
            CityClass cityClassification = CityClass.CC_VILLAGE;

            if (cityPopulation > 2000)
            {
                cityClassification = CityClass.CC_TOWN;
            }
            if (cityPopulation > 10000)
            {
                cityClassification = CityClass.CC_CITY;
            }
            if (cityPopulation > 50000)
            {
                cityClassification = CityClass.CC_CAPITAL;
            }
            if (cityPopulation > 100000)
            {
                cityClassification = CityClass.CC_METROPOLIS;
            }
            if (cityPopulation > 500000)
            {
                cityClassification = CityClass.CC_MEGALOPOLIS;
            }

            return cityClassification;
        }

        /// <summary>
        /// Evaluate problems of the city, take votes, and decide which are the most
        /// important ones.
        /// problemTable contains severity of each problem,
        /// problemVotes contains votes of each problem,
        /// problemOrder contains (in decreasing order) the worst problems.
        /// </summary>
        /// <param name="problemTable">Storage of how bad each problem is.</param>
        private void doProblems(short[] problemTable)
        {
            bool[] problemTaken = new bool[(int) CityVotingProblems.PROBNUM]; // Which problems are taken?

            for (int z = 0; z < (int) CityVotingProblems.PROBNUM; z++)
            {
                problemTaken[z] = false;
                problemTable[z] = 0;
            }

            problemTable[(int)CityVotingProblems.CVP_CRIME] = crimeAverage;                /* Crime */
            problemTable[(int)CityVotingProblems.CVP_POLLUTION] = pollutionAverage;            /* Pollution */
            problemTable[(int)CityVotingProblems.CVP_HOUSING] = (short) (landValueAverage * 7 / 10);   /* Housing */
            problemTable[(int)CityVotingProblems.CVP_TAXES] = (short) (cityTax * 10);                /* Taxes */
            problemTable[(int)CityVotingProblems.CVP_TRAFFIC] = getTrafficAverage();         /* Traffic */
            problemTable[(int)CityVotingProblems.CVP_UNEMPLOYMENT] = getUnemployment();           /* Unemployment */
            problemTable[(int)CityVotingProblems.CVP_FIRE] = getFireSeverity();           /* Fire */
            voteProblems(problemTable);

            for (int z = 0; z < (int) CityVotingProblems.CVP_PROBLEM_COMPLAINTS; z++)
            {
                // Find biggest problem not taken yet
                int maxVotes = 0;
                int bestProblem = (int) CityVotingProblems.CVP_NUMPROBLEMS;
                for (int i = 0; i < (int) CityVotingProblems.CVP_NUMPROBLEMS; i++)
                {
                    if ((problemVotes[i] > maxVotes) && (!problemTaken[i]))
                    {
                        bestProblem = i;
                        maxVotes = problemVotes[i];
                    }
                }

                // bestProblem == CVP_NUMPROBLEMS means no problem found
                problemOrder[z] = (short) bestProblem;
                if (bestProblem < (int) CityVotingProblems.CVP_NUMPROBLEMS)
                {
                    problemTaken[bestProblem] = true;
                }
                // else: No problem found.
                //       Repeating the procedure will give the same result.
                //       Optimize by filling all remaining entries, and breaking out
            }
        }

        /// <summary>
        /// Vote on the problems of the city.
        /// problemVotes contains the vote counts
        /// </summary>
        /// <param name="problemTable">Storage of how bad each problem is.</param>
        private void voteProblems(short[] problemTable)
        {
            for (int z = 0; z < (int) CityVotingProblems.PROBNUM; z++)
            {
                problemVotes[z] = 0;
            }

            int problem = 0; // Problem to vote for
            int voteCount = 0; // Number of votes
            int loopCount = 0; // Number of attempts
            while (voteCount < 100 && loopCount < 600)
            {
                if (getRandom(300) < problemTable[problem])
                {
                    problemVotes[problem]++;
                    voteCount++;
                }
                problem++;
                if (problem > (int) CityVotingProblems.PROBNUM)
                {
                    problem = 0;
                }
                loopCount++;
            }
        }

        /// <summary>
        /// Compute average traffic in the city.
        /// </summary>
        /// <returns>Value representing how large the traffic problem is.</returns>
        public short getTrafficAverage()
        {
            long trafficTotal;
            short x, y, count;

            trafficTotal = 0;
            count = 1;
            for (x = 0; x < WORLD_W; x += (short) landValueMap.MAP_BLOCKSIZE)
            {
                for (y = 0; y < WORLD_H; y += (short) landValueMap.MAP_BLOCKSIZE)
                {
                    if (landValueMap.worldGet(x, y) > 0)
                    {
                        trafficTotal += trafficDensityMap.worldGet(x, y);
                        count++;
                    }
                }
            }

            trafficAverage = (short)((trafficTotal / count) * 2.4);

            return trafficAverage;
        }

        /// <summary>
        /// Compute severity of unemployment
        /// </summary>
        /// <returns>Value representing the severity of unemployment problems</returns>
        public short getUnemployment()
        {
            short b = (short) ((comPop + indPop) * 8);

            if (b == 0)
            {
                return 0;
            }

            // Ratio total people / working. At least 1.
            float r = ((float)resPop) / b;

            b = (short)((r - 1) * 255); // (r - 1) is the fraction unemployed people
            return Math.Min(b, (short)255);
        }

        /// <summary>
        /// Compute severity of fire
        /// </summary>
        /// <returns>Value representing the severity of fire problems</returns>
        public short getFireSeverity()
        {
            return (short) Math.Min(firePop * 5, 255);
        }

        /// <summary>
        /// Compute total score
        /// </summary>
        /// <param name="problemTable">Storage of how bad each problem is.</param>
        private void getScore(short[] problemTable)
        {
            int x, z;
            short cityScoreLast;

            cityScoreLast = cityScore;
            x = 0;

            for (z = 0; z < (int) CityVotingProblems.CVP_NUMPROBLEMS; z++)
            {
                x += problemTable[z];       /* add 7 probs */
            }

            /**
             * @todo Should this expression depend on CVP_NUMPROBLEMS?
             */
            x = x / 3;                    /* 7 + 2 average */
            x = Math.Min(x, 256);

            z = clamp((256 - x) * 4, 0, 1000);

            if (resCap)
            {
                z = (int)(z * .85);
            }

            if (comCap)
            {
                z = (int)(z * .85);
            }

            if (indCap)
            {
                z = (int)(z * .85);
            }

            if (roadEffect < MAX_ROAD_EFFECT)
            {
                z -= MAX_ROAD_EFFECT - (int) roadEffect;
            }

            if (policeEffect < MAX_POLICE_STATION_EFFECT)
            {
                // 10.0001 = 10000.1 / 1000, 1/10.0001 is about 0.1
                z = (int)(z * (0.9 + (policeEffect / (10.0001 * MAX_POLICE_STATION_EFFECT))));
            }

            if (fireEffect < MAX_FIRE_STATION_EFFECT)
            {
                // 10.0001 = 10000.1 / 1000, 1/10.0001 is about 0.1
                z = (int)(z * (0.9 + (fireEffect / (10.0001 * MAX_FIRE_STATION_EFFECT))));
            }

            if (resValve < -1000)
            {
                z = (int)(z * .85);
            }

            if (comValve < -1000)
            {
                z = (int)(z * .85);
            }

            if (indValve < -1000)
            {
                z = (int)(z * .85);
            }

            float SM = 1.0f;
            if (cityPop == 0 || cityPopDelta == 0)
            {
                SM = 1.0f; // there is nobody or no migration happened

            }
            else if (cityPopDelta == cityPop)
            {
                SM = 1.0f; // city sprang into existence or doubled in size

            }
            else if (cityPopDelta > 0)
            {
                SM = ((float)cityPopDelta / cityPop) + 1.0f;

            }
            else if (cityPopDelta < 0)
            {
                SM = 0.95f + ((float)cityPopDelta / (cityPop - cityPopDelta));
            }

            z = (int)(z * SM);
            z = z - getFireSeverity() - cityTax; // dec score for fires and taxes

            float TM = unpoweredZoneCount + poweredZoneCount;   // dec score for unpowered zones
            if (TM > 0.0f)
            {
                z = (int)(z * (float)(poweredZoneCount / TM));
            }
            else
            {
            }

            z = clamp(z, 0, 1000);

            cityScore = (short) ((cityScore + z) / 2);

            cityScoreDelta = (short) (cityScore - cityScoreLast);
        }

        /// <summary>
        /// Vote whether the mayor is doing a good job
        /// </summary>
        public void doVotes()
        {
            int z;

            cityYes = 0;

            for (z = 0; z < 100; z++)
            {
                if (getRandom(1000) < cityScore)
                {
                    cityYes++;
                }
            }
        }

        /// <summary>
        /// Push new score to the user
        /// </summary>
        public void doScoreCard()
        {
            callback("update", "s", "evaluation");

            // The user interface should pull these raw values out and format
            // them. The simulator core used to format them and push them out,
            // but the user interface should pull them out and format them
            // itself.

            // City Evaluation ${FormatYear(currentYear())}
            // Public Opinion
            //   Is the mayor doing a good job?
            //     Yes: ${FormatPercent(cityYes)}
            //     No: ${FormatPercent(100 - cityYes)}
            //   What are the worst problems?
            //     for i in range(0, CVP_PROBLEM_COMPLAINTS),
            //                 while problemOrder[i] < CVP_NUMPROBLEMS:
            //     ${probStr[problemOrder[i]]}:
            //                  ${FormatPercent(problemVotes[problemOrder[i]])}
            // Statistics
            //   Population: ${FormatNumber(cityPop)}
            //   Net Migration: ${FormatNumber(cityPopDelta)} (last year)
            //   Assessed Value: ${FormatMoney(cityAssessedValue))
            //   Category: ${cityClassStr[cityClass]}
            //   Game Level: ${cityLevelStr[gameLevel]}
        }

        /// <summary>
        /// Request that new score is displayed to the user.
        /// </summary>
        public void changeEval()
        {
            evalChanged = true;
        }

        /// <summary>
        /// Update the score after being requested.
        /// </summary>
        public void scoreDoer()
        {
            if (evalChanged)
            {
                doScoreCard();
                evalChanged = false;
            }
        }

        /// <summary>
        /// Return number of problem in the city.
        /// </summary>
        /// <returns>Number of problems.</returns>
        public int countProblems()
        {
            int i;
            for (i = 0; i < (int)CityVotingProblems.CVP_PROBLEM_COMPLAINTS; i++)
            {
                if (problemOrder[i] == (int)CityVotingProblems.CVP_NUMPROBLEMS)
                {
                    break;
                }
            }
            return i;
        }

        /// <summary>
        /// Return the index of the i-th worst problem.
        /// </summary>
        /// <param name="i">Number of the problem.</param>
        /// <returns>
        /// Index into the #problemOrder table of the i-th problem. 
        /// Returns -1 if such a problem does not exist.
        /// </returns>
        public int getProblemNumber(int i)
        {
            if (i < 0 || i >= (int) CityVotingProblems.CVP_PROBLEM_COMPLAINTS
                || problemOrder[i] == (int) CityVotingProblems.CVP_NUMPROBLEMS)
            {
                return -1;
            }
            else
            {
                return problemOrder[i];
            }
        }

        /// <summary>
        /// Return number of votes to solve the i-th worst problem.
        /// </summary>
        /// <param name="i">Number of the problem.</param>
        /// <returns>
        /// Number of votes to solve the i-th worst problem.
        /// Returns -1 if such a problem does not exist.
        /// </returns>
        public int getProblemVotes(int i)
        {
            if (i < 0 || i >= (int) CityVotingProblems.CVP_PROBLEM_COMPLAINTS
                || problemOrder[i] == (int) CityVotingProblems.CVP_NUMPROBLEMS)
            {
                return -1;
            }
            else
            {
                return problemVotes[problemOrder[i]];
            }
        }
    }
}
