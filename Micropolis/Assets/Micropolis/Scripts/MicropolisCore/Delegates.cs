namespace MicropolisCore
{
    public delegate void UpdateDateHandler();

    public delegate void UpdateFundsHandler();

    public delegate void UpdateCityName();

    public delegate void SendMessage(short mesgNum, short x, short y, bool picture, bool important);
}