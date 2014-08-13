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
    class App
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly AppView view;

        private Config config;
        private Storage storage;
        private SharpZipFileCompressor fileCompressor;
        private ConsoleOptions consoleOptions;

        public App()
        {
            view = new AppView();
        }

        public void Run(string[] args)
        {
            try
            {
                view.WriteStartApp();

                CreateServices(args);

                switch (consoleOptions.Action)
                {
                    case ConsoleAction.Archive:
                        RunArchiveFlow();
                        break;

                    case ConsoleAction.Restore:
                        RunRestoreFlow();
                        break;

                    case ConsoleAction.Init:
                        break;

                    default:
                        view.DisplayHelp();
                        break;

                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            view.WriteEndApp();
        }

        private void RunArchiveFlow()
        {
            ArchiveFlow archiveFlow = new ArchiveFlow(consoleOptions, config, storage, fileCompressor);
            archiveFlow.Execute();
        }

        private void RunRestoreFlow()
        {
            RestoreFlow restoreFlow = new RestoreFlow(consoleOptions, config, storage, fileCompressor);
            restoreFlow.Execute();
        }

        private void CreateServices(string[] args)
        {
            config = new Config();
            storage = new Storage();
            fileCompressor = new SharpZipFileCompressor();
            consoleOptions = new ConsoleOptions(args);
        }
    }
}
