using Cloudmersive.APIClient.NETCore.VirusScan.Api;
using Cloudmersive.APIClient.NETCore.VirusScan.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace MvcApp.Common
{
    public interface IVirusScanner
    {
        void Scan(Stream file);
        Task ScanAsync(Stream file);
    }

    public class VirusScanner : IVirusScanner
    {
        #region Constructor

        public VirusScanner(string apiKey)
        {
            this.ScanApi = new Lazy<ScanApi>(() =>
            {
                return new ScanApi(new Configuration()
                {
                    ApiKey = new Dictionary<string, string>() { { CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nameof(Configuration.ApiKey)), apiKey } }
                });
            });
        }

        #endregion

        #region Properties

        public Lazy<ScanApi> ScanApi { get; private set; }

        #endregion

        public static IVirusScanner Default(string apiKey) => new VirusScanner(apiKey);

        public void Scan(Stream file)
        {
            var result = this.ScanApi.Value.ScanFile(file);

            // parse the result
            if (result.CleanResult != true)
                throw new Exception("File is infected.");

        }

        public async Task ScanAsync(Stream file)
        {
            var result = await this.ScanApi.Value.ScanFileAsync(file);

            // parse the result
            if (result.CleanResult != true)
                throw new Exception("File is infected.");
        }
    }
}
