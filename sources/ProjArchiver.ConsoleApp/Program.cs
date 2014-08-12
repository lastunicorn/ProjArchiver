// ProjArchiver
// Copyright (C) 2014 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using NLog;

namespace DustInTheWind.ProjArchiver.ConsoleApp
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("ProjArchiver");
                Console.WriteLine("===============================================================================");
                Console.WriteLine();

                Config config = new Config
                {
                    ArchivesDirectory = @"c:\temp\archives"
                };

                Storage storage = new Storage();
                FileCompressor fileCompressor = new FileCompressor();

                ArchiverService archiverService = new ArchiverService(config, storage, fileCompressor);
                archiverService.Archive(@"c:\temp\projects\myproj", "bla bla bla");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            Console.WriteLine();
            Console.WriteLine("Bye!");

            Pause();
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
