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
    class ArchiveFlow
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ConsoleOptions consoleOptions;

        private readonly IFileCompressor fileCompressor;
        private readonly IStorage storage;
        private readonly Config config;

        public ArchiveFlow(ConsoleOptions consoleOptions, Config config, IStorage storage, IFileCompressor fileCompressor)
        {
            this.consoleOptions = consoleOptions;
            this.config = config;
            this.storage = storage;
            this.fileCompressor = fileCompressor;
        }

        public void Execute()
        {
            if (consoleOptions.ProjectDirectory == null)
                throw new ProjArchiveException("ProjectDirectory is not specified.");

            Archive(consoleOptions.ProjectDirectory, consoleOptions.Description ?? string.Empty);
        }

        private void Archive(string projectDirectoryFullPath, string description)
        {
            logger.Debug("Archive start");

            if (config.ArchivesDirectory == null)
                throw new ProjArchiveException("The ArchivesDirectoryFullPath was not specified.");

            Archiver archiver = new Archiver(storage, fileCompressor)
            {
                ArchivesDirectoryFullPath = config.ArchivesDirectory,
                ProjectDirectoryFullPath = projectDirectoryFullPath,
                Description = description
            };

            archiver.Archive();

            logger.Debug("Archive end");
        }
    }
}
