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

using NLog;

namespace DustInTheWind.ProjArchiver.ConsoleApp.Flows
{
    class RestoreFlow
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ConsoleOptions consoleOptions;

        private readonly IFileCompressor fileCompressor;
        private readonly IStorage storage;
        private readonly Config config;

        public RestoreFlow(ConsoleOptions consoleOptions, Config config, IStorage storage, IFileCompressor fileCompressor)
        {
            this.consoleOptions = consoleOptions;
            this.config = config;
            this.storage = storage;
            this.fileCompressor = fileCompressor;
        }

        public void Execute()
        {
            if (consoleOptions.ProjectName == null)
                throw new ProjArchiveException("ProjectName is not specified.");

            if (consoleOptions.WorkDirectory == null)
                throw new ProjArchiveException("WorkDirectory is not specified.");

            Restore(consoleOptions.ProjectName, consoleOptions.WorkDirectory);
        }

        private void Restore(string projectName, string workDirectoryFullPath)
        {
            logger.Debug("Restore start");

            if (config.ArchivesDirectory == null)
                throw new ProjArchiveException("The ArchivesDirectoryFullPath was not specified.");

            Restorer restorer = new Restorer(storage, fileCompressor)
            {
                ArchivesDirectoryFullPath = config.ArchivesDirectory,
                WorkDirectoryFullPath = workDirectoryFullPath,
                ProjectName = projectName
            };

            restorer.Restore();

            logger.Debug("Restore end");
        }
    }
}
