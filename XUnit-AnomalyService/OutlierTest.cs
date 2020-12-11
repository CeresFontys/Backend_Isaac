using System;
using System.Collections.Generic;
using Isaac_AnomalyService.Models;
using Xunit;

namespace XUnit_AnomalyService
{
    //public class OutlierTest
    //{

    //    private readonly List<SensorData> mockData = new List<SensorData>();

    //    private readonly OutlierAlgo _OutlierAlgo = new OutlierAlgo(1 ,1 ,35, 15, 90, 10, 28, 18, 60, 30);


    //    public void SetupTest()
    //    {
    //        //Extreme anomalys
    //        mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 36, X = 1, Y = 1, Type = DataType.Temperature });
    //        mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 10, X = 1, Y = 1, Type = DataType.Temperature });
    //        mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 91, X = 1, Y = 1, Type = DataType.Humidity });
    //        mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 9, X = 1, Y = 1, Type = DataType.Humidity });

    //        //Normal anomalys
    //        mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 29, X = 1, Y = 1, Type = DataType.Temperature });
    //        mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 17, X = 1, Y = 1, Type = DataType.Temperature });
    //        mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 61, X = 1, Y = 1, Type = DataType.Humidity });
    //        mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 28, X = 1, Y = 1, Type = DataType.Humidity });

    //        //Next Higher
    //        mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 22, X = 2, Y = 2, Type = DataType.Temperature });
    //        mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 24, X = 2, Y = 2, Type = DataType.Temperature });
    //        mockData.Add(new SensorData() { DateTime = DateTime.Today, Floor = 3, Value = 22, X = 2, Y = 2, Type = DataType.Temperature });
    //        foreach (SensorData sensor in mockData)
    //        {
    //            _OutlierAlgo.SortData(sensor);
    //        }

    //    }

    //    [Fact]
    //    public void TestSorting()
    //    {
    //        SetupTest();
    //        Assert.Equal(7, _OutlierAlgo.temperatureSensorList.Count);
    //        Assert.Equal(4, _OutlierAlgo.humiditySensorList.Count);
    //    }

    //    [Fact]
    //    public void TestExtremeTop()
    //    {
    //        SetupTest();
    //        _OutlierAlgo.RunOutlierAlgo();
    //        var result = _OutlierAlgo.sensorErrorList.FindAll(x => x.Type == SensorError.ErrorType.ExtremeTop);
    //        Assert.Equal(2, result.Count);
    //    }

    //    [Fact]
    //    public void TestExtremeBot()
    //    {
    //        SetupTest();
    //        _OutlierAlgo.RunOutlierAlgo();
    //        var result = _OutlierAlgo.sensorErrorList.FindAll(x => x.Type == SensorError.ErrorType.ExtremeBottom);
    //        Assert.Equal(2, result.Count);
    //    }

    //    [Fact]
    //    public void TestNormalTop()
    //    {
    //        SetupTest();
    //        _OutlierAlgo.RunOutlierAlgo();
    //        var result = _OutlierAlgo.sensorErrorList.FindAll(x => x.Type == SensorError.ErrorType.NormalTop);
    //        Assert.Equal(2, result.Count);
    //    }

    //    [Fact]
    //    public void TestNormalBot()
    //    {
    //        SetupTest();
    //        _OutlierAlgo.RunOutlierAlgo();
    //        var result = _OutlierAlgo.sensorErrorList.FindAll(x => x.Type == SensorError.ErrorType.NormalBottom);
    //        Assert.Equal(2, result.Count);
    //    }

    //    [Fact]
    //    public void TestNextDif()
    //    {
    //        SetupTest();
    //        _OutlierAlgo.RunOutlierAlgo();
    //        var result = _OutlierAlgo.sensorErrorList.FindAll(x => x.Type == SensorError.ErrorType.NextDif);
    //        Assert.Equal(2, result.Count);
    //    }

    //    [Fact]
    //    public void TestPrevDif()
    //    {
    //        SetupTest();
    //        _OutlierAlgo.RunOutlierAlgo();
    //        var result = _OutlierAlgo.sensorErrorList.FindAll(x => x.Type == SensorError.ErrorType.PrevDif);
    //        Assert.Equal(2, result.Count);
    //    }
    //}
}
