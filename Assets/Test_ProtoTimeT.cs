using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AgentSimulation;
using EMgine;

public class Test_ProtoTimeT
{
    // A Test behaves as an ordinary method
    [Test]
    public void TimeTest()
    {
        ProtoTimeT testTime1 = new ProtoTimeT(1.0f);
        ProtoTimeT testTime2 = new ProtoTimeT(2.0f);
        Assert.That(testTime2.IsAfter(testTime1));

        ProtoTimeT.ProtoDeltaTimeT deltaTime1 = (ProtoTimeT.ProtoDeltaTimeT)testTime1.Difference(testTime2); // 1 - 2 = -1


        Assert.That(testTime2.IsAfter(testTime1));

    }

    [Test]
    public void DeltaTimeTests()
    {
        ProtoTimeT.ProtoDeltaTimeT deltaTime1 = new ProtoTimeT.ProtoDeltaTimeT(0.5f);
        ProtoTimeT.ProtoDeltaTimeT deltaTime2 = new ProtoTimeT.ProtoDeltaTimeT(1.5f);

        ProtoTimeT.ProtoDeltaTimeT deltaSum = (ProtoTimeT.ProtoDeltaTimeT)deltaTime1.SumWith(deltaTime2); // 2
        Assert.AreEqual(deltaSum.GetAsValue(), 2.0f);

        ProtoTimeT.ProtoDeltaTimeT deltaDiff = (ProtoTimeT.ProtoDeltaTimeT)deltaTime1.Difference(deltaTime2); // -1
        Assert.AreEqual(deltaDiff.GetAsValue(), -1.0f);

        ProtoTimeT.ProtoDeltaTimeT deltaMult = (ProtoTimeT.ProtoDeltaTimeT)deltaTime1.MultiplyByValue(0.0d); // 0
        Assert.AreEqual(deltaMult.GetAsValue(), 0.0f);


        int deltaCompare = deltaTime1.CompareTo(deltaTime2); // -1
        Assert.AreEqual(deltaCompare, -1);

        Assert.That(deltaDiff.IsNegative());

        Assert.That(deltaMult.IsZero());

        ProtoTimeT.ProtoDeltaTimeT deltaCopy = (ProtoTimeT.ProtoDeltaTimeT)deltaTime1.Copy();
        Assert.AreEqual(deltaCopy.GetAsValue(), deltaTime1.GetAsValue());

    }
}