using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGitHub.CustomRestSharp;
using NGitHub.Models;
using RestSharp;

namespace NGitHub.Test.CustomRestSharp {
    [TestClass]
    public class CustomJsonDeserializerTests {
        [TestMethod]
        public void Deserialize_ShouldBeAbleToHandle_NullDateTimeValues() {
            var response = new RestResponse { Content = "[{\"closed_at\":null}]" };
            var serializer = new CustomJsonDeserializer();
            var issues = serializer.Deserialize<List<Issue>>(response);

            Assert.AreEqual(default(DateTime), issues[0].ClosedAt);
        }
    }
}
