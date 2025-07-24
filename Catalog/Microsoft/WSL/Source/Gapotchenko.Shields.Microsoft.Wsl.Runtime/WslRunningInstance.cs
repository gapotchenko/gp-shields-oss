// Gapotchenko.Shields.Microsoft.Wsl
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Threading;
using Gapotchenko.Shields.Microsoft.Wsl.Deployment;

namespace Gapotchenko.Shields.Microsoft.Wsl.Runtime;

#if NET
[SupportedOSPlatform("linux")]
#endif
sealed class WslRunningInstance(string distributionName) : IWslRunningInstance
{
    public string DistributionName => distributionName;

    public IWslSetupInstance Setup => m_CachedSetupInstance.Value;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static EvaluateOnce<IWslSetupInstance> m_CachedSetupInstance = new(GetSetupInstanceCore);

    static IWslSetupInstance GetSetupInstanceCore() =>
        WslDeployment.EnumerateSetupInstances().FirstOrDefault() ??
        throw new InvalidOperationException("Cannot determine WSL setup instance.");
}
