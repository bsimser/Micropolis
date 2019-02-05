using System;
using System.Text;
using MicropolisCore;
using MicropolisEngine;
using UnityEngine;

namespace MicropolisGame
{
    /// <summary>
    /// The CallbackManager is responsible for gluing the Micropolis
    /// engine to the Unity UI elements and updating them during runtime
    /// through events invoked from inside the engine.
    /// </summary>
    public class CallbackManager
    {
        private readonly MicropolisUnityEngine _engine;

        public CallbackManager(MicropolisUnityEngine engine)
        {
            _engine = engine;
            _engine.OnUpdateDate += UpdateDate;
            _engine.OnUpdateFunds += UpdateFunds;
            _engine.OnUpdateCityName += UpdateCityName;
            _engine.OnUpdateEvaluation += UpdateEvaluation;
            _engine.OnSendMessage += SendMessage;
        }

        private void UpdateFunds()
        {
            GuiWindowManager.Instance.GetWindow(EnumGuiWindow.Funds)
                .gameObject.GetComponent<GuiWindowFunds>()
                .SetFunds(_engine.totalFunds);
        }

        private void UpdateDate()
        {
            GuiWindowManager.Instance.GetWindow(EnumGuiWindow.Date)
                .gameObject.GetComponent<GuiWindowDate>()
                .SetDate(_engine.cityYear, _engine.cityMonth);
            // temporary until we find the message that triggers population update
            GuiWindowManager.Instance.GetWindow(EnumGuiWindow.Population)
                .gameObject.GetComponent<GuiWindowPopulation>()
                .SetPopulation(_engine.cityPop);
        }

        private void UpdateCityName()
        {
            Debug.Log(_engine.cityName);
        }

        private void SendMessage(short mesgNum, short s, short s1, bool picture, bool important)
        {
            //Debug.Log(string.Format("mesgNum: {0} x: {1} y: {2} picture: {3} important: {4}", mesgNum, s, s1, picture, important));
            //TODO handle autoGoto on important messages
            //TODO if there's a picture to show show it (how do we know what picture?)
            var message = _engine.messages[mesgNum];
            GuiWindowManager.Instance.GetWindow(EnumGuiWindow.Messages)
                .gameObject.GetComponent<GuiWindowMessage>()
                .SetMessage(message);
        }

        private void UpdateEvaluation()
        {
            // TODO probably have this show a panel with this information
            var sb = new StringBuilder();

            sb.AppendFormat("City Evaluation {0}" + Environment.NewLine, _engine.currentYear());
            sb.AppendLine("Public Opinion");
            sb.AppendLine("  Is the mayor doing a good job?");
            sb.AppendFormat("    Yes: {0}" + Environment.NewLine, _engine.cityYes);
            sb.AppendFormat("    No: {0}" + Environment.NewLine, 100 - _engine.cityYes);
            sb.AppendLine("  What are the worst problems?");
            for (int i = 0; i < (int)CityVotingProblems.CVP_PROBLEM_COMPLAINTS; i++)
            {
                // this is only for debugging but eventually we need this data 
                // and should not be getting it directly from the arrays
                sb.AppendFormat("{0}:{1}" + Environment.NewLine,
                    _engine.problemOrder[i],
                    _engine.problemVotes[_engine.problemOrder[i]]);
            }
            sb.AppendLine("Statistics");
            sb.AppendFormat("  Population: {0}" + Environment.NewLine, _engine.cityPop);
            sb.AppendFormat("  Net Migration: {0} (last year)" + Environment.NewLine, _engine.cityPopDelta);
            sb.AppendFormat("  Assessed Value: {0}" + Environment.NewLine, _engine.cityAssessedValue);
            sb.AppendFormat("  Category: {0}" + Environment.NewLine, _engine.cityClass);
            sb.AppendFormat("  Game Level: {0}" + Environment.NewLine, _engine.gameLevel);

            Debug.Log(sb.ToString());
        }
    }
}