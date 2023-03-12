using MediatR;
using Models.API.Data;

namespace Models.API.Service.Command
{
    public class InsertModelCommand: IRequest<Unit>
    {

    }

    public class InsertModelCommandHandler : IRequestHandler<InsertModelCommand, Unit>
    {
        private readonly ModelsStore _modelsStore;

        public InsertModelCommandHandler(ModelsStore modelsStore)
        {
            _modelsStore = modelsStore;
        }

        public Task<Unit> Handle(InsertModelCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
