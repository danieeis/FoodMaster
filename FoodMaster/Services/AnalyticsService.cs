﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using FoodMaster.Models;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace FoodMaster
{
    public class AnalyticsService : IAnalyticsService
    {
        const string _androidKey = "97bf580b-b7d2-4c85-91b4-5fb976a4fd4a;";

        public AnalyticsService() => AppCenter.Start(ApiKey, typeof(Analytics), typeof(Crashes));

        string ApiKey = _androidKey;

        public void Track(string trackIdentifier, IDictionary<string, string>? table = null) =>
            Analytics.TrackEvent(trackIdentifier, table);

        public void Track(string trackIdentifier, string key, string value) =>
            Analytics.TrackEvent(trackIdentifier, new Dictionary<string, string> { { key, value } });

        public ITimedEvent TrackTime(string trackIdentifier, IDictionary<string, string>? table = null) =>
            new TimedEvent(this, trackIdentifier, table);

        public ITimedEvent TrackTime(string trackIdentifier, string key, string value) =>
            TrackTime(trackIdentifier, new Dictionary<string, string> { { key, value } });

        public void Report(Exception exception,
                                  string key,
                                  string value,
                                  [CallerMemberName] string callerMemberName = "",
                                  [CallerLineNumber] int lineNumber = 0,
                                  [CallerFilePath] string filePath = "")
        {
            Report(exception, new Dictionary<string, string> { { key, value } }, callerMemberName, lineNumber, filePath);
        }

        public void Report(Exception exception,
                                  IDictionary<string, string>? properties = null,
                                  [CallerMemberName] string callerMemberName = "",
                                  [CallerLineNumber] int lineNumber = 0,
                                  [CallerFilePath] string filePath = "")
        {
            PrintException(exception, callerMemberName, lineNumber, filePath, properties);

            Crashes.TrackError(exception, properties);
        }

        [Conditional("DEBUG")]
        void PrintException(Exception exception, string callerMemberName, int lineNumber, string filePath, IDictionary<string, string>? properties = null)
        {
            var fileName = System.IO.Path.GetFileName(filePath);

            Debug.WriteLine(exception.GetType());
            Debug.WriteLine($"Error: {exception.Message}");
            Debug.WriteLine($"Line Number: {lineNumber}");
            Debug.WriteLine($"Caller Name: {callerMemberName}");
            Debug.WriteLine($"File Name: {fileName}");

            if (properties != null)
            {
                foreach (var property in properties)
                    Debug.WriteLine($"{property.Key}: {property.Value}");
            }

            Debug.WriteLine(exception);
        }
    }
}
