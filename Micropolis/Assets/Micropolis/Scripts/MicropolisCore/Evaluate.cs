namespace Micropolis.MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Evaluate city
        /// TODO Handle lack of voting explicitly
        /// </summary>
        public void cityEvaluation()
        {
        }

        /// <summary>
        /// Initialize evaluation variables
        /// </summary>
        public void evalInit()
        {
        }

        /// <summary>
        /// Assess value of the city.
        /// TODO Make function return the value, or change the name of the function.
        /// </summary>
        public void getAssessedValue()
        {
        }

        /// <summary>
        /// Compute city population and city classification.
        /// </summary>
        public void doPopNum()
        {
        }

        /// <summary>
        /// Compute average traffic in the city.
        /// </summary>
        /// <returns>Value representing how large the traffic problem is.</returns>
        public short getTrafficAverage()
        {
            return 0;
        }

        /// <summary>
        /// Compute severity of unemployment
        /// </summary>
        /// <returns>Value representing the severity of unemployment problems</returns>
        public short getUnemployment()
        {
            return 0;
        }

        /// <summary>
        /// Compute severity of fire
        /// </summary>
        /// <returns>Value representing the severity of fire problems</returns>
        public short getFireSeverity()
        {
            return 0;
        }

        /// <summary>
        /// Vote whether the mayor is doing a good job
        /// </summary>
        public void doVotes()
        {
        }

        /// <summary>
        /// Push new score to the user
        /// </summary>
        public void doScoreCard()
        {
        }

        /// <summary>
        /// Request that new score is displayed to the user.
        /// </summary>
        public void changeEval()
        {
        }

        /// <summary>
        /// Update the score after being requested.
        /// </summary>
        public void scoreDoer()
        {
        }

        /// <summary>
        /// Return number of problem in the city.
        /// </summary>
        /// <returns>Number of problems.</returns>
        public int countProblems()
        {
            return 0;
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
            return 0;
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
            return 0;
        }
    }
}
