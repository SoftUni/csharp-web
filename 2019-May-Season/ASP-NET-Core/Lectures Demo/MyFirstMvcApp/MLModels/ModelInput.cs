//*****************************************************************************************
//*                                                                                       *
//* This is an auto-generated file by Microsoft ML.NET CLI (Command-Line Interface) tool. *
//*                                                                                       *
//*****************************************************************************************

using Microsoft.ML.Data;

namespace MyFirstMvcAppML.Model.DataModels
{
    public class ModelInput
    {
        [ColumnName("Size"), LoadColumn(0)]
        public float Size { get; set; }


        [ColumnName("Floor"), LoadColumn(1)]
        public float Floor { get; set; }


        [ColumnName("TotalFloors"), LoadColumn(2)]
        public float TotalFloors { get; set; }


        [ColumnName("District"), LoadColumn(3)]
        public string District { get; set; }


        [ColumnName("Year"), LoadColumn(4)]
        public float Year { get; set; }


        [ColumnName("Type"), LoadColumn(5)]
        public string Type { get; set; }


        [ColumnName("BuildingType"), LoadColumn(6)]
        public string BuildingType { get; set; }


        [ColumnName("Price"), LoadColumn(7)]
        public float Price { get; set; }


    }
}
