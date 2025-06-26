using Shared.DTOs.ReplicationStatus;

namespace Core.Application.Interfaces.ReplicationStatus
{
    public interface IReplicationStatusRepository
    {
        Task<List<Shared.DTOs.ReplicationStatus.ReplicationStatus>> GetAllReplicationStatusList(string billMonth);
    }
}
