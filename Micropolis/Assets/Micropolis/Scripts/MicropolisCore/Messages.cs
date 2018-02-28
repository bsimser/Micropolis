using System;

namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Check progress of the user, and send him messages about it.
        /// </summary>
        public void sendMessages()
        {
            short PowerPop;
            float TM;

            // Running a scenario, and waiting it to 'end' so we can give a score
            if (scenario > ScenarioType.SC_NONE && scoreType > ScenarioType.SC_NONE && scoreWait > 0)
            {
                scoreWait--;
                if (scoreWait == 0)
                {
                    doScenarioScore(scoreType);
                }
            }

            checkGrowth();

            totalZonePop = (short)(resZonePop + comZonePop + indZonePop);
            PowerPop = (short)(nuclearPowerPop + coalPowerPop);

            switch (cityTime & 63)
            {
                case 1:
                    if (totalZonePop / 4 >= resZonePop)
                    {
                        sendMessage((short) MessageNumber.MESSAGE_NEED_MORE_RESIDENTIAL);
                    }
                    break;

                case 5:
                    if (totalZonePop / 8 >= comZonePop)
                    {
                        sendMessage((short) MessageNumber.MESSAGE_NEED_MORE_COMMERCIAL);
                    }
                    break;

                case 10:
                    if (totalZonePop / 8 >= indZonePop)
                    {
                        sendMessage((short) MessageNumber.MESSAGE_NEED_MORE_INDUSTRIAL);
                    }
                    break;

                case 14:
                    if (totalZonePop > 10 && totalZonePop * 2 > roadTotal)
                    {
                        sendMessage((short) MessageNumber.MESSAGE_NEED_MORE_ROADS);
                    }
                    break;

                case 18:
                    if (totalZonePop > 50 && totalZonePop > railTotal)
                    {
                        sendMessage((short) MessageNumber.MESSAGE_NEED_MORE_RAILS);
                    }
                    break;

                case 22:
                    if (totalZonePop > 10 && PowerPop == 0)
                    {
                        sendMessage((short) MessageNumber.MESSAGE_NEED_ELECTRICITY);
                    }
                    break;

                case 26:
                    if (resPop > 500 && stadiumPop == 0)
                    {
                        sendMessage((short) MessageNumber.MESSAGE_NEED_STADIUM);
                        resCap = true;
                    }
                    else
                    {
                        resCap = false;
                    }
                    break;

                case 28:
                    if (indPop > 70 && seaportPop == 0)
                    {
                        sendMessage((short)MessageNumber.MESSAGE_NEED_SEAPORT);
                        indCap = true;
                    }
                    else
                    {
                        indCap = false;
                    }
                    break;

                case 30:
                    if (comPop > 100 && airportPop == 0)
                    {
                        sendMessage((short)MessageNumber.MESSAGE_NEED_AIRPORT);
                        comCap = true;
                    }
                    else
                    {
                        comCap = false;
                    }
                    break;

                case 32:
                    TM = (float) (unpoweredZoneCount + poweredZoneCount); // dec score for unpowered zones;
                    if (TM > 0)
                    {
                        if (poweredZoneCount / TM < 0.7f)
                        {
                            sendMessage((short) MessageNumber.MESSAGE_BLACKOUTS_REPORTED);
                        }
                    }
                    break;

                case 35:
                    if (pollutionAverage > /* 80 */ 60)
                    {
                        sendMessage((short) MessageNumber.MESSAGE_HIGH_POLLUTION, -1, -1, true);
                    }
                    break;

                case 42:
                    if (crimeAverage > 100)
                    {
                        sendMessage((short)MessageNumber.MESSAGE_HIGH_CRIME, -1, -1, true);
                    }
                    break;

                case 45:
                    if (totalPop > 60 && fireStationPop == 0)
                    {
                        sendMessage((short)MessageNumber.MESSAGE_NEED_FIRE_STATION);
                    }
                    break;

                case 48:
                    if (totalPop > 60 && policeStationPop == 0)
                    {
                        sendMessage((short)MessageNumber.MESSAGE_NEED_POLICE_STATION);
                    }
                    break;

                case 51:
                    if (cityTax > 12)
                    {
                        sendMessage((short)MessageNumber.MESSAGE_TAX_TOO_HIGH);
                    }
                    break;

                case 54:
                    // If roadEffect < 5/8 of max effect
                    if (roadEffect < (5 * MAX_ROAD_EFFECT / 8) && roadTotal > 30)
                    {
                        sendMessage((short)MessageNumber.MESSAGE_ROAD_NEEDS_FUNDING);
                    }
                    break;

                case 57:
                    // If fireEffect < 0.7 of max effect
                    if (fireEffect < 7 * MAX_FIRE_STATION_EFFECT / 10 && totalPop > 20)
                    {
                        sendMessage((short)MessageNumber.MESSAGE_FIRE_STATION_NEEDS_FUNDING);
                    }
                    break;

                case 60:
                    // If policeEffect < 0.7 of max effect
                    if (policeEffect < 7 * MAX_POLICE_STATION_EFFECT / 10)
                    {
                        sendMessage((short)MessageNumber.MESSAGE_POLICE_NEEDS_FUNDING);
                    }
                    break;

                case 63:
                    if (trafficAverage > 60)
                    {
                        sendMessage((short)MessageNumber.MESSAGE_TRAFFIC_JAMS, -1, -1, true);
                    }
                    break;
            }
        }

        /// <summary>
        /// Detect a change in city class, and produce a message if the player has
        /// reached the next class.
        /// TODO This code is very closely related to Micropolis::doPopNum().
        ///      Maybe merge both in some way?
        ///      (This function gets called much more often however then doPopNum().
        ///      Also, at the first call, the difference between thisCityPop and
        ///      cityPop is huge.)
        /// </summary>
        public void checkGrowth()
        {
            if ((cityTime & 3) == 0)
            {
                short category = 0;
                long thisCityPop = getPopulation();

                if (cityPopLast > 0)
                {
                    CityClass lastClass = getCityClass(cityPopLast);
                    CityClass newClass = getCityClass(thisCityPop);

                    if (lastClass != newClass)
                    {
                        // Switched class, find appropiate message.
                        switch (newClass)
                        {
                            case CityClass.CC_VILLAGE:
                                // Don't mention it.
                                break;

                            case CityClass.CC_TOWN:
                                category = (short) MessageNumber.MESSAGE_REACHED_TOWN;
                                break;

                            case CityClass.CC_CITY:
                                category = (short)MessageNumber.MESSAGE_REACHED_CITY;
                                break;

                            case CityClass.CC_CAPITAL:
                                category = (short)MessageNumber.MESSAGE_REACHED_CAPITAL;
                                break;

                            case CityClass.CC_METROPOLIS:
                                category = (short)MessageNumber.MESSAGE_REACHED_METROPOLIS;
                                break;

                            case CityClass.CC_MEGALOPOLIS:
                                category = (short)MessageNumber.MESSAGE_REACHED_MEGALOPOLIS;
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }

                if (category > 0 && category != categoryLast)
                {
                    sendMessage(category, NOWHERE, NOWHERE, true);
                    categoryLast = category;
                }

                cityPopLast = thisCityPop;
            }
        }

        /// <summary>
        /// Compute score for each scenario
        /// </summary>
        /// <param name="scenarioType">Scenario used</param>
        private void doScenarioScore(ScenarioType type)
        {
            short z = (short) MessageNumber.MESSAGE_SCENARIO_LOST; // you lose

            switch (type)
            {
                case ScenarioType.SC_DULLSVILLE:
                    if (cityClass >= CityClass.CC_METROPOLIS)
                    {
                        z = (short) MessageNumber.MESSAGE_SCENARIO_WON;
                    }
                    break;

                case ScenarioType.SC_SAN_FRANCISCO:
                    if (cityClass >= CityClass.CC_METROPOLIS)
                    {
                        z = (short)MessageNumber.MESSAGE_SCENARIO_WON;
                    }
                    break;

                case ScenarioType.SC_HAMBURG:
                    if (cityClass >= CityClass.CC_METROPOLIS)
                    {
                        z = (short)MessageNumber.MESSAGE_SCENARIO_WON;
                    }
                    break;

                case ScenarioType.SC_BERN:
                    if (trafficAverage < 80)
                    {
                        z = (short)MessageNumber.MESSAGE_SCENARIO_WON;
                    }
                    break;

                case ScenarioType.SC_TOKYO:
                    if (cityScore > 500)
                    {
                        z = (short)MessageNumber.MESSAGE_SCENARIO_WON;
                    }
                    break;

                case ScenarioType.SC_DETROIT:
                    if (crimeAverage < 60)
                    {
                        z = (short)MessageNumber.MESSAGE_SCENARIO_WON;
                    }
                    break;

                case ScenarioType.SC_BOSTON:
                    if (cityScore > 500)
                    {
                        z = (short)MessageNumber.MESSAGE_SCENARIO_WON;
                    }
                    break;

                case ScenarioType.SC_RIO:
                    if (cityScore > 500)
                    {
                        z = (short)MessageNumber.MESSAGE_SCENARIO_WON;
                    }
                    break;

                default:
                    break;
            }

            sendMessage(z, NOWHERE, NOWHERE, true, true);

            if (z == (short) MessageNumber.MESSAGE_SCENARIO_LOST)
            {
                doLoseGame();
            }
        }

        /// <summary>
        /// Send the user a message of an event that happens at a particular position
        /// in the city.
        /// </summary>
        /// <param name="mesgNum">Message number of the message to display.</param>
        /// <param name="x">X coordinate of the position of the event.</param>
        /// <param name="y">Y coordinate of the position of the event.</param>
        /// <param name="picture">Flag that is true if a picture should be shown.</param>
        /// <param name="important">Flag that is true if the message is important.</param>
        public void sendMessage(short mesgNum, short x = NOWHERE, short y = NOWHERE, bool picture = false, bool important = false)
        {
            if (OnSendMessage != null)
            {
                OnSendMessage(mesgNum, x, y, picture, important);
            }
        }

        /// <summary>
        /// Make a sound for message mesgNum if appropriate.
        /// </summary>
        /// <param name="mesgNum">Message number displayed.</param>
        /// <param name="x">Horizontal coordinate in the city of the sound.</param>
        /// <param name="y">Vertical coordinate in the city of the sound.</param>
        public void doMakeSound(int mesgNum, int x, int y)
        {
            // TODO
        }

        /// <summary>
        /// Tell the front-end that it should perform an auto-goto
        /// </summary>
        /// <param name="x">X position at the map</param>
        /// <param name="y">Y position at the map</param>
        /// <param name="msg">Message</param>
        public void doAutoGoto(short x, short y, string msg)
        {
            callback("autoGoto", "dd", x.ToString(), y.ToString());
        }

        /// <summary>
        /// Tell the front-end that the player has lost the game
        /// </summary>
        public void doLoseGame()
        {
            callback("loseGame", "");
        }

        /// <summary>
        /// Tell the front-end that the player has won the game
        /// </summary>
        public void doWinGame()
        {
            callback("winGame", "");
        }
    }
}
