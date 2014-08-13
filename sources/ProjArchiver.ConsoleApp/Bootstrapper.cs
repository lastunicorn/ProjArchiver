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
using System.Collections.Generic;
using DustInTheWind.ProjArchiver.ConsoleApp.Flows;
using Ninject;
using NLog;

namespace DustInTheWind.ProjArchiver.ConsoleApp
{
    class Bootstrapper
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private IKernel ninjectKernel;

        public void Run(string[] args)
        {
            try
            {
                CreateServices(args);

                MainFlow mainFlow = ninjectKernel.Get<MainFlow>();
                mainFlow.Execute();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void CreateServices(IList<string> args)
        {
            ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<IStorage>().To<Storage>().InSingletonScope();
            ninjectKernel.Bind<IFileCompressor>().To<SharpZipFileCompressor>().InSingletonScope();

            ConsoleOptions consoleOptions = new ConsoleOptions(args);
            ninjectKernel.Bind<ConsoleOptions>().ToConstant(consoleOptions);
        }
    }
}
