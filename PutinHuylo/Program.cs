using System.IO.Compression;
using System.Text.RegularExpressions;

Console.Write("Enter folder:");
string dirPath = Console.ReadLine();
int choice;

while (true)
{
    Console.WriteLine("\n1 - Delte useless files");
    Console.WriteLine("2 - Delte empty folders");
    Console.WriteLine("3 - Change folder");
    Console.WriteLine("4 - Exit");
    Console.WriteLine("5 - Delete expired files\n");
    Console.Write("Choose an option: ");
    choice = Int32.Parse(Console.ReadLine());

    switch (choice)
    {
        case 1:
            if (Directory.Exists(dirPath))
            {
                int count = DelteUselessFiles(dirPath);
                Console.WriteLine($"\nDeleted {count} files");
            }
            break;
        case 2:
            if (Directory.Exists(dirPath))
            {
                int count = DelteEmptyFolders(dirPath);
                Console.WriteLine($"\nDeleted {count} folders");
            }
            break;
        case 3:
            Console.Write("\nEnter folder:");
            dirPath = Console.ReadLine();
            break;
        case 4:
            return;
        case 5:
            if (Directory.Exists(dirPath))
            {
                int count = DelteExpiredFiles(dirPath);
                Console.WriteLine($"\nDeleted {count} files");
            }
            break;
        default:
            break;
    }

    //var fileNames = Directory.GetFiles(dirPath).Where(f => f.EndsWith(".zip")).ToList();
    //int count = 1;

    //foreach (var fiNamele in fileNames)
    //{
    //    while (true)
    //    {
    //        if (!Directory.Exists(count.ToString()))
    //        {
    //            break;
    //        }
    //        count++;
    //    }

    //    var destinationDir = Path.Combine(dirPath, count.ToString());
    //    Directory.CreateDirectory(destinationDir);

    //    Console.WriteLine($"Extracting {fiNamele}");
    //    ZipFile.ExtractToDirectory(fiNamele, destinationDir);
    //}
}

int DelteUselessFiles(string dirrectoy)
{
    int count = 0;
    var subDirectoies = Directory.GetDirectories(dirrectoy);
    foreach (var subDirectory in subDirectoies)
    {
        count += DelteUselessFiles(subDirectory);
    }

    var files = Directory.GetFiles(dirrectoy);

    string pattern = @"^(az|odds|ofp|oisk|opu|osd)";
    Regex regex = new Regex(pattern);

    foreach (var file in files)
    {
        MatchCollection matchCollection = regex.Matches(Path.GetFileName(file).ToLower());

        if (matchCollection.Count > 0)
        {
            count++;
            File.Delete(file);
        }
    }

    return count;
}

int DelteEmptyFolders(string dirrectoy)
{
    int count = 0;
    var subDirectoies = Directory.GetDirectories(dirrectoy);

    foreach (var subDirectory in subDirectoies)
    {
        count += DelteEmptyFolders(subDirectory);
    }

    if (Directory.GetFiles(dirrectoy).Length == 0 && Directory.GetDirectories(dirrectoy).Length == 0)
    {
        count++;
        Directory.Delete(dirrectoy, true);
    }

    return count;
}

int DelteExpiredFiles(string dirrectoy)
{
    int count = 0;
    var subDirectoies = Directory.GetDirectories(dirrectoy);
    foreach (var subDirectory in subDirectoies)
    {
        count += DelteExpiredFiles(subDirectory);
    }

    var files = Directory.GetFiles(dirrectoy);

    string pattern = @"[^\d](\d\d)(\d\d)(\d\d\d\d)[^\d]";
    Regex regex = new Regex(pattern);

    foreach (var file in files)
    {
        MatchCollection matchCollection = regex.Matches(Path.GetFileName(file).ToLower());

        if (matchCollection.Count > 0 &&
            Int32.Parse(matchCollection[0].Groups[3].Value) < 2015 &&
            Int32.Parse(matchCollection[0].Groups[2].Value) == 1 &&
            Int32.Parse(matchCollection[0].Groups[1].Value) == 1)
        {
            count++;
            File.Delete(file);
        }
    }

    pattern = @"[^\d](\d\d)\.(\d\d)\.(\d\d)[^\d]";
    regex = new Regex(pattern);

    foreach (var file in files)
    {
        var t = Path.GetFileName(file).ToLower();
        MatchCollection matchCollection = regex.Matches(Path.GetFileName(file).ToLower());

        if (matchCollection.Count > 0 &&
            Int32.Parse(matchCollection[0].Groups[3].Value) < 15 &&
            Int32.Parse(matchCollection[0].Groups[2].Value) == 1 &&
            Int32.Parse(matchCollection[0].Groups[1].Value) == 1)
        {
            count++;
            File.Delete(file);
        }
    }

    return count;
}