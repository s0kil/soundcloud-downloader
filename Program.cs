using SoundCloudExplode;

internal static class Program
{
    public static async Task Main()
    {
        var soundCloud = new SoundCloudClient();

        while (true)
        {
            Console.WriteLine("Press Ctrl+C To Exit");
            Console.WriteLine("Enter Soundcloud Tracks URL: ");
            var tracksUrl = Console.ReadLine() ?? "";
            if (Uri.IsWellFormedUriString(tracksUrl, UriKind.Absolute))
            {
                var tracks = soundCloud.Tracks.GetTracksAsync(tracksUrl);
                await foreach (var track in tracks)
                {
                    var trackAuthor = track.User.Username;
                    var trackName = string.Join("_", track.Title.Split(Path.GetInvalidFileNameChars()));
                    var trackFullName = $"{trackAuthor}: {trackName}";
                    var trackPath = Path.Join(Environment.CurrentDirectory, "Downloads", trackAuthor, $"{trackAuthor}: {trackName}.mp3");

                    if (File.Exists(trackPath))
                    {
                        Console.WriteLine($"Skipping {trackFullName}, Already Downloaded");
                        continue;
                    }

                    try
                    {
                        Console.WriteLine($"Downloading {trackFullName}");
                        await soundCloud.DownloadAsync(track, trackPath);
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine($"Failed To Download {trackFullName} Due To An Error, Maybe This Track Requires Soundcloud GO+ Or Internet Issue");
                        Console.WriteLine(error.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Bad Url");
            }
        }
    }
}
