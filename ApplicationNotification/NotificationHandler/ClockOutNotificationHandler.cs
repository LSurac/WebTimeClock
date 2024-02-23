using System.Globalization;
using ApplicationNotification.Configurations;
using ApplicationNotification.Notification;
using ApplicationNotification.Services;

namespace ApplicationNotification.NotificationHandler
{
    public class ClockOutNotificationHandler(NotificationSettings notificationSettings, EmailService emailService)
    {
        public async Task Handle(
            ClockOutNotification notification,
            CancellationToken cancellationToken)
        {
            var mailBody = notificationSettings.NotificationMailBody;

            if (mailBody.Contains("[EMPLOYEE_FULLNAME]"))
            {
                mailBody = mailBody.Replace("[EMPLOYEE_FULLNAME]",
                    $"{notification.Employee.ForName} {notification.Employee.LastName}");
            }

            if (mailBody.Contains("[TIMECLOCK_TIME]"))
            {
                var cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Time");
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(notification.TimeClock.UtcTime, cetTimeZone);

                mailBody = mailBody.Replace("[TIMECLOCK_TIME]", localTime.ToString(CultureInfo.CurrentCulture));
            }

            await emailService.SendEmail(mailBody); ;
        }
    }
}