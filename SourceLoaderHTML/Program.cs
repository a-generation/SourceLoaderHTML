using HtmlAgilityPack;

class Program
{
    static async Task Main(string[] args)
    {
        // Ensure the user provided correct arguments
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: .exe \"url\" \"output folder\" \"ignored extensions\"");
            Console.WriteLine("Example: .exe \"https://example.com\" \"resources\" \"css js woff\"");
            return;
        }

        // Command line arguments
        string url = args[0];  // Website URL
        string folder = args[1];  // Folder to save resources
        string[] ignoredExtensions = args[2].Split(' ');  // Ignored extensions (space-separated)

        // Create the folder if it doesn't exist
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        // Load the HTML page
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Failed to retrieve the page: {response.StatusCode}");
            return;
        }

        string pageContent = await response.Content.ReadAsStringAsync();

        // Parse HTML using HtmlAgilityPack
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(pageContent);

        // Find all resources (scripts, styles, images)
        var resources = doc.DocumentNode.Descendants()
            .Where(node => node.Name == "script" && node.Attributes["src"] != null ||
                           node.Name == "link" && node.Attributes["href"] != null ||
                           node.Name == "img" && node.Attributes["src"] != null)
            .Select(node =>
            {
                if (node.Name == "script")
                    return node.Attributes["src"].Value;
                if (node.Name == "link")
                    return node.Attributes["href"].Value;
                if (node.Name == "img")
                    return node.Attributes["src"].Value;
                return null;
            })
            .Distinct()
            .ToList();

        // Filter out resources based on ignored extensions
        var filteredResources = resources
            .Where(resource => !ignoredExtensions.Any(ext => resource.EndsWith($".{ext}", StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Console.WriteLine($"Found {filteredResources.Count} resources to download (after filtering).");

        // Download each resource
        int counter = 1;
        foreach (var resource in filteredResources)
        {
            string resourceUrl = new Uri(new Uri(url), resource).AbsoluteUri;

            // Remove any URL query parameters for safe file naming
            string cleanFileName = Path.GetFileName(new Uri(resourceUrl).LocalPath);
            string fileName = Path.Combine(folder, cleanFileName);

            Console.WriteLine($"({counter}/{filteredResources.Count}) Downloading resource: {resourceUrl}");

            try
            {
                byte[] resourceData = await client.GetByteArrayAsync(resourceUrl);
                await File.WriteAllBytesAsync(fileName, resourceData);
                Console.WriteLine($"({counter}/{filteredResources.Count}) File {cleanFileName} successfully downloaded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading {resourceUrl}: {ex.Message}");
            }

            counter++;
        }

        Console.WriteLine("Download complete.");
    }
}
