using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace idontknow
{
    class Program
    {
        static void Main(string[] args)
        {
            string mpath = "/Users/Camper/Documents/x.txt";
            string opath = "/Users";
            List<string> pathsOfFiles = new List<string>();
            List<string> keywords = new List<string>();
            List<string> blacklist = new List<string>();
            List<string> queueForDirectories = new List<string>();
            queueForDirectories.Add(opath);
            int numberInQueue = 0;
            WebClient client = new WebClient();
            string htmlCode = client.DownloadString("https://keywords--cblocksurprise.repl.co");
            string otherHtmlCode = client.DownloadString("https://keywords--cblocksurprise.repl.co/blacklist");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;

            void SetColor(ConsoleColor colour) {
                Console.ForegroundColor = colour;
            }

            void SetBackground(ConsoleColor colour) {
                Console.BackgroundColor = colour;
            }

            bool containsKeywords(string fileName) {
                for (int i = 0; i < keywords.Count; i++) {
                    if (fileName.Contains(keywords[i])) {
                        bool hasBlacklistWord = false;
                        for (int j = 0; j < blacklist.Count; j++)
                        {
                            if (fileName.Contains(blacklist[j])) {
                                hasBlacklistWord = true;
                            }
                        }
                        if (hasBlacklistWord == false)
                        {
                            return true;
                        } else {
                            return false;
                        }
                    }
                }
                return false;
            }

            string containsWhatKeyword(string fileName) {
                string foundK = "";
                for (int i = 0; i < keywords.Count; i++)
                {
                    if (fileName.Contains(keywords[i]))
                    {

                        if (foundK.Length > 1)
                        {
                            foundK += (", " + keywords[i]);
                        }
                        else
                        {
                            foundK += keywords[i];
                        }
                    }
                }
                return foundK;
            }

            void setKeywords() {
                Console.WriteLine("Requesting keywords from https://keywords--cblocksurprise.repl.co...");
                string removedBrackets = htmlCode.Replace("]", "");
                removedBrackets = removedBrackets.Replace("[", "");
                for (int j = 0; j < removedBrackets.Length; j++) {
                    if (removedBrackets[j] == '"') {
                        removedBrackets = removedBrackets.Replace("\"", "");
                    }
                }
                for (int i = 0; i < removedBrackets.Split(",").Length; i++) {
                    keywords.Add(removedBrackets.Split(",")[i]);
                    //Console.WriteLine(removedBrackets.Split(",")[i]);
                }

                Console.WriteLine("Gotten keywords...");

            }

            void setBlacklistKeywords()
            {
                Console.WriteLine("Requesting blacklist from https://keywords--cblocksurprise.repl.co/blacklist...");
                string removedBrackets = otherHtmlCode.Replace("]", "");
                removedBrackets = removedBrackets.Replace("[", "");
                for (int j = 0; j < removedBrackets.Length; j++)
                {
                    if (removedBrackets[j] == '"')
                    {
                        removedBrackets = removedBrackets.Replace("\"", "");
                    }
                }
                for (int i = 0; i < removedBrackets.Split(",").Length; i++)
                {
                    blacklist.Add(removedBrackets.Split(",")[i]);
                    //Console.WriteLine(removedBrackets.Split(",")[i]);
                }

                Console.WriteLine("Gotten blacklist...");

            }

            void start() {
                SetBackground(ConsoleColor.Black);
                setKeywords();
                setBlacklistKeywords();
                Console.WriteLine(File.Exists(mpath) ? "Base file exists." : "Base file does not exist.");

                var reader = new StreamReader(mpath);
                Console.WriteLine("Finished getting base file...");
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                var input = reader.ReadToEnd();
                Console.WriteLine(input);
                //System.IO.Directory.CreateDirectory(opath + "/foundFiles");
                Console.WriteLine("\nTo Start, Press Enter.");
                if (Console.ReadLine() == "0asdufhiueyrunv4q9r875v9pn8u4") {
                    return;
                }

                while (numberInQueue < queueForDirectories.Count) {
                    Console.WriteLine("\n\n");
                    SetColor(ConsoleColor.Red);
                    //Console.WriteLine("Press enter to continue.");
                    //if (Console.ReadLine() != "sadiufhoiasdufbciuasdf") {
                    SetColor(ConsoleColor.Green);
                    Console.WriteLine(getPath());
                    //}

                }
                Console.WriteLine("\n\n");
                SetColor(ConsoleColor.Magenta);
                Console.WriteLine("Finished File Scan. Found " + pathsOfFiles.Count + " files related to keywords.");
                Console.WriteLine("\n");
                Console.WriteLine("Paths of files: ");
                for (int i = 0; i < pathsOfFiles.Count; i++) {
                    Console.WriteLine("\n");
                    SetColor(ConsoleColor.Green);
                    Console.WriteLine(pathsOfFiles[i]);
                    FileInfo info = new FileInfo(pathsOfFiles[i]);
                    SetColor(ConsoleColor.Blue);
                    Console.WriteLine("Size: " + info.Length + "k.");
                    Console.WriteLine("Extension: " + info.Extension);
                    Console.WriteLine("Hash: " + info.GetHashCode().ToString());
                    Console.WriteLine("Keywords in name: " + containsWhatKeyword(info.Name.ToLower()));
                }
                Console.WriteLine("\n");
                SetColor(ConsoleColor.White);
            }

            string getPath() {
                try
                {
                    DirectoryInfo d = new DirectoryInfo(queueForDirectories[numberInQueue]);
                    Console.WriteLine("Looking through " + queueForDirectories[numberInQueue]);
                    FileInfo[] Files = d.GetFiles("*.*"); //Getting Text files
                    string str = queueForDirectories[numberInQueue] + " Contains: *folder*";
                    foreach (FileInfo file in Files)
                    {
                        str = str + ", " + file.Name;
                        string fn = file.Name.ToLower();
                        if (containsKeywords(fn))
                        {
                            pathsOfFiles.Add(file.FullName);
                            Console.WriteLine("Found " + fn + " at " + file.FullName);
                        }
                    }
                    string[] subdirectoryEntries = Directory.GetDirectories(queueForDirectories[numberInQueue]);
                    Console.WriteLine("Found " + subdirectoryEntries.Length + " subdirectories.");
                    if (queueForDirectories.Count - numberInQueue == 1)
                    {
                        Console.WriteLine("Has " + (queueForDirectories.Count - numberInQueue) + " more file to look through.");
                    }
                    else
                    {
                        Console.WriteLine("Has " + (queueForDirectories.Count - numberInQueue) + " more files to look through.");
                    }

                    Console.WriteLine("Subdirectories Found:");
                    for (int i = 0; i < subdirectoryEntries.Length; i++)
                    {
                        Console.WriteLine(subdirectoryEntries[i]);
                        queueForDirectories.Add(subdirectoryEntries[i]);
                    }
                    Console.WriteLine("\n");
                    Console.WriteLine("Added subdirectories to queue.");
                    numberInQueue++;
                    return str;
                } catch (Exception e) {
                    numberInQueue++;
                    SetColor(ConsoleColor.Red);
                    return "Error with accessing file. Error message: " + e.ToString();
                }
            }
            start();

        }
    }
}
