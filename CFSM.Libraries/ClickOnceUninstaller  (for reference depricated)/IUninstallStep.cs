using System;
using System.Collections.Generic;

namespace ClickOnceUninstaller
{
    public interface IUninstallStep : IDisposable
    {
        void Prepare(List<string> componentsToRemove);

        void PrintDebugInformation();

        void Execute();
    }
}
