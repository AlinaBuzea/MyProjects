﻿using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Models
{
    internal static class NotificationClass
    {
        public static NotificationRequest ShowSavedProductNotification(string returningData)
        {
            return new NotificationRequest
            {
                NotificationId = 101,
                Title = "Product Saved",
                Description = "The product had been saved",
                ReturningData = returningData,
                Schedule =
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };
        }

        public static NotificationRequest ShowBudgetExceededNotification(string categoryName)
        {
            return new NotificationRequest
            {
                NotificationId = 102,
                Title = "Budget Depasit!",
                Description = "Bugetul stabilit pentru categoria " + categoryName + " a fost depasit!",
                ReturningData = categoryName,
                Schedule =
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };
        }

        public static NotificationRequest ShowLimitBudgetExceededNotification(string categoryName)
        {
            return new NotificationRequest
            {
                NotificationId = 103,
                Title = "Budget ajuns la limita",
                Description = "Bugetul stabilit pentru categoria " + categoryName + " a ajuns la limita!",
                ReturningData = categoryName,
                Schedule =
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };
        }
    }
}
