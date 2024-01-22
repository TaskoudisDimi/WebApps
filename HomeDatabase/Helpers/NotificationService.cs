using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;

namespace HomeDatabase.Helpers
{
    public class NotificationService
    {

        private readonly IConfiguration _configuration;

        public NotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromAccessToken("AAAAurj2oio:APA91bGG0DirGN0XTvJKGB17i5usBaeQZzVKem9Ig-Nv0_R7MA7kYAu3Rw7q6Xhlu4-XzK-C3xffQ150t7nljqOpAYiF3eFrvf_12waB06O2Qu67imX0WHTG9YCTDEawsJWLvQzKhmBG")
            });
        }

        //Send Notification to Android
        public async Task SendNotificationAsync(string deviceToken, string title, string body)
        {
            var message = new Message()
            {
                Token = deviceToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body,
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                }
            };

            var messaging = FirebaseMessaging.DefaultInstance;
            await messaging.SendAsync(message);
        }

        //Send Notification to iOS

        //Send notification to browser
        public async Task SendBroewserNotififactionAsync(string userName, string title, string body)
        {

            string connectionId = GetUserConnectionId(userName);

            if (!string.IsNullOrEmpty(connectionId))
            {
                //await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", title, body);
            }
        }

        private string GetUserConnectionId(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
