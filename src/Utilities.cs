namespace InfiniteWeapons
{
    public partial class InfiniteWeapons
    {
        private void DebugPrint(string message)
        {
            if (Config.Debug)
            {
                Console.WriteLine(Localizer["core.debugprint"].Value.Replace("{message}", message));
            }
        }
    }
}