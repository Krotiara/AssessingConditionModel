using MediatR;
using Models.API.Data;
using Models.API.Entities;
using Razorvine.Pickle;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML;
using Models.API.Service.Service;

namespace Models.API.Service.Command
{
    public class PredictModelCommand: IRequest<float[]>
    {
        public ModelMeta ModelMeta { get; set; }

        public float[] InputArgs { get; set; }
    }

    public class PredictModelCommandHandler : IRequestHandler<PredictModelCommand, float[]>
    {
        private readonly ModelsStore _modelsStore;
        private readonly MLContext _mlContext;

        public PredictModelCommandHandler(ModelsStore modelsStore)
        {
            _modelsStore = modelsStore;
            _mlContext = new MLContext();
        }

        public async Task<float[]> Handle(PredictModelCommand request, CancellationToken cancellationToken)
        {
            MemoryStream model = await _modelsStore.Get(request.ModelMeta.File);
            DataViewSchema modelSchema;
            ITransformer trainedModel = _mlContext.Model.Load(model, out modelSchema);

            var runtimeType = new ClassFactory().CreateType(modelSchema);
            dynamic dynamicPredictionEngine;
            var genericPredictionMethod = _mlContext.Model.GetType().GetMethod("CreatePredictionEngine", new[] { typeof(ITransformer), typeof(DataViewSchema) });
            var predictionMethod = genericPredictionMethod.MakeGenericMethod(runtimeType, typeof(Prediction));
            dynamicPredictionEngine = predictionMethod.Invoke(_mlContext.Model, new object[] { trainedModel, modelSchema });
            var predictMethod = dynamicPredictionEngine.GetType().GetMethod("Predict", new[] { runtimeType });
            var predict = predictMethod.Invoke(dynamicPredictionEngine, new[] { request.InputArgs });
            return ((Prediction)predict).Predictions;
        }


        private float[] Predict(Tensor<float> t1, InferenceSession session, string inputArgsTitle)
        {
            NamedOnnxValue t1Value = NamedOnnxValue.CreateFromTensor(inputArgsTitle, t1); //float_input - всегда один и тот же?
            using (var outPut = session.Run(new List<NamedOnnxValue> { t1Value }))
            {
                DisposableNamedOnnxValue val = outPut.First();
                DenseTensor<float> value = val.Value as DenseTensor<float>;
                return value.Buffer.ToArray();
            }
        }
    }
}
