using BioAge.API.Entites;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML;
using Microsoft.ML.Data;
using CNTK;

namespace BioAge.API.Service.Command
{

    //public class TestInput
    //{
    //    [VectorType(10)]
    //    [ColumnName("input.1")]
    //    public float[] Features { get; set; }
    //}

    public class CalculateBioAgeByFunctionalParamsCommandHandler : IRequestHandler<CalculateBioAgeByFunctionalParamsCommand, double>
    {

        public CalculateBioAgeByFunctionalParamsCommandHandler()
        {
        }

        public async Task<double> Handle(CalculateBioAgeByFunctionalParamsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Dictionary<ParameterNames, PatientParameter> paramsDict = request.BioAgeCalculationParameters.Parameters;
                float systolicPressure = float.Parse(
                    paramsDict[ParameterNames.SystolicPressure].Value.Replace(",","."));
                float diastolicPressure = float.Parse(
                    paramsDict[ParameterNames.DiastolicPressure].Value.Replace(",", "."));
                float inhaleBreathHolding = float.Parse(
                    paramsDict[ParameterNames.InhaleBreathHolding].Value.Replace(",", "."));
                float outhaleBreathHolding = float.Parse(
                    paramsDict[ParameterNames.OuthaleBreathHolding].Value.Replace(",", "."));
                float lungCapacity = float.Parse(
                    paramsDict[ParameterNames.LungCapacity].Value.Replace(",", "."));
                float weight = float.Parse(
                    paramsDict[ParameterNames.Weight].Value.Replace(",", "."));
                float accommodation = float.Parse(
                    paramsDict[ParameterNames.Accommodation].Value.Replace(",", "."));
                float hearingAcuity = float.Parse(
                    paramsDict[ParameterNames.HearingAcuity].Value.Replace(",", "."));
                float staticBalancing = float.Parse(
                    paramsDict[ParameterNames.StaticBalancing].Value.Replace(",", "."));

                //string modelPath =Path.Combine(Directory.GetCurrentDirectory(), @"Resources/bioAgeFuncModel.onnx");

                //var a = DeviceDescriptor.AllDevices();
                //var deviceDescriptor = DeviceDescriptor.AllDevices().First();
                //var function = Function.Load("modelPath", deviceDescriptor, ModelFormat.ONNX);

                //var data = mlContext.Data.LoadFromEnumerable(new List<TestInput>());
                //var pipeline = mlContext.Transforms ApplyOnnxModel(modelFile: modelPath, outputColumnNames: new[] { "bioAge"}, inputColumnNames: new[] { "input.1" });
                //// Fit scoring pipeline
                //var model = pipeline.Fit(data);


                //var session = new InferenceSession(modelPath);

                //float[] input = { systolicPressure, diastolicPressure, 
                //    (systolicPressure - diastolicPressure), inhaleBreathHolding, 
                //    outhaleBreathHolding, lungCapacity, weight, accommodation, 
                //    hearingAcuity, staticBalancing };



                //Tensor<float> t1 = new DenseTensor<float>(input, new int[] {1,10});
               
                //var inputs = new List<NamedOnnxValue>() {NamedOnnxValue.CreateFromTensor<float>(null, t1)};
                //using(var outPut  = session.Run(inputs))
                //{
                //    var a = 1;
                //}

                return await Task.FromResult<double>(
                   - 1.07 * systolicPressure
                   + 1.1 * diastolicPressure
                   + 1.94 * (systolicPressure - diastolicPressure)
                   - 1.45 * inhaleBreathHolding
                   + 1.32 * outhaleBreathHolding
                   - 3.46 * lungCapacity
                   + 0.15 * weight
                   - 4.35 * accommodation
                   + 5.57 * hearingAcuity
                   - 2.6 * staticBalancing);
            }
            catch (KeyNotFoundException ex)
            {
                throw new BioAgeCalculationException("No necessary parameters in input", ex);
            }
            catch (Exception ex)
            {
                throw new BioAgeCalculationException("Unexpected exception", ex);
            }
        }
    }
}
