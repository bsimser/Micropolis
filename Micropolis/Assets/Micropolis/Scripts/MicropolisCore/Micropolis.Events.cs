namespace MicropolisCore
{
    public partial class Micropolis
    {
        public event UpdateDateHandler OnUpdateDate;

        public event UpdateFundsHandler OnUpdateFunds;

        public event UpdateCityName OnUpdateCityName;

        public event UpdateEvaluation OnUpdateEvaluation;

        public event SendMessage OnSendMessage;
    }
}