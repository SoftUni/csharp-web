using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using MyFirstMvcAppML.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstMvcApp.Controllers
{
    public class MLController : Controller
    {
        private readonly PredictionEnginePool<ModelInput, ModelOutput> predictionEngine;

        public MLController(PredictionEnginePool<ModelInput, ModelOutput> predictionEngine)
        {
            this.predictionEngine = predictionEngine;
        }

        public IActionResult Predict()
        {
            var input = new ModelInput
            {
                BuildingType = "3-СТАЕН",
                District = "град София, Лозенец",
                Floor = 6,
                TotalFloors = 6,
                Size = 100,
                Year = 2017,
                Type = "Тухла"
            };

            var output = this.predictionEngine.Predict(input);
            return this.Content(output.Score.ToString());
        }
    }
}
