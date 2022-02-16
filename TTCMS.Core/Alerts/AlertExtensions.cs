﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTCMS.Core.Infrastructure.Alerts
{
    public static class AlertExtensions
    {
        private const string Alerts = "_Alerts";

        public static List<Alert> GetAlerts(this TempDataDictionary tempData)
        {
            if (!tempData.ContainsKey(Alerts))
            {
                tempData[Alerts] = new List<Alert>();
            }
            return tempData[Alerts] as List<Alert>;
        }

        public static ActionResult WithSuccess(this ActionResult result, string message)
        {
            return new AlertDecoratorResult(result, "alert-success", message);
        }

        public static ActionResult WithInfo(this ActionResult result, string message)
        {
            return new AlertDecoratorResult(result, "alert-info", message);
        }

        public static ActionResult WithWarning(this ActionResult result, string message)
        {
            return new AlertDecoratorResult(result, "alert-warning", message);
        }

        public static ActionResult WithError(this ActionResult result, string message)
        {
            return new AlertDecoratorResult(result, "alert-danger", message);
        }
    }
}