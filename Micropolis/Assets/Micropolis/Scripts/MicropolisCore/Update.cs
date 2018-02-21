using UnityEngine;

namespace Micropolis.MicropolisCore
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
        }

        public void updateGraphs()
        {
        }

        public void updateEvaluation()
        {
        }

        public void updateHeads()
        {
        }

        /// <summary>
        /// Set a flag that the funds display is out of date.
        /// </summary>
        public void updateFunds()
        {
        }

        public void reallyUpdateFunds()
        {
        }

        public void doTimeStuff()
        {
            updateDate();
        }

        public void updateDate()
        {
            int megalinium = 1000000;

            cityTimeLast = cityTime >> 2;

            cityYear = (int)cityTime / 48 + startingYear;
            cityMonth = ((int)cityTime % 48) >> 2;

            if (cityYear >= megalinium)
            {
                setYear(startingYear);
                cityYear = startingYear;
                //sendMessage(MESSAGE_NOT_ENOUGH_POWER, NOWHERE, NOWHERE, true);
            }

            if ((cityYearLast != cityYear) ||
                (cityMonthLast != cityMonth))
            {
                cityYearLast = cityYear;
                cityMonthLast = cityMonth;

                //callback("update", "s", "date");
                Debug.Log(string.Format(
                    "cityTime:{0} startingYear:{1} year:{2} month:{3}",
                    cityTime,
                    startingYear,
                    cityTime/48+startingYear,
                    (cityTime%48)>>2
                    ));
            }
        }

        public void showValves()
        {
            if (valveFlag)
            {
                drawValve();
                valveFlag = false;
            }
        }

        public void drawValve()
        {
        }

        public void setDemand(float r, float c, float i)
        {
        }

        public void updateOptions()
        {
        }

        public void updateUserInterface()
        {
        }
    }
}
