using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Diagnostics;

namespace ServerLibrary.Helpers
{
    public class SystemResourcesHealthCheck : IHealthCheck
    {
        private const double CpuLimitMinutes = 60.0; // Azure plan gratuito
        private const double MemoryLimitMiB = 1024.0;

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // Obtener memoria usada
            var currentProcess = Process.GetCurrentProcess();

            // Tiempo total de CPU desde el inicio del proceso (aproximación)
            var cpuTimeMinutes = currentProcess.TotalProcessorTime.TotalMinutes;

            // Memoria usada por el proceso (WorkingSet) en MiB
            var memoryUsedMiB = currentProcess.WorkingSet64 / (1024.0 * 1024.0);

            // Mensaje de salida
            var statusMessage = $"CPU: {cpuTimeMinutes:F2} min / {CpuLimitMinutes} min, Memoria: {memoryUsedMiB:F2} MiB / {MemoryLimitMiB} MiB";

            if (cpuTimeMinutes >= CpuLimitMinutes || memoryUsedMiB >= MemoryLimitMiB)
            {
                return Task.FromResult(
                    HealthCheckResult.Unhealthy("Límites de recursos superados: " + statusMessage)
                );
            }

            return Task.FromResult(
                HealthCheckResult.Healthy("Uso dentro de límites: " + statusMessage)
            );
        }
    }
}
