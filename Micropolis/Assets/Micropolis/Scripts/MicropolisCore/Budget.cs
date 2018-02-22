namespace MicropolisCore
{
    public partial class Micropolis
    {
        public void initFundingLevel()
        {
            firePercent = 1.0f;
            fireValue = 0;
            policePercent = 1.0f;
            policeValue = 0;
            roadPercent = 1.0f;
            roadValue = 0;
            mustDrawBudget = 1;
        }

        /// <summary>
        /// Game decided to show the budget window
        /// </summary>
        public void doBudget()
        {
            doBudgetNow(false);
        }

        /// <summary>
        /// User queried the budget window
        /// </summary>
        public void doBudgetFromMenu()
        {
            doBudgetNow(true);
        }

        /// <summary>
        /// Handle budget window.
        /// </summary>
        /// <param name="fromMenu">User requested the budget window</param>
        public void doBudgetNow(bool fromMenu)
        {
            // TODO
        }

        public void updateBudget()
        {
            if (mustDrawBudget != 0)
            {
                callback("update", "s", "budget");
                mustDrawBudget = 0;
            }
        }

        public void showBudgetWindowAndStartWaiting()
        {
            callback("showBudgetAndWait", "");
        }

        public void setCityTax(short tax)
        {
            cityTax = tax;
            callback("update", "s", "taxRate");
        }
    }
}