using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wellbeing;

public static class Updater
{
    private const string TagsUrl = "https://api.github.com/repos/AspireOne/desktop-time-limiter/tags";
    private const string ExeUrl = "https://github.com/AspireOne/desktop-time-limiter/releases/latest/download/Wellbeing.exe";
    private static readonly string DownloadedExePath = Path.Combine(Program.RootDirectory, "DownloadCache", Program.ExeName);
        
    private static readonly HttpClient Client = new() { DefaultRequestHeaders = { UserAgent = { ProductInfoHeaderValue.Parse("Other") } } };
    private static readonly Regex VersionRegex = new(@"(?<=""v)\d\.\d\.\d(?="")", RegexOptions.ECMAScript);
    
    public static async Task<bool> IsUpdateAvailable()
    {
        string[] availableVersions = await FetchAvailableVersions();

        Logger.Log("Available versions: ", false);
        Array.ForEach(availableVersions, x => Logger.Log(x, false));

        string highestVer = GetHighestVersion(availableVersions);
        Logger.Log("highest version: " + highestVer);

        bool highestIsHigherThanCurr = IsVersionHigher(highestVer, Program.Version);
        Logger.Log("Highest available is higher than current: " + highestIsHigherThanCurr);

        return highestIsHigherThanCurr;
    }

    public static void DownloadLatestUpdateAsync(Action<Action> downloadedHandler)
    {
        if (File.Exists(DownloadedExePath))
            File.Delete(DownloadedExePath);

        string directoryPath = Path.GetDirectoryName(DownloadedExePath)!;
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
        
        using WebClient wc = new();
        wc.DownloadFileCompleted += (_, _) => downloadedHandler(Update);

        try
        {
            Logger.Log("Downloading update");
            wc.DownloadFileAsync(new Uri(ExeUrl), DownloadedExePath);
        }
        catch (WebException e)
        {
            Logger.Log("A handled error occured while trying to download new update binary.");
            Logger.Log(e);
        }
    }

    /// <summary>
    /// Will execute the update and close this program. Does not return.
    /// </summary>
    private static void Update()
    {
        Process.Start(DownloadedExePath);
        Application.Exit();
    }
    
    private static async Task<string[]> FetchAvailableVersions()
    {
        using HttpResponseMessage response = await Client.GetAsync(TagsUrl);
        using HttpContent content = response.Content;
        string result = await content.ReadAsStringAsync();
            
        MatchCollection matches = VersionRegex.Matches(result);
        string[] matchedVersions = new string[matches.Count];

        for (int i = 0; i < matches.Count; ++i)
            matchedVersions[i] = matches[i].Value;

        return matchedVersions;
    }

    private static string GetHighestVersion(IReadOnlyList<string> versions)
        => versions.Aggregate(versions[0], (current, ver) => IsVersionHigher(ver, current) ? ver : current);
    private static bool IsVersionHigher(string version, string other)
        => int.Parse(version.Replace(".", "")) > int.Parse(other.Replace(".", ""));
}