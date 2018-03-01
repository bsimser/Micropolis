namespace MicropolisCore
{
    public partial class Micropolis
    {
        public void doUpdateHeads()
        {
            showValves();
            doTimeStuff();
            reallyUpdateFunds();
            updateOptions();
        }

        public void updateMaps()
        {
            invalidateMaps();
            doUpdateHeads();
        }

        public void updateGraphs()
        {
            changeCensus();
        }

        public void updateEvaluation()
        {
            changeEval();
        }

        public void updateHeads()
        {
            mustUpdateFunds = true;
            valveFlag = true;
            cityTimeLast = cityYearLast = cityMonthLast = totalFundsLast =
                resLast = comLast = indLast = -999999;
            doUpdateHeads();
        }

        /// <summary>
        /// Set a flag that the funds display is out of date.
        /// </summary>
        public void updateFunds()
        {
            mustUpdateFunds = true;
        }

        private void reallyUpdateFunds()
        {
            if (!mustUpdateFunds)
            {
                return;
            }

            mustUpdateFunds = false;

            if (totalFunds != totalFundsLast)
            {
                totalFundsLast = totalFunds;

                callback("update", "s", "funds");
            }
        }

        private void doTimeStuff()
        {
            updateDate();
        }

        private void updateDate()
        {
            int megalinium = 1000000;

            cityTimeLast = cityTime >> 2;

            cityYear = (int)cityTime / 48 + startingYear;
            cityMonth = ((int)cityTime % 48) >> 2;

            if (cityYear >= megalinium)
            {
                setYear(startingYear);
                cityYear = startingYear;
                // BUG message is wrong
                sendMessage((short) MessageNumber.MESSAGE_NOT_ENOUGH_POWER, NOWHERE, NOWHERE, true);
            }

            if (cityYearLast != cityYear ||
                cityMonthLast != cityMonth)
            {
                cityYearLast = cityYear;
                cityMonthLast = cityMonth;

                callback("update", "s", "date");
            }
        }

        private void showValves()
        {
            if (valveFlag)
            {
                drawValve();
                valveFlag = false;
            }
        }

        private void drawValve()
        {
            float r, c, i;

            r = resValve;

            if (r < -1500)
            {
                r = -1500;
            }

            if (r > 1500)
            {
                r = 1500;
            }

            c = comValve;

            if (c < -1500)
            {
                c = -1500;
            }

            if (c > 1500)
            {
                c = 1500;
            }

            i = indValve;

            if (i < -1500)
            {
                i = -1500;
            }

            if (i > 1500)
            {
                i = 1500;
            }

            if ((r != resLast) ||
                (c != comLast) ||
                (i != indLast))
            {

                resLast = (int)r;
                comLast = (int)c;
                indLast = (int)i;

                setDemand(r, c, i);
            }
        }

        public void setDemand(float r, float c, float i)
        {
            callback("update", "s", "demand");
        }

        public void updateOptions()
        {
            if (mustUpdateOptions)
            {
                mustUpdateOptions = false;
                callback("update", "s", "options");
            }
        }

        /// <summary>
        /// TODO Keeping track of pending updates should be moved to the interface
        ///      (the simulator generates events, the interface forwards them to
        ///      the GUI when possible/allowed.
        /// </summary>
        public void updateUserInterface()
        {
            // TODO Send all pending update messages to the user interface

            // city: after load file, load scenario, or generate city
            // map: when small overall map changes
            // editor: when large close-up map changes
            // graph: when graph changes
            // evaluation: when evaluation changes
            // budget: when budget changes
            // date: when date changes
            // funds: when funds change
            // demand: when demand changes
            // level: when level changes
            // speed: when speed changes
            // delay: when delay changes
            // option: when options change
        }
    }
}
