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

using DustInTheWind.ProjArchiver.Properties;
using NLog;

namespace DustInTheWind.ProjArchiver
{
    public class ArchiverService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IFileCompressor fileCompressor;
        private readonly IStorage storage;
        private readonly Config config;

        public ArchiverService(Config config, IStorage storage, IFileCompressor fileCompressor )
        {
            this.config = config;
            this.storage = storage;
            this.fileCompressor = fileCompressor;
        }

        public void Archive(string projectDirectoryFullPath, string description)
        {
            logger.Debug("Archive start");

            if (config.ArchivesDirectory == null)
                throw new ProjArchiveException(Resources.Err_ArchiveDirectoryNotSet);

            Archiver helper = new Archiver(storage, fileCompressor)
            {
                ArchivesDirectoryFullPath = config.ArchivesDirectory,
                ProjectDirectoryFullPath = projectDirectoryFullPath,
                Description = description
            };

            helper.Archive();

            logger.Debug("Archive end");
        }
    }
}