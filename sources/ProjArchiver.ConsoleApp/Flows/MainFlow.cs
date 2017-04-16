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
using Ninject;
using NLog;

namespace DustInTheWind.ProjArchiver.ConsoleApp.Flows
{
    class MainFlow
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly MainFlowView view;
        private readonly ConsoleOptions consoleOptions;
        private readonly IKernel ninjectKernel;

        public MainFlow(MainFlowView view, IKernel ninjectKernel, ConsoleOptions consoleOptions)
        {
            this.ninjectKernel = ninjectKernel;
            this.consoleOptions = consoleOptions;
            this.view = view;
        }

        public void Execute()
        {
            try
            {
                view.WriteStartApp();

                switch (consoleOptions.Action)
                {
                    case ConsoleAction.Archive:
                        RunArchiveFlow();
                        break;

                    case ConsoleAction.Restore:
                        RunRestoreFlow();
                        break;

                    case ConsoleAction.Init:
                        RunInitFlow();
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

        private void RunInitFlow()
        {
            InitFlow initFlow = ninjectKernel.Get<InitFlow>();
            initFlow.Execute();
        }

        private void RunArchiveFlow()
        {
            ArchiveFlow archiveFlow = ninjectKernel.Get<ArchiveFlow>();
            archiveFlow.Execute();
        }

        private void RunRestoreFlow()
        {
            RestoreFlow restoreFlow = ninjectKernel.Get<RestoreFlow>();
            restoreFlow.Execute();
        }
    }
}
