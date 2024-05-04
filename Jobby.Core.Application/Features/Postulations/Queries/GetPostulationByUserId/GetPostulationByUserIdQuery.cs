using AutoMapper;
using Jobby.Core.Application.DTOS.Postulations;
using Jobby.Core.Application.Interfaces.Repositories;
using Jobby.Core.Application.Wrappers;
using Jobby.Core.Domain.Entities;
using MediatR;


namespace Jobby.Core.Application.Features.Postulations.Queries.GetPostulationByUserId
{
    public class GetPostulationByUserIdQuery : IRequest<Response<List<PostulationDto>>>
    {
        public string UserId { get; set; }
    }

    public class GetPostulationByUserIdQueryHandler : IRequestHandler<GetPostulationByUserIdQuery, Response<List<PostulationDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Postulation> _repository;
        
        public Task<Response<List<PostulationDto>>> Handle(GetPostulationByUserIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
