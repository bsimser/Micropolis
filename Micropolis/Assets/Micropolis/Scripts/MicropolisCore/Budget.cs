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
            long fireInt = (int)(fireFund * firePercent);
            long policeInt = (int)(policeFund * policePercent);
            long roadInt = (int)(roadFund * roadPercent);

            long total = fireInt + policeInt + roadInt;

            long yumDuckets = taxFund + totalFunds;

            if (yumDuckets > total)
            {

                // Enough yumDuckets to fully fund fire, police and road.

                fireValue = fireInt;
                policeValue = policeInt;
                roadValue = roadInt;

                // todo Why are we not subtracting from yumDuckets what we
                // spend, like the code below is doing?

            }
            else if (total > 0)
            {

                //assert(yumDuckets <= total);

                // Not enough yumDuckets to fund everything.
                // First spend on roads, then on fire, then on police.

                if (yumDuckets > roadInt)
                {

                    // Enough yumDuckets to fully fund roads.

                    roadValue = roadInt;
                    yumDuckets -= roadInt;

                    if (yumDuckets > fireInt)
                    {

                        // Enough yumDuckets to fully fund fire.

                        fireValue = fireInt;
                        yumDuckets -= fireInt;

                        if (yumDuckets > policeInt)
                        {

                            // Enough yumDuckets to fully fund police.
                            // Hey what are we doing here? Should never get here.
                            // We tested for yumDuckets > total above
                            // (where total = fireInt + policeInt + roadInt),
                            // so this should never happen.

                            policeValue = policeInt;
                            yumDuckets -= policeInt;

                        }
                        else
                        {

                            // Fuly funded roads and fire.
                            // Partially fund police.

                            policeValue = yumDuckets;

                            if (yumDuckets > 0)
                            {

                                // Scale back police percentage to available cash.

                                policePercent = ((float)yumDuckets) / ((float)policeFund);

                            }
                            else
                            {

                                // Exactly nothing left, so scale back police percentage to zero.

                                policePercent = 0.0f;

                            }

                        }

                    }
                    else
                    {

                        // Not enough yumDuckets to fully fund fire.

                        fireValue = yumDuckets;

                        // No police after funding roads and fire.

                        policeValue = 0;
                        policePercent = 0.0f;

                        if (yumDuckets > 0)
                        {

                            // Scale back fire percentage to available cash.

                            firePercent =
                                ((float)yumDuckets) / ((float)fireFund);

                        }
                        else
                        {

                            // Exactly nothing left, so scale back fire percentage to zero.

                            firePercent = 0.0f;

                        }

                    }

                }
                else
                {

                    // Not enough yumDuckets to fully fund roads.

                    roadValue = yumDuckets;

                    // No fire or police after funding roads.

                    fireValue = 0;
                    policeValue = 0;
                    firePercent = 0.0f;
                    policePercent = 0.0f;

                    if (yumDuckets > 0)
                    {

                        // Scale back road percentage to available cash.

                        roadPercent = ((float)yumDuckets) / ((float)roadFund);

                    }
                    else
                    {

                        // Exactly nothing left, so scale back road percentage to zero.

                        roadPercent = 0.0f;

                    }

                }

            }
            else
            {

                //assert(yumDuckets == total);
                //assert(total == 0);

                // Zero funding, so no values but full percentages.

                fireValue = 0;
                policeValue = 0;
                roadValue = 0;
                firePercent = 1.0f;
                policePercent = 1.0f;
                roadPercent = 1.0f;

            }

            noMoney(fromMenu, total, yumDuckets);
        }

        private void noMoney(bool fromMenu, long total, long yumDuckets)
        {
            if (!autoBudget || fromMenu)
            {
                // FIXME: This might have blocked on the Mac, but now it's asynchronous.
                // Make sure the stuff we do just afterwards is intended to be done immediately
                // and is not supposed to wait until after the budget dialog is dismissed.
                // Otherwise don't do it after this and arrange for it to happen when the
                // modal budget dialog is dismissed.
                showBudgetWindowAndStartWaiting();

                // FIXME: Only do this AFTER the budget window is accepted.

                if (!fromMenu)
                {
                    fireSpend = fireValue;
                    policeSpend = policeValue;
                    roadSpend = roadValue;

                    total = fireSpend + policeSpend + roadSpend;

                    long moreDough = (long)(taxFund - total);
                    spend((int)-moreDough);

                }

                mustDrawBudget = 1;
                doUpdateHeads();
            }
            else
            { 
                /* autoBudget & !fromMenu */

                // FIXME: Not sure yumDuckets is the right value here. It gets the
                // amount spent subtracted from it above in some cases, but not if
                // we are fully funded. I think we want to use the original value
                // of yumDuckets, which is taxFund + totalFunds.

                if (yumDuckets > total)
                {

                    long moreDough = (long)(taxFund - total);
                    spend((int) -moreDough);

                    fireSpend = fireFund;
                    policeSpend = policeFund;
                    roadSpend = roadFund;

                    mustDrawBudget = 1;
                    doUpdateHeads();

                }
                else
                {

                    setAutoBudget(false); /* force autobudget */
                    mustUpdateOptions = true;
                    sendMessage((short) MessageNumber.MESSAGE_NO_MONEY, NOWHERE, NOWHERE, false, true);
                    noMoney(fromMenu, total, yumDuckets);
                }
            }
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