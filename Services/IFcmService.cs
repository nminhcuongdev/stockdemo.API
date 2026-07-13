namespace StockDemo.API.Services
{
    public interface IFcmService
    {
        bool IsEnabled { get; }

        /// <summary>
        /// Sends a notification to the given device tokens.
        /// Returns the tokens that are no longer valid and should be pruned.
        /// </summary>
        Task<List<string>> SendAsync(
            IReadOnlyList<string> tokens,
            string title,
            string body,
            IReadOnlyDictionary<string, string>? data = null);
    }
}
