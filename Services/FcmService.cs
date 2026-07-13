using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace StockDemo.API.Services
{
    /// <summary>
    /// Firebase Cloud Messaging sender. Degrades gracefully: if no service-account
    /// credentials are configured, it is disabled and sending becomes a no-op, so the
    /// API still builds and runs without Firebase set up.
    /// </summary>
    public class FcmService : IFcmService
    {
        private readonly ILogger<FcmService> logger;
        private static readonly object InitLock = new();

        public bool IsEnabled { get; }

        public FcmService(IConfiguration configuration, ILogger<FcmService> logger)
        {
            this.logger = logger;

            // Credentials path from config "Fcm:CredentialsPath" (if non-empty) or GOOGLE_APPLICATION_CREDENTIALS.
            var credentialsPath = configuration["Fcm:CredentialsPath"];
            if (string.IsNullOrWhiteSpace(credentialsPath))
            {
                credentialsPath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
            }

            if (string.IsNullOrWhiteSpace(credentialsPath) || !File.Exists(credentialsPath))
            {
                logger.LogWarning(
                    "FCM disabled: service-account credentials not found (Fcm:CredentialsPath / GOOGLE_APPLICATION_CREDENTIALS). Low-stock push notifications will not be sent.");
                IsEnabled = false;
                return;
            }

            try
            {
                lock (InitLock)
                {
                    if (FirebaseApp.DefaultInstance == null)
                    {
                        FirebaseApp.Create(new AppOptions
                        {
                            Credential = GoogleCredential.FromFile(credentialsPath)
                        });
                    }
                }
                IsEnabled = true;
                logger.LogInformation("FCM initialized from {Path}", credentialsPath);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to initialize FCM; push notifications disabled.");
                IsEnabled = false;
            }
        }

        public async Task<List<string>> SendAsync(
            IReadOnlyList<string> tokens,
            string title,
            string body,
            IReadOnlyDictionary<string, string>? data = null)
        {
            var invalidTokens = new List<string>();
            if (!IsEnabled || tokens.Count == 0)
            {
                return invalidTokens;
            }

            var message = new MulticastMessage
            {
                Tokens = tokens.ToList(),
                Notification = new Notification { Title = title, Body = body },
                Data = data?.ToDictionary(kv => kv.Key, kv => kv.Value)
            };

            try
            {
                var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
                for (int i = 0; i < response.Responses.Count; i++)
                {
                    var r = response.Responses[i];
                    if (!r.IsSuccess && r.Exception is FirebaseMessagingException fme &&
                        (fme.MessagingErrorCode == MessagingErrorCode.Unregistered ||
                         fme.MessagingErrorCode == MessagingErrorCode.InvalidArgument))
                    {
                        invalidTokens.Add(tokens[i]);
                    }
                }
                logger.LogInformation("FCM sent: {Success} ok, {Failure} failed",
                    response.SuccessCount, response.FailureCount);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "FCM send failed");
            }

            return invalidTokens;
        }
    }
}
