﻿using Microsoft.WindowsAzure.Storage;

namespace Lykke.SettingsReader.Checkers
{
    internal class AzureTableChecker : ISettingsFieldChecker
    {
        private readonly bool _throwExceptionOnFail;

        internal AzureTableChecker(bool throwExceptionOnFail)
        {
            _throwExceptionOnFail = throwExceptionOnFail;
        }

        public CheckFieldResult CheckField(object model, string propertyName, string value)
        {
            string url = string.Empty;
            try
            {
                var account = CloudStorageAccount.Parse(value);
                var client = account.CreateCloudTableClient();
                url = client.BaseUri.ToString();
                client.ListTablesSegmentedAsync(null).GetAwaiter().GetResult();
                return CheckFieldResult.Ok(propertyName, url);
            }
            catch
            {
                return CheckFieldResult.Failed(propertyName, url, _throwExceptionOnFail);
            }
        }
    }
}
