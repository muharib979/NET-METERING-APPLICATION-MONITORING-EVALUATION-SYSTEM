using Core.Application.Interfaces.ReplicationStatus;
using Shared.DTOs.ReplicationStatus;

namespace Core.Application.Queries.ReplicationStatus.GetAllReplicationStatusListQuery
{
    public class GetAllReplicationStatusListQuery : IRequest<List<ReplicationStatusDto>>
    {
        public string BillMonth { get; set; }
        public class Handler : IRequestHandler<GetAllReplicationStatusListQuery, List<ReplicationStatusDto>>
        {
            private readonly IReplicationStatusRepository _replicationRepository;
            private IMapper _mapper;
            public Handler(IReplicationStatusRepository replicationRepository, IMapper mapper)
            {
                _replicationRepository = replicationRepository;
                _mapper = mapper;
            }

            public async Task<List<ReplicationStatusDto>> Handle(GetAllReplicationStatusListQuery request, CancellationToken cancellationToken)
            {
                var result = _mapper.Map<List<ReplicationStatusDto>>( await _replicationRepository.GetAllReplicationStatusList(request.BillMonth));
                return result;
            }
        }
    }
}
