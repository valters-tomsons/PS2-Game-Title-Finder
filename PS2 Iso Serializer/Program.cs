using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenZipExtractor;


namespace PS2_Iso_Serializer
{
    class Program
    {
        //Variables
        public static string gameindex;
        public static string gameloc;
        public static string gameserial;
        public static string filename;

        static void Main(string[] args)
        {

            List<string> regionList = new List<string>();

            //Adds items to region list
            regionList.Add("SLUS");
            regionList.Add("SCUS");
            regionList.Add("SCES");
            regionList.Add("SLES");
            regionList.Add("SCPS");
            regionList.Add("SLPS");
            regionList.Add("SLPM");




            //Initilization
            Console.WriteLine("Search a PS2 image file for game name");
            Console.WriteLine("Uses 7zipSharp and PCSX2 game library");
            Console.WriteLine(@"Written by https://github.com/FaithLV");

        GameIndex:
            Console.WriteLine("");
            Console.WriteLine("Enter location of GameIndex.dbf");
            gameindex = Console.ReadLine();

            if(File.Exists(gameindex) == false)
            {
                Console.WriteLine("File does not exist");
                goto GameIndex;
            }

        GameFile:
            Console.WriteLine("");
            Console.WriteLine("Enter location of chosen .iso/.gz file");
            gameloc = Console.ReadLine();

            if (File.Exists(gameloc) == false)
            {
                Console.WriteLine("File does not exist");
                goto GameFile;
            }

            Console.WriteLine("");

            //Looks for all files in the arhive
            using (ArchiveFile archiveFile = new ArchiveFile(gameloc))
            {
                foreach (Entry entry in archiveFile.Entries)
                {
                    //If any file in archive starts with a regional number, save it in variable
                    filename = new string(entry.FileName.Take(4).ToArray());
                    if (regionList.Contains(filename))
                    {
                        gameserial = entry.FileName.Replace(".", String.Empty);
                        gameserial = gameserial.Replace("_", "-");

                        Console.WriteLine("Serial = " + gameserial);
                    }
                }
            }

            using (var reader = new StreamReader(gameindex))
            {
                bool serialFound = false;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line.Contains("Serial = " + gameserial))
                    {
                        serialFound = true;
                    }
                    else if(serialFound == true)
                    {
                        Console.WriteLine(line);
                        break;
                    }
                }
            }

            Console.ReadLine();

        }
    }
}
