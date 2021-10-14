﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FoodMaster
{
    public interface IAnalyticsService
    {
        void Track(string trackIdentifier, IDictionary<string, string>? table = null);

        void Track(string trackIdentifier, string key, string value);

        ITimedEvent TrackTime(string trackIdentifier, IDictionary<string, string>? table = null);

        ITimedEvent TrackTime(string trackIdentifier, string key, string value);

        void Report(Exception exception,
                           string key,
                           string value,
                           [CallerMemberName] string callerMemberName = "",
                           [CallerLineNumber] int lineNumber = 0,
                           [CallerFilePath] string filePath = "");

        void Report(Exception exception,
                                  IDictionary<string, string>? properties = null,
                                  [CallerMemberName] string callerMemberName = "",
                                  [CallerLineNumber] int lineNumber = 0,
                                  [CallerFilePath] string filePath = "");
    }

    public interface ITimedEvent : IDisposable
    {
        IDictionary<string, string> Data { get; }
    }
}