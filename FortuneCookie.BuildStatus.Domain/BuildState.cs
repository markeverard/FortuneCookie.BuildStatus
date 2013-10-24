namespace FortuneCookie.BuildStatus.Domain
{
    /// <summary>
    /// Available Build states
    /// </summary>
    public enum BuildState
    {
        NotConnected = 0,
        Good = 1,
        Broken = 2,
        Building = 3,
        Exception = 4
    }
}