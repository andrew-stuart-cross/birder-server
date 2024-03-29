﻿namespace Birder.Helpers
{
    public class LoggingEvents
    {
        public const int GenerateItems = 1000;
        public const int ListItems = 1001;
        public const int GetItem = 1002;
        public const int InsertItem = 1003;
        public const int UpdateItem = 1004;
        public const int DeleteItem = 1005;

        public const int InvalidModelState = 4010;

        public const int GetItemNotFound = 4000;
        public const int GetListNotFound = 4001;
        public const int UpdateItemNotFound = 4002;


        public const int InvalidOrMissingArgument = 7000;

        public const int SqlServerException = 8000;
        public const int SqlServerConnectionTimeoutException = 8001;

        public const int Exception = 9000;
    }
}
